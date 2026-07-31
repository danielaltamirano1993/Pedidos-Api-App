USE SistemaPedidosDb;
GO
  
SELECT 
TOP (1000) [Id]
,[ClienteId]
,[Fecha]
,[Total]
,[Usuario]
,[Estado]
FROM [SistemaPedidosDb].[dbo].[PedidoCabecera]


SELECT 
TOP (1000) [Id]
,[PedidoId]
,[ProductoId]
,[Cantidad]
,[Precio]
FROM [SistemaPedidosDb].[dbo].[PedidoDetalle]


SELECT 
TOP (1000) [Id]
,[Fecha]
,[Evento]
,[Descripcion]
,[PedidoId]
FROM [SistemaPedidosDb].[dbo].[LogAuditoria]
