using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using PsicoAgenda.Domain.Interfaces;
using PsicoAgenda.Domain.Models;
using PsicoAgenda.Persistence.Context;

namespace PsicoAgenda.Persistence.Repositories;

public class Repository<TEntity>(AppDbContext dbContext) : IRepository<TEntity> where TEntity : EntidadBase
{
    public async Task Crear(TEntity entity)
    {
        await dbContext.Set<TEntity>().AddAsync(entity);
    }

    public void Actualizar(TEntity entity)
    { 
        dbContext.Set<TEntity>().Update(entity);
    }

    public void Eliminar(TEntity entity)
    {
        dbContext.Set<TEntity>().Remove(entity);
    }

    public async Task<TEntity?> SeleccionarPorId(Guid id, CancellationToken cancellationToken)
    {
        return await dbContext.Set<TEntity>().FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<TEntity>> SeleccionarTodos(CancellationToken cancellationToken)
    {
        return await dbContext.Set<TEntity>().ToListAsync(cancellationToken);
    }

    public async Task<TEntity?> SeleccionarPor(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken)
    {
        return await dbContext.Set<TEntity>().FirstOrDefaultAsync(predicate, cancellationToken);
    }

    public async Task<TEntity?> SeleccionarPorEIncluir(Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken,
        params Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>[]? includes)
    {
        var query = dbContext.Set<TEntity>().AsQueryable();
        
        if (includes != null && includes.Any())
            query = includes.Aggregate(query, (current, include) => include(current));
        
        return await query.FirstOrDefaultAsync(predicate, cancellationToken);
    }

    public async Task<IEnumerable<TEntity>> SeleccionarTodosPorEIncluir(Expression<Func<TEntity, bool>> predicate,
        params Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>[]? includes)
    {
        
        var query = dbContext.Set<TEntity>().AsQueryable();
        
        if (includes != null && includes.Any())
            query = includes.Aggregate(query, (current, include) => include(current));
        
        return await query.Where(predicate).ToListAsync();
    }
}
