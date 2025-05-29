using Coderland.API.Application.Interfaces;
using Coderland.API.Application.Services;
using Coderland.API.Domain.Interfaces;
using Coderland.API.Infrastructure.Persistence;
using Coderland.API.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System;

var builder = WebApplication.CreateBuilder(args);

// Registro de servicios principales
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Configuración de Swagger para documentación de la API
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() 
    { 
        Title = "Coderland API", 
        Version = "v1", 
        Description = "API para gestión de marcas de automóviles con arquitectura hexagonal",
        Contact = new() 
        { 
            Name = "Coderland", 
            Email = "info@coderland.com", 
            Url = new Uri("https://coderland.com") 
        },
        License = new() 
        { 
            Name = "MIT License", 
            Url = new Uri("https://opensource.org/licenses/MIT") 
        }
    });
    
    // Incluimos los comentarios XML para mejorar la documentación
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
    c.EnableAnnotations();
    c.OrderActionsBy(apiDesc => apiDesc.RelativePath);
});

// Configuración de la conexión a PostgreSQL
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Registro de repositorios y servicios siguiendo el patrón de inyección de dependencias
builder.Services.AddScoped<IMarcaAutoRepository, MarcaAutoRepository>();
builder.Services.AddScoped<IMarcaAutoService, MarcaAutoService>();

var app = builder.Build();

// Inicialización automática de la base de datos
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<ApplicationDbContext>();
        context.Database.EnsureCreated();
        
        // Aplicamos migraciones pendientes si existen
        if (context.Database.GetPendingMigrations().Any())
        {
            context.Database.Migrate();
        }
        
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogInformation("Base de datos inicializada correctamente");
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Error al inicializar la base de datos");
    }
}

// Configuración del pipeline HTTP
app.UseSwagger();
app.UseSwaggerUI(c => 
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Coderland API v1");
    c.RoutePrefix = string.Empty; // Swagger UI como página principal
    
    // Mejoras en la interfaz de usuario de Swagger
    c.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.List);
    c.DefaultModelsExpandDepth(0);
    c.DisplayRequestDuration();
    c.EnableDeepLinking();
    c.EnableFilter();
    c.DocumentTitle = "Coderland API - Documentación";
    
    // Configurar para que sea accesible desde localhost
    c.ConfigObject.AdditionalItems.Add("host", "localhost:8080");
});

// Configuración específica para entorno de desarrollo
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

// Configuración del middleware
// Deshabilitamos la redirección HTTPS para entornos Docker
// app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();
app.MapControllers();

// Iniciamos la aplicación
app.Run();
