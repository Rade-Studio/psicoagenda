using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;
using PsicoAgenda.Domain.Models;

namespace PsicoAgenda.Domain.Interfaces;

public interface IRepository<TEntity> where TEntity : EntidadBase
{
    Task Crear(TEntity entity);
    void Actualizar(TEntity entity);
    void Eliminar(TEntity entity);
    Task<TEntity?> SeleccionarPorId(Guid id, CancellationToken cancellationToken);
    Task<IEnumerable<TEntity>> SeleccionarTodos(CancellationToken cancellationToken);
    Task<TEntity?> SeleccionarPor(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken);

    // Seleccionar una entidad con una condicion e incluir relaciones
    Task<TEntity?> SeleccionarPorEIncluir(Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken,
        params Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>[]? includes);
    
    // Seleccionar todos con una condicion e incluir relaciones
    Task<IEnumerable<TEntity>> SeleccionarTodosPorEIncluir(Expression<Func<TEntity, bool>> predicate,
        params Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>[]? includes);
}