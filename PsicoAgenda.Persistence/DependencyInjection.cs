using Microsoft.Extensions.DependencyInjection;
using PsicoAgenda.Domain.Interfaces;
using PsicoAgenda.Persistence.Repositories;
using PsicoAgenda.Persistence.UnitOfWork;

namespace PsicoAgenda.Persistence;

public static class DependencyInjection
{
    public static void AddPersistence(this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWorkManager>();
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
    }
    
}