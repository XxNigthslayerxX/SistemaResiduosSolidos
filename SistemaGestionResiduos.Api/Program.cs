using Microsoft.EntityFrameworkCore;
using SistemaGestionResiduos.Infrastructure.Data;
using SistemaGestionResiduos.Infrastructure.Repositories;
using SistemaGestionResiduos.Application.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Configure SQLite
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register repositories and services
builder.Services.AddScoped(typeof(GenericRepository<>));
builder.Services.AddScoped<RutaService>();
builder.Services.AddScoped<ContenedorService>();
builder.Services.AddScoped<RecoleccionService>();

// Add Swagger/OpenAPI services.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add CORS policy to allow requests from http://localhost:5095.
builder.Services.AddCors(options =>
{
    options.AddPolicy("MyCorsPolicy", policyBuilder =>
    {
        policyBuilder.WithOrigins("http://localhost:5095")
                     .AllowAnyHeader()
                     .AllowAnyMethod();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Use the CORS policy
app.UseCors("MyCorsPolicy");

app.UseAuthorization();

app.MapControllers();

// Create database and apply migrations
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    db.Database.EnsureCreated();
}

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
