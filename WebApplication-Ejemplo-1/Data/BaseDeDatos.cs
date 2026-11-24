using Microsoft.EntityFrameworkCore;
using WebApplication_Ejemplo_1.Models;

namespace WebApplication_Ejemplo_1.Data;

public sealed class BaseDeDatos(DbContextOptions<BaseDeDatos> options) : DbContext(options)
{
    public DbSet<Tutor> Tutores => Set<Tutor>();
    public DbSet<Tesis> Tesis => Set<Tesis>();
    public DbSet<Usuario> Usuarios => Set<Usuario>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Tutor>(entity =>
        {
            entity.Property(tutor => tutor.Nombre).HasMaxLength(150).IsRequired();
            entity.Property(tutor => tutor.Especialidad).HasMaxLength(120).IsRequired();
            entity.Property(tutor => tutor.Correo).HasMaxLength(254).IsRequired();
            entity.HasIndex(tutor => tutor.Correo).IsUnique();
        });

        modelBuilder.Entity<Tesis>(entity =>
        {
            entity.Property(tesis => tesis.Titulo)
                .UseCollation("Latin1_General_100_CI_AS")
                .HasMaxLength(250)
                .IsRequired();
            entity.Property(tesis => tesis.Estudiante).HasMaxLength(150).IsRequired();
            entity.Property(tesis => tesis.Estado).HasMaxLength(50).IsRequired();
            entity.Property(tesis => tesis.Imagen).HasMaxLength(300);
            entity.HasIndex(tesis => tesis.Titulo).IsUnique();
            entity.HasIndex(tesis => tesis.IdTutor);
            entity.HasOne(tesis => tesis.Tutor)
                .WithMany(tutor => tutor.Tesis)
                .HasForeignKey(tesis => tesis.IdTutor)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.Property(usuario => usuario.NombreUsuario).HasMaxLength(100).IsRequired();
            entity.Property(usuario => usuario.PasswordHash).IsRequired();
            entity.Property(usuario => usuario.Rol).HasMaxLength(50).IsRequired();
            entity.HasIndex(usuario => usuario.NombreUsuario).IsUnique();
        });
    }
}
