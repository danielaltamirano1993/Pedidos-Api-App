CREATE DATABASE SistemaPedidosDb;
GO

USE SistemaPedidosDb;
GO

CREATE TABLE PedidoCabecera (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    ClienteId INT NOT NULL,
    Fecha DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    Total DECIMAL(18, 2) NOT NULL,
    Usuario VARCHAR(100) NOT NULL,
    Estado VARCHAR(30) NOT NULL DEFAULT 'COMPLETADO'
);

CREATE TABLE PedidoDetalle (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    PedidoId INT NOT NULL,
    ProductoId INT NOT NULL,
    Cantidad INT NOT NULL,
    Precio DECIMAL(18, 2) NOT NULL,
    CONSTRAINT FK_PedidoDetalle_PedidoCabecera FOREIGN KEY (PedidoId) 
        REFERENCES PedidoCabecera(Id) ON DELETE CASCADE
);

CREATE TABLE LogAuditoria (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Fecha DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    Evento VARCHAR(100) NOT NULL,
    Descripcion VARCHAR(MAX) NOT NULL,
    PedidoId INT NULL,
    CONSTRAINT FK_LogAuditoria_PedidoCabecera FOREIGN KEY (PedidoId) 
        REFERENCES PedidoCabecera(Id) ON DELETE SET NULL
);

CREATE INDEX IX_PedidoCabecera_ClienteId ON PedidoCabecera(ClienteId);
CREATE INDEX IX_PedidoDetalle_PedidoId ON PedidoDetalle(PedidoId);