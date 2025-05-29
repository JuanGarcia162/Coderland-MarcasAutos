using Coderland.API.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;

namespace Coderland.API.Infrastructure.Persistence
{
    // Contexto de EF Core para conectar con PostgreSQL
    // Aquí definimos las tablas y sus relaciones
    public class ApplicationDbContext : DbContext
    {
        // Constructor estándar que recibe las opciones de configuración
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            // No creamos la BD aquí para evitar problemas en las pruebas
            // Eso lo manejamos en Program.cs
        }

        // Tabla de marcas de autos en la BD
        public DbSet<MarcaAuto> MarcasAutos { get; set; }

        // Configuración del modelo y datos iniciales
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuramos la tabla MarcasAutos
            modelBuilder.Entity<MarcaAuto>(entity =>
            {
                entity.ToTable("MarcasAutos");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Nombre).IsRequired().HasMaxLength(100); // Nombre obligatorio, máx 100 chars
                entity.Property(e => e.PaisOrigen).IsRequired().HasMaxLength(50); // País obligatorio, máx 50 chars
                entity.Property(e => e.AnioFundacion).IsRequired(); // Año obligatorio
                entity.Property(e => e.Activo).IsRequired(); // Estado obligatorio
            });

            // Datos iniciales para tener algo en la BD desde el principio
            modelBuilder.Entity<MarcaAuto>().HasData(
                new MarcaAuto
                {
                    Id = 1,
                    Nombre = "Toyota",
                    PaisOrigen = "Japón",
                    AnioFundacion = 1937,
                    Activo = true
                },
                new MarcaAuto
                {
                    Id = 2,
                    Nombre = "Volkswagen",
                    PaisOrigen = "Alemania",
                    AnioFundacion = 1937,
                    Activo = true
                },
                new MarcaAuto
                {
                    Id = 3,
                    Nombre = "Ford",
                    PaisOrigen = "Estados Unidos",
                    AnioFundacion = 1903,
                    Activo = true
                }
            );
        }
    }
}
