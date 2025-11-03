using Microsoft.EntityFrameworkCore;
using PsicoAgenda.Application;
using PsicoAgenda.Application.Mappers;
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
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddApplication();
builder.Services.AddPersistence();
builder.Services.AddInfrastructure();

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
