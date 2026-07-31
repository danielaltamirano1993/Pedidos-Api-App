# Pedidos API (.NET 8) 🛒

API REST para el registro transaccional de pedidos con validación de cliente externa y registro de auditoría.

---

## 🚀 Tecnologías

- **Framework:** .NET 8 Web API
- **ORM:** Entity Framework Core
- **Base de Datos:** SQL Server
- **Documentación:** Swagger

---

## Configuración Rápida

1. **Base de Datos:** Ejecuta el archivo `scripts/script.sql` en SQL Server para crear la base de datos `SistemaPedidosDb`.
2. **Cadena de Conexión:** Revisa o actualiza la propiedad `CadenaSql` en `PedidosApiApp/appsettings.json`:
   
   ```json
   "ConnectionStrings": {
     "CadenaSql": "Server=THE-MONJE-DAN\\MONJEDAN;Database=SistemaPedidosDb;Trusted_Connection=True;TrustServerCertificate=True;"
   }
```

## Endpoint Principal
POST /api/pedidos

   ```json
{
  "clienteId": 1,
  "usuario": "daniel.altamirano",
  "items": [
    {
      "productoId": 10,
      "cantidad": 2,
      "precio": 15.50
    },
    {
      "productoId": 12,
      "cantidad": 1,
      "precio": 25.00
    }
  ]
}
```