using Microsoft.AspNetCore.Mvc;
using PedidosApiApp.Data;
using PedidosApiApp.DTOs;
using PedidosApiApp.Modelos;
using PedidosApiApp.Servicios;
using PedidosApiApp.Validaciones;

namespace PedidosApiApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PedidosController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IServicioValidacionCliente _validacionCliente;
    private readonly ILogger<PedidosController> _logger;

    public PedidosController(
        AppDbContext context,
        IServicioValidacionCliente validacionCliente,
        ILogger<PedidosController> logger)
    {
        _context = context;
        _validacionCliente = validacionCliente;
        _logger = logger;
    }

    [HttpPost]
    public async Task<IActionResult> CrearPedido([FromBody] CrearPedidoRequest solicitud)
    {
        // 1. Validación sintáctica/Estructural
        var (esValido, mensajeError) = ValidadorPedido.Validar(solicitud);
        if (!esValido)
        {
            _logger.LogWarning("Validación fallida: {Mensaje}", mensajeError);
            return BadRequest(new { mensaje = mensajeError });
        }

        // 2. Validación de existencia del cliente mediante servicio externo
        bool clienteExiste = await _validacionCliente.ExisteClienteAsync(solicitud.ClienteId);
        if (!clienteExiste)
        {
            _logger.LogWarning("El cliente con ID {ClienteId} no existe en el servicio externo.", solicitud.ClienteId);
            return NotFound(new { mensaje = $"El cliente con ID {solicitud.ClienteId} no fue encontrado o no existe." });
        }

        // 3. Manejo de Transacción Atómica
        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            decimal totalCalculado = solicitud.Items.Sum(item => item.Cantidad * item.Precio);

            var cabecera = new PedidoCabecera
            {
                ClienteId = solicitud.ClienteId,
                Usuario = solicitud.Usuario,
                Total = totalCalculado,
                Fecha = DateTime.UtcNow,
                Estado = "COMPLETADO",
                Detalles = solicitud.Items.Select(item => new PedidoDetalle
                {
                    ProductoId = item.ProductoId,
                    Cantidad = item.Cantidad,
                    Precio = item.Precio
                }).ToList()
            };

            _context.PedidoCabeceras.Add(cabecera);
            await _context.SaveChangesAsync();

            // Registro en tabla LogAuditoria
            var logAuditoria = new LogAuditoria
            {
                Fecha = DateTime.UtcNow,
                Evento = "CREACION_PEDIDO",
                Descripcion = $"Pedido registrado con éxito. Cliente: {solicitud.ClienteId}, Total: {totalCalculado}",
                PedidoId = cabecera.Id
            };

            _context.LogAuditorias.Add(logAuditoria);
            await _context.SaveChangesAsync();

            // Confirmar transacción
            await transaction.CommitAsync();

            _logger.LogInformation("Pedido #{PedidoId} procesado exitosamente.", cabecera.Id);

            var respuesta = new CrearPedidoResponse(
                cabecera.Id,
                cabecera.Total,
                cabecera.Fecha,
                "Pedido creado satisfactoriamente."
            );

            return CreatedAtAction(nameof(CrearPedido), new { id = cabecera.Id }, respuesta);
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            _logger.LogError(ex, "Error crítico durante el procesamiento del pedido. Se ejecutó Rollback.");
            return StatusCode(500, new { mensaje = "Ocurrió un error interno al procesar la transacción." });
        }
    }
}