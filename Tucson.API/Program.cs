using Tucson.Application.Service;
using Tucson.Domain.Interfaces;
using Tucson.Infrastructure.Repositories;
using Microsoft.OpenApi.Models;
using Tucson.Application.Service.Interface;
using Tucson.Application.Strategies;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin", builder =>
    {
        builder.WithOrigins("http://localhost:5140") 
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

// Agregar servicios al contenedor
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Tucson API", Version = "v1" });

    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);

    c.EnableAnnotations();
});

builder.Services.AddSingleton<IReservationRepository, ReservationRepository>();
builder.Services.AddScoped<ITableAssignmentStrategy, DiamondTableAssignmentStrategy>();
builder.Services.AddScoped<ITableAssignmentStrategy, NonDiamondTableAssignmentStrategy>();
builder.Services.AddScoped<IAdvanceDaysValidationStrategy, ClassicAdvanceDaysValidationStrategy>();
builder.Services.AddScoped<IAdvanceDaysValidationStrategy, GoldAdvanceDaysValidationStrategy>();
builder.Services.AddScoped<IAdvanceDaysValidationStrategy, PlatinumAdvanceDaysValidationStrategy>();
builder.Services.AddScoped<IAdvanceDaysValidationStrategy, DiamondAdvanceDaysValidationStrategy>();
builder.Services.AddScoped<IReservationService, ReservationService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger(); 
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Tucson API v1")); // Configure Swagger UI
}

app.UseHttpsRedirection();

app.UseCors("AllowSpecificOrigin");

app.UseAuthorization();

app.MapControllers();

app.Run();
