using Microsoft.EntityFrameworkCore;
using MediatR;
using Practice_Figures.Application.Common.Behaviors;
using Practice_Figures.Application.Common.Interfaces;
using Practice_Figures.Application.Figures.Queries;
using Practice_Figures.Application.Figures.Validators;
using Practice_Figures.Infrastructure.Data;
using Practice_Figures.Infrastructure.Repositories;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' was not found.");

builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddScoped<IUnitOfWork>(provider => provider.GetRequiredService<AppDbContext>());
builder.Services.AddScoped<IFigureRepository, FigureRepository>();
builder.Services.AddScoped<IFigureReferenceRepository, FigureReferenceRepository>();
builder.Services.AddScoped<FigureValidator>();
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(FigureValidationBehavior<,>));
builder.Services.AddSwaggerGen();
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower;
    });
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetFiguresQuery).Assembly));

var app = builder.Build();

app.MapControllers();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.Run();
