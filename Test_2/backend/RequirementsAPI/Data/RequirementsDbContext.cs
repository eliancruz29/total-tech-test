using Microsoft.EntityFrameworkCore;
using RequirementsAPI.Models.Entities;

namespace RequirementsAPI.Data;

public class RequirementsDbContext : DbContext
{
    public RequirementsDbContext(DbContextOptions<RequirementsDbContext> options)
        : base(options)
    {
    }

    public DbSet<Proceso> Procesos { get; set; }
    public DbSet<Subproceso> Subprocesos { get; set; }
    public DbSet<CasoUso> CasosUso { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Proceso configuration
        modelBuilder.Entity<Proceso>(entity =>
        {
            entity.ToTable("proceso");
            entity.HasKey(e => e.IdProceso);
            entity.Property(e => e.IdProceso).HasColumnName("id_proceso");
            entity.Property(e => e.Nombre).HasColumnName("nombre").HasMaxLength(150).IsRequired();
            entity.Property(e => e.Descripcion).HasColumnName("descripcion");
            entity.Property(e => e.RequirementText).HasColumnName("requirement_text");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at").HasDefaultValueSql("GETDATE()");

            entity.HasMany(e => e.Subprocesos)
                .WithOne(e => e.Proceso)
                .HasForeignKey(e => e.IdProceso)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Subproceso configuration
        modelBuilder.Entity<Subproceso>(entity =>
        {
            entity.ToTable("subproceso");
            entity.HasKey(e => e.IdSubproceso);
            entity.Property(e => e.IdSubproceso).HasColumnName("id_subproceso");
            entity.Property(e => e.IdProceso).HasColumnName("id_proceso");
            entity.Property(e => e.Nombre).HasColumnName("nombre").HasMaxLength(150).IsRequired();
            entity.Property(e => e.Descripcion).HasColumnName("descripcion");

            entity.HasMany(e => e.CasosUso)
                .WithOne(e => e.Subproceso)
                .HasForeignKey(e => e.IdSubproceso)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // CasoUso configuration
        modelBuilder.Entity<CasoUso>(entity =>
        {
            entity.ToTable("caso_uso");
            entity.HasKey(e => e.IdCasoUso);
            entity.Property(e => e.IdCasoUso).HasColumnName("id_caso_uso");
            entity.Property(e => e.IdSubproceso).HasColumnName("id_subproceso");
            entity.Property(e => e.Nombre).HasColumnName("nombre").HasMaxLength(150).IsRequired();
            entity.Property(e => e.Descripcion).HasColumnName("descripcion");
            entity.Property(e => e.ActorPrincipal).HasColumnName("actor_principal").HasMaxLength(150);
            entity.Property(e => e.TipoCasoUso).HasColumnName("tipo_caso_uso");
            entity.Property(e => e.Precondiciones).HasColumnName("precondiciones");
            entity.Property(e => e.Postcondiciones).HasColumnName("postcondiciones");
            entity.Property(e => e.CriteriosDeAceptacion).HasColumnName("criterios_de_aceptacion");
        });
    }
}

