using Microsoft.EntityFrameworkCore.Storage;
using PsicoAgenda.Domain.Interfaces;
using PsicoAgenda.Domain.Models;
using PsicoAgenda.Persistence.Context;

namespace PsicoAgenda.Persistence.UnitOfWork;

public class UnitOfWorkManager(
    AppDbContext context,
    IRepository<Cita> citas,
    IRepository<Cuestionario> cuestionarios,
    IRepository<Paciente> pacientes,
    IRepository<RespuestaCuestionario> respuestasCuestionarios,
    IRepository<Sesion> sesiones,
    IRepository<SesionNota> sesionNotas
) : IUnitOfWork
{
    
    private IDbContextTransaction? _transaction;
    public IRepository<Cita> Citas { get; } = citas;
    public IRepository<Cuestionario> Cuestionarios { get; } = cuestionarios;
    public IRepository<Paciente> Pacientes { get; } = pacientes;
    public IRepository<RespuestaCuestionario> RespuestasCuestionarios { get; } = respuestasCuestionarios;
    public IRepository<Sesion> Sesiones { get; } = sesiones;
    public IRepository<SesionNota> SesionNotas { get; } = sesionNotas;
    
    public void Dispose()
    {
        context?.Dispose();
        _transaction?.Dispose();
        _transaction = null;
    }

    public async Task<int> GuardarCambios(CancellationToken cancellationToken = default)
    {
        return await context.SaveChangesAsync(cancellationToken);
    }

    public async Task IniciarTransaccion()
    {
        _transaction ??= await context.Database.BeginTransactionAsync();
    }

    public async Task ConfirmarTransaccion()
    {
        try
        {
            await _transaction!.CommitAsync();
        }
        finally
        {
            _transaction?.Dispose();
            _transaction = null;
        }
        
    }

    public async Task CancelarTransaccion()
    {
        try
        {
            // Hacemos rollback solo si existe una transacción
            await _transaction!.RollbackAsync();
        }
        finally
        {
            // Liberamos los recursos de la transacción
            _transaction?.Dispose();
            _transaction = null;
        }
    }
}