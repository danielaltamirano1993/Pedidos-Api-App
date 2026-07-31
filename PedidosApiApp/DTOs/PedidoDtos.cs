namespace PedidosApiApp.DTOs;

public record CrearPedidoRequest(
    int ClienteId,
    string Usuario,
    List<ElementoPedidoDto> Items
);

public record ElementoPedidoDto(
    int ProductoId,
    int Cantidad,
    decimal Precio
);

public record CrearPedidoResponse(
    int PedidoId,
    decimal Total,
    DateTime Fecha,
    string Mensaje
);