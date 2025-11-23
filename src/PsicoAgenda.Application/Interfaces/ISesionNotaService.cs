using PsicoAgenda.Application.Dtos.SesionNota;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PsicoAgenda.Application.Interfaces
{
    public interface ISesionNotaService
    {
        Task<SesionNotaRespuesta> CrearSesionNotaAsync(Guid sesionId,SesionNotaCreacion request, CancellationToken cancellationToken);
        Task<IEnumerable<SesionNotaRespuesta>> ObtenerSesionNotasPorSesionAsync(Guid sesionId, CancellationToken cancellationToken);
        Task<SesionNotaRespuesta> ActualizarSesionNotaAsync(Guid notaId, SesionNotaActualizacion request, CancellationToken cancellationToken);
        Task EliminarSesionNotaAsync(Guid notaId, CancellationToken cancellationToken);
    }
}
