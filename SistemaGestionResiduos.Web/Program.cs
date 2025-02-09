using Microsoft.EntityFrameworkCore;
using SistemaGestionResiduos.Application.Services;
using SistemaGestionResiduos.Domain.Entities;
using SistemaGestionResiduos.Infrastructure.Data;
using SistemaGestionResiduos.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Configure SQLite with detailed error logging
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
Console.WriteLine($"Connection string: {connectionString}");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlite(connectionString);
    options.EnableDetailedErrors();
    options.EnableSensitiveDataLogging();
});

// Register repositories and services
builder.Services.AddScoped(typeof(GenericRepository<>));
builder.Services.AddScoped<GenericRepository<Ruta>>(); // Registro del repositorio de rutas
builder.Services.AddScoped<RutaService>();
builder.Services.AddScoped<ContenedorService>();
builder.Services.AddScoped<RecoleccionService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Create database and apply migrations
try
{
    using (var scope = app.Services.CreateScope())
    {
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        Console.WriteLine("Intentando crear la base de datos...");
        var created = db.Database.EnsureCreated();
        Console.WriteLine($"Base de datos {(created ? "creada" : "ya existía")}");
        
        // Verificar que podemos conectarnos
        var canConnect = db.Database.CanConnect();
        Console.WriteLine($"¿Puede conectarse a la base de datos? {canConnect}");
        
        if (!canConnect)
        {
            throw new Exception("No se puede conectar a la base de datos después de crearla");
        }

        // Verificar la estructura de la base de datos
        Console.WriteLine("\nVerificando estructura de la base de datos:");
        var command = db.Database.GetDbConnection().CreateCommand();
        command.CommandText = "SELECT name FROM sqlite_master WHERE type='table';";
        await db.Database.OpenConnectionAsync();
        
        try
        {
            using (var result = await command.ExecuteReaderAsync())
            {
                Console.WriteLine("Tablas encontradas:");
                while (await result.ReadAsync())
                {
                    var tableName = result.GetString(0);
                    Console.WriteLine($"- {tableName}");
                    
                    // Obtener estructura de cada tabla
                    var tableCommand = db.Database.GetDbConnection().CreateCommand();
                    tableCommand.CommandText = $"PRAGMA table_info('{tableName}');";
                    using (var tableResult = await tableCommand.ExecuteReaderAsync())
                    {
                        while (await tableResult.ReadAsync())
                        {
                            var columnName = tableResult.GetString(1);
                            var columnType = tableResult.GetString(2);
                            var notNull = tableResult.GetBoolean(3);
                            Console.WriteLine($"  * {columnName} ({columnType}){(notNull ? " NOT NULL" : "")}");
                        }
                    }
                }
            }
        }
        finally
        {
            await db.Database.CloseConnectionAsync();
        }
    }
}
catch (Exception ex)
{
    Console.WriteLine($"Error al crear/conectar a la base de datos: {ex.Message}");
    Console.WriteLine($"StackTrace: {ex.StackTrace}");
    throw;
}

app.Run();
