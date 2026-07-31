namespace PedidosApiApp.Modelos;

public class LogAuditoria
{
    public int Id { get; set; }
    public DateTime Fecha { get; set; } = DateTime.UtcNow;
    public string Evento { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public int? PedidoId { get; set; }

    public PedidoCabecera? Pedido { get; set; }
}