using Coderland.API.Application.Interfaces;
using Coderland.API.Application.Services;
using Coderland.API.Domain.Entities;
using Coderland.API.Domain.Interfaces;
using Coderland.API.Infrastructure.Persistence;
using Coderland.API.Infrastructure.Repositories;
using Coderland.API.Presentation.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Coderland.Test
{
    // Tests para verificar que el controlador de marcas de autos funciona correctamente
    // Usamos base de datos en memoria para no depender de PostgreSQL en las pruebas
    public class MarcasAutosControllerTests
    {
        // Test que verifica que el endpoint GET devuelve correctamente todas las marcas
        // Comprueba que la respuesta sea 200 OK y contenga las marcas que esperamos
        [Fact]
        public async Task GetAll_ReturnsAllMarcasAutos()
        {
            // Arrange - Configuramos una BD en memoria con nombre único para este test
            var dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: $"CoderlandBD_{Guid.NewGuid()}") // Usamos GUID para evitar conflictos
                .Options;

            // Creamos y configuramos la BD en memoria para este test
            using (var context = new ApplicationDbContext(dbContextOptions))
            {
                // Limpiamos la BD para asegurarnos de empezar desde cero
                // Esto evita que datos de otros tests interfieran con este
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
                
                // Agregamos datos de prueba que usaremos para verificar las respuestas
                // Estos son los datos que esperamos recibir del endpoint
                context.MarcasAutos.Add(new MarcaAuto
                {
                    Id = 101, // Usar IDs diferentes para evitar conflictos
                    Nombre = "Toyota",
                    PaisOrigen = "Japón",
                    AnioFundacion = 1937,
                    Activo = true
                });
                context.MarcasAutos.Add(new MarcaAuto
                {
                    Id = 102,
                    Nombre = "Volkswagen",
                    PaisOrigen = "Alemania",
                    AnioFundacion = 1937,
                    Activo = true
                });
                context.MarcasAutos.Add(new MarcaAuto
                {
                    Id = 103,
                    Nombre = "Ford",
                    PaisOrigen = "Estados Unidos",
                    AnioFundacion = 1903,
                    Activo = true
                });
                await context.SaveChangesAsync();
            }

            // Creamos una nueva instancia del contexto para simular una conexión real
            using (var context = new ApplicationDbContext(dbContextOptions))
            {
                // Montamos toda la cadena de dependencias como en la aplicación real
                // Repositorio -> Servicio -> Controlador (siguiendo la arquitectura hexagonal)
                var repository = new MarcaAutoRepository(context);
                var service = new MarcaAutoService(repository);
                var controller = new MarcasAutosController(service);

                // Act - Llamamos al endpoint que queremos probar
                var result = await controller.GetAll();

                // Assert - Verificamos que la respuesta sea correcta
                var okResult = Assert.IsType<OkObjectResult>(result.Result); // Comprobamos que sea un 200 OK
                var marcas = Assert.IsAssignableFrom<IEnumerable<MarcaAuto>>(okResult.Value); // Verificamos que el contenido sea una lista de marcas
                
                // Verificamos que las marcas que esperamos estén en la respuesta
                // No comprobamos el número total porque podría haber más marcas en la BD
                var marcasList = marcas.ToList();
                Assert.Contains(marcasList, m => m.Nombre == "Toyota" && m.Id == 101); // Debe estar Toyota
                Assert.Contains(marcasList, m => m.Nombre == "Volkswagen" && m.Id == 102); // Debe estar VW
                Assert.Contains(marcasList, m => m.Nombre == "Ford" && m.Id == 103); // Debe estar Ford
            }
        }

        // Test que verifica que podemos obtener una marca específica por su ID
        // Comprueba que devuelva los datos correctos de Toyota cuando pedimos el ID 201
        [Fact]
        public async Task GetById_ReturnsCorrectMarcaAuto()
        {
            // Arrange - Configuramos una BD en memoria con nombre único
            var dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: $"CoderlandBD_{Guid.NewGuid()}")
                .Options;

            // Creamos y configuramos la BD en memoria con un solo registro
            using (var context = new ApplicationDbContext(dbContextOptions))
            {
                // Limpiamos la BD para asegurarnos de empezar desde cero
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
                
                // Agregamos solo la marca Toyota para este test
                context.MarcasAutos.Add(new MarcaAuto
                {
                    Id = 201, // Usar un ID diferente para evitar conflictos
                    Nombre = "Toyota",
                    PaisOrigen = "Japón",
                    AnioFundacion = 1937,
                    Activo = true
                });
                await context.SaveChangesAsync();
            }

            // Creamos una nueva instancia del contexto para simular una conexión real
            using (var context = new ApplicationDbContext(dbContextOptions))
            {
                // Montamos toda la cadena de dependencias
                var repository = new MarcaAutoRepository(context);
                var service = new MarcaAutoService(repository);
                var controller = new MarcasAutosController(service);

                // Act - Llamamos al endpoint con el ID específico
                var result = await controller.GetById(201); // Usar el ID 201 que se creó en la prueba

                // Assert - Verificamos todos los datos de la marca
                var okResult = Assert.IsType<OkObjectResult>(result.Result); // Debe ser 200 OK
                var marca = Assert.IsType<MarcaAuto>(okResult.Value); // El contenido debe ser un objeto MarcaAuto
                
                // Verificamos que todos los campos tengan los valores esperados
                Assert.Equal("Toyota", marca.Nombre); // Nombre correcto
                Assert.Equal("Japón", marca.PaisOrigen); // País correcto
                Assert.Equal(1937, marca.AnioFundacion); // Año correcto
                Assert.True(marca.Activo); // Debe estar activa
            }
        }

        // Test que verifica que obtenemos un 404 NotFound cuando buscamos un ID que no existe
        // Es importante validar también los casos negativos para asegurar un buen manejo de errores
        [Fact]
        public async Task GetById_ReturnsNotFound_WhenMarcaAutoDoesNotExist()
        {
            // Arrange - Configuramos una BD en memoria vacía
            var dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: $"CoderlandBD_{Guid.NewGuid()}")
                .Options;

            // Creamos una nueva instancia del contexto con una BD vacía
            using (var context = new ApplicationDbContext(dbContextOptions))
            {
                // Limpiamos la BD para asegurarnos de que esté vacía
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
                
                // Montamos la cadena de dependencias pero sin datos en la BD
                var repository = new MarcaAutoRepository(context);
                var service = new MarcaAutoService(repository);
                var controller = new MarcasAutosController(service);

                // Act - Intentamos obtener un ID que sabemos que no existe
                var result = await controller.GetById(999); // Usar un ID que no existe

                // Assert - Verificamos que devuelva un 404 NotFound
                // Esto es crucial para que el cliente sepa que el recurso no existe
                Assert.IsType<NotFoundResult>(result.Result);
            }
        }
    }
}
