using Microsoft.Extensions.DependencyInjection;
using PsicoAgenda.Application.Interfaces;
using PsicoAgenda.Infrastructure.Services;

namespace PsicoAgenda.Infrastructure;

public static class DependencyInjection
{
    public static void AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<IPacienteService, PacienteService>();
        services.AddScoped<ICitaService, CitaService>();
    }
    
}