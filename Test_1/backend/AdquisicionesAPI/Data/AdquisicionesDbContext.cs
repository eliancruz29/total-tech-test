using Microsoft.EntityFrameworkCore;
using AdquisicionesAPI.Models.Entities;

namespace AdquisicionesAPI.Data;

public class AdquisicionesDbContext : DbContext
{
    public AdquisicionesDbContext(DbContextOptions<AdquisicionesDbContext> options)
        : base(options)
    {
    }

    // Main entities
    public DbSet<SpartanUser> SpartanUsers { get; set; }
    public DbSet<Pedido> Pedidos { get; set; }
    public DbSet<PedidoDetalle> PedidoDetalles { get; set; }
    public DbSet<CatProveedor> Proveedores { get; set; }
    public DbSet<CatInsumo> Insumos { get; set; }
    public DbSet<Requisicion> Requisiciones { get; set; }
    public DbSet<ProcedimientoAdquisicion> ProcedimientosAdquisicion { get; set; }

    // Catalog entities
    public DbSet<CatEstadoPedido> EstadosPedido { get; set; }
    public DbSet<CatEstadoSurtido> EstadosSurtido { get; set; }
    public DbSet<CatTipoPedido> TiposPedido { get; set; }
    public DbSet<CatTipoDocumentoPedido> TiposDocumentoPedido { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure Pedido entity
        modelBuilder.Entity<Pedido>(entity =>
        {
            entity.HasKey(e => e.IdPedido);
            entity.HasIndex(e => e.Folio).IsUnique();
            entity.HasIndex(e => e.FechaPedido);
            entity.HasIndex(e => e.IdEstadoPedido);
            entity.HasIndex(e => e.IdProveedor);

            // Relationships
            entity.HasOne(e => e.TipoDocumentoPedido)
                .WithMany(t => t.Pedidos)
                .HasForeignKey(e => e.IdTipoDocumentoPedido)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.TipoPedido)
                .WithMany(t => t.Pedidos)
                .HasForeignKey(e => e.IdTipoPedido)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Proveedor)
                .WithMany(p => p.Pedidos)
                .HasForeignKey(e => e.IdProveedor)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.ProcedimientoAdquisicion)
                .WithMany(p => p.Pedidos)
                .HasForeignKey(e => e.IdProcedimientoAdquisicion)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.EstadoPedido)
                .WithMany(e => e.Pedidos)
                .HasForeignKey(e => e.IdEstadoPedido)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.EstadoSurtido)
                .WithMany(e => e.Pedidos)
                .HasForeignKey(e => e.IdEstadoSurtido)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.UsuarioRegistro)
                .WithMany(u => u.PedidosRegistrados)
                .HasForeignKey(e => e.IdUsuarioRegistro)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.UsuarioModifica)
                .WithMany()
                .HasForeignKey(e => e.IdUsuarioModifica)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.UsuarioAprueba)
                .WithMany()
                .HasForeignKey(e => e.IdUsuarioAprueba)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Configure PedidoDetalle entity
        modelBuilder.Entity<PedidoDetalle>(entity =>
        {
            entity.HasKey(e => e.IdPedidoDetalle);
            entity.HasIndex(e => e.IdPedido);

            // Relationships
            entity.HasOne(e => e.Pedido)
                .WithMany(p => p.PedidoDetalles)
                .HasForeignKey(e => e.IdPedido)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Requisicion)
                .WithMany(r => r.PedidoDetalles)
                .HasForeignKey(e => e.IdRequisicion)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Insumo)
                .WithMany(i => i.PedidoDetalles)
                .HasForeignKey(e => e.IdInsumo)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Configure SpartanUser entity
        modelBuilder.Entity<SpartanUser>(entity =>
        {
            entity.HasKey(e => e.IdUser);
            entity.HasIndex(e => e.Email).IsUnique();
        });

        // Configure CatProveedor entity
        modelBuilder.Entity<CatProveedor>(entity =>
        {
            entity.HasKey(e => e.IdProveedor);
            entity.HasIndex(e => e.Rfc).IsUnique();
            entity.HasIndex(e => e.RazonSocial);

            entity.HasOne(e => e.UsuarioRegistro)
                .WithMany(u => u.ProveedoresRegistrados)
                .HasForeignKey(e => e.IdUsuarioRegistro)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Configure CatInsumo entity
        modelBuilder.Entity<CatInsumo>(entity =>
        {
            entity.HasKey(e => e.IdInsumo);
            entity.HasIndex(e => e.Codigo).IsUnique();
        });

        // Configure Requisicion entity
        modelBuilder.Entity<Requisicion>(entity =>
        {
            entity.HasKey(e => e.IdRequisicion);
            entity.HasIndex(e => e.Folio).IsUnique();

            entity.HasOne(e => e.UsuarioSolicitante)
                .WithMany(u => u.RequisicionesSolicitadas)
                .HasForeignKey(e => e.IdUsuarioSolicitante)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.ProcedimientoAdquisicion)
                .WithMany()
                .HasForeignKey(e => e.IdProcedimientoAdquisicion)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Configure ProcedimientoAdquisicion entity
        modelBuilder.Entity<ProcedimientoAdquisicion>(entity =>
        {
            entity.HasKey(e => e.IdProcedimientoAdquisicion);
            entity.HasIndex(e => e.Identificador).IsUnique();
        });

        // Configure catalog entities
        modelBuilder.Entity<CatEstadoPedido>(entity =>
        {
            entity.HasKey(e => e.IdEstadoPedido);
            entity.HasIndex(e => e.Descripcion).IsUnique();
        });

        modelBuilder.Entity<CatEstadoSurtido>(entity =>
        {
            entity.HasKey(e => e.IdEstadoSurtido);
            entity.HasIndex(e => e.Descripcion).IsUnique();
        });

        modelBuilder.Entity<CatTipoPedido>(entity =>
        {
            entity.HasKey(e => e.IdTipoPedido);
            entity.HasIndex(e => e.Descripcion).IsUnique();
        });

        modelBuilder.Entity<CatTipoDocumentoPedido>(entity =>
        {
            entity.HasKey(e => e.IdTipoDocumentoPedido);
            entity.HasIndex(e => e.Descripcion).IsUnique();
        });
    }
}
