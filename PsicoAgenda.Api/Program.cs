using Microsoft.EntityFrameworkCore;
using PsicoAgenda.Application;
using PsicoAgenda.Application.Mappers;
using FluentValidation;
using FluentValidation.AspNetCore;
using PsicoAgenda.Infrastructure.Validaciones;
using PsicoAgenda.Domain.Interfaces;
using PsicoAgenda.Infrastructure;
using PsicoAgenda.Persistence;
using PsicoAgenda.Persistence.Context;
using PsicoAgenda.Persistence.Repositories;
using PsicoAgenda.Persistence.UnitOfWork;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(db =>
{
    db.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddControllers(options =>
{
    // We'll use an async action filter to run FluentValidation async validators
    options.Filters.Add<PsicoAgenda.Api.Filters.FluentValidationAsyncActionFilter>();
});
// Do not use AddFluentValidationAutoValidation because ASP.NET automatic validation is synchronous
// Register persistence/infrastructure so validators that depend on services can be resolved
builder.Services.AddPersistence();
builder.Services.AddInfrastructure();

// Register all validators from the Infrastructure assembly (contains our validators)
// Validators are registered in Infrastructure DI (so they can receive IUnitOfWork).
// Make the async action filter available for DI
builder.Services.AddScoped<PsicoAgenda.Api.Filters.FluentValidationAsyncActionFilter>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddApplication();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
