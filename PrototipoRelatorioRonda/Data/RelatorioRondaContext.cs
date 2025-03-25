using Microsoft.EntityFrameworkCore;
using PrototipoRelatorioRonda.Models;

namespace PrototipoRelatorioRonda.Data;

public class RelatorioRondaContext : DbContext
{
    public RelatorioRondaContext(DbContextOptions<RelatorioRondaContext> options) : base(options) { }

    public DbSet<Empresa> Empresas { get; set; }
    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<RelatorioRonda> RelatorioRondas { get; set; }
    public DbSet<VoltaRonda> VoltaRondas { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configurações da Empresa
        modelBuilder.Entity<Empresa>(entity =>
        {
            entity.ToTable("Empresa");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Nome).IsRequired().HasMaxLength(100);
            entity.HasIndex(e => e.Nome).IsUnique();

            // Filtro global para sempre trazer apenas empresas ativas
            entity.HasQueryFilter(e => e.Ativo);
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.ToTable("Usuarios");
            entity.HasKey(u => u.Id);
            entity.Property(u => u.Nome).IsRequired().HasMaxLength(100);
            entity.Property(u => u.Email).IsRequired().HasMaxLength(100);
            entity.Property(u => u.HashSenha).IsRequired();

            // Índices para otimizar consultas
            entity.HasIndex(u => u.Nome).IsUnique();
            entity.HasIndex(u => u.Email).IsUnique();

            // Relacionamento com Empresa
            entity.HasOne(u => u.Empresa)
                  .WithMany(e => e.Usuarios)
                  .HasForeignKey(u => u.EmpresaId)
                  .OnDelete(DeleteBehavior.Restrict); // Evita exclusão em cascata

            // Filtro global para sempre trazer apenas usuários ativos
            entity.HasQueryFilter(u => u.Ativo);
        });

        // Configurações do RelatorioRonda
        modelBuilder.Entity<RelatorioRonda>(entity =>
        {
            entity.ToTable("RelatoriosRonda");
            entity.HasKey(r => r.Id);
            entity.Property(r => r.Data).IsRequired();

            // Relacionamentos
            entity.HasOne(r => r.Empresa)
                  .WithMany()
                  .HasForeignKey(r => r.EmpresaId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(r => r.Vigilante)
                  .WithMany()
                  .HasForeignKey(r => r.VigilanteId)
                  .OnDelete(DeleteBehavior.Restrict);

            // Índice composto para busca otimizada
            entity.HasIndex(r => new { r.EmpresaId, r.Data });
        });

        // Configurações das VoltasRonda
        modelBuilder.Entity<VoltaRonda>(entity =>
        {
            entity.ToTable("VoltasRonda");
            entity.HasKey(v => v.Id);

            // Relacionamento com RelatorioRonda
            entity.HasOne(v => v.RelatorioRonda)
                  .WithMany(r => r.Voltas)
                  .HasForeignKey(v => v.RelatorioRondaId)
                  .OnDelete(DeleteBehavior.Cascade); // Exclusão em cascata é útil aqui

            // Garante que NumeroVolta seja único dentro de um mesmo relatório
            entity.HasIndex(v => new { v.RelatorioRondaId, v.NumeroVolta }).IsUnique();
        });
    }

    // Sobrescrever SaveChanges para automatizar CriadoEm
    public override int SaveChanges()
    {
        var entries = ChangeTracker
            .Entries()
            .Where(e => e.Entity is BaseModel &&
                         (e.State == EntityState.Added));

        foreach (var entityEntry in entries)
        {
            ((BaseModel)entityEntry.Entity).CriadoEm = DateTime.Now;
        }

        return base.SaveChanges();
    }
}
