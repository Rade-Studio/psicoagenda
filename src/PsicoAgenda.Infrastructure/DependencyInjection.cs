using Microsoft.Extensions.DependencyInjection;
using PsicoAgenda.Application.Interfaces;
using PsicoAgenda.Infrastructure.Services;
using FluentValidation;
using FluentValidation.AspNetCore;
using PsicoAgenda.Infrastructure.Validaciones;

namespace PsicoAgenda.Infrastructure;

public static class DependencyInjection
{
    public static void AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<IPacienteService, PacienteService>();
        services.AddScoped<ICitaService, CitaService>();
        services.AddScoped<ISesionService, SesionService>();
        services.AddScoped<ISesionNotaService, SesionNotaService>();
        services.AddScoped<IDashBoardService, DashBoardService>();
        // Validators registered as FluentValidation IValidator<T>
        services.AddScoped<IValidator<PsicoAgenda.Application.Dtos.Citas.CitaCreacion>, CitaValidator>();
        services.AddScoped<IValidator<PsicoAgenda.Application.Dtos.Sesiones.SesionCreacion>, SesionValidator>();
        services.AddScoped<IValidator<PsicoAgenda.Application.Dtos.Pacientes.PacienteCreacion>, PacienteValidator>();
        services.AddScoped<IValidator<PsicoAgenda.Application.Dtos.Pacientes.PacienteActualizacion>, PacienteUpdateValidator>();
        services.AddScoped<IValidator<PsicoAgenda.Application.Dtos.SesionNota.SesionNotaCreacion>, SesionNotaValidator>();
    }
    
}