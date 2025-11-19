using PsicoAgenda.Application.Dtos.Sesiones;

namespace PsicoAgenda.Application.Interfaces;

public interface ISesionService
{
    Task<SesionRespuesta?> ObtenerSesion(Guid id, CancellationToken cancellationToken);
    Task<IEnumerable<SesionRespuesta>> ObtenerSesiones(CancellationToken cancellationToken);

    Task CrearSesion(SesionCreacion request);
    Task<SesionRespuesta> ActualizarSesion(Guid id, SesionActualizacion request, CancellationToken cancellationToken);
    Task EliminarSesion(Guid id, CancellationToken cancellationToken);
}

