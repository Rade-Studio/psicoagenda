using Microsoft.EntityFrameworkCore.Storage;
using PsicoAgenda.Domain.Models;

namespace PsicoAgenda.Domain.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IRepository<Cita> Citas { get; }
    IRepository<Cuestionario> Cuestionarios { get; }
    IRepository<Paciente> Pacientes { get; }
    IRepository<RespuestaCuestionario> RespuestasCuestionarios { get; }
    IRepository<Sesion> Sesiones { get; }
    IRepository<SesionNota> SesionNotas { get; }
    
    Task<int> GuardarCambios(CancellationToken cancellationToken = default);
    Task IniciarTransaccion();
    Task ConfirmarTransaccion();
    Task CancelarTransaccion();


}