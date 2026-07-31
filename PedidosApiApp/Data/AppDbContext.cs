using Microsoft.EntityFrameworkCore;
using PedidosApiApp.Modelos;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace PedidosApiApp.Data;

public class AppDbContext : Microsoft.EntityFrameworkCore.DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<PedidoCabecera> PedidoCabeceras { get; set; } = null!;
    public DbSet<PedidoDetalle> PedidoDetalles { get; set; } = null!;
    public DbSet<LogAuditoria> LogAuditorias { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<PedidoCabecera>().ToTable("PedidoCabecera");
        modelBuilder.Entity<PedidoDetalle>().ToTable("PedidoDetalle");
        modelBuilder.Entity<LogAuditoria>().ToTable("LogAuditoria");

        modelBuilder.Entity<PedidoCabecera>()
            .HasMany(p => p.Detalles)
            .WithOne(d => d.Pedido)
            .HasForeignKey(d => d.PedidoId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}