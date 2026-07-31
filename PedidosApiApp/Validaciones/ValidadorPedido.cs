using PedidosApiApp.DTOs;

namespace PedidosApiApp.Validaciones;

public static class ValidadorPedido
{
    public static (bool EsValido, string? MensajeError) Validar(CrearPedidoRequest solicitud)
    {
        if (solicitud.ClienteId <= 0)
            return (false, "El campo 'clienteId' debe ser un entero mayor a 0.");

        if (string.IsNullOrWhiteSpace(solicitud.Usuario))
            return (false, "El campo 'usuario' es obligatorio.");

        if (solicitud.Items == null || !solicitud.Items.Any())
            return (false, "El pedido debe incluir al menos un producto en la lista 'items'.");

        foreach (var item in solicitud.Items)
        {
            if (item.ProductoId <= 0)
                return (false, "El 'productoId' debe ser mayor a 0.");

            if (item.Cantidad <= 0)
                return (false, $"La cantidad para el producto {item.ProductoId} debe ser mayor a 0.");

            if (item.Precio <= 0)
                return (false, $"El precio para el producto {item.ProductoId} debe ser mayor a 0.");
        }

        return (true, null);
    }
}