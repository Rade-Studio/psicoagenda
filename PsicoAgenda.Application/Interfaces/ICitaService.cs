using PsicoAgenda.Application.Dtos.Citas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PsicoAgenda.Application.Interfaces
{
    public interface ICitaService
    {
        Task<CitaRespuesta> ObtenerCita(Guid id, CancellationToken cancellationToken);
        Task<IEnumerable<CitaRespuesta>> ObtenerCitas(CancellationToken cancellationToken);
        Task CrearCita(CitaCreacion request);
        Task<CitaRespuesta> ActualizarCita(Guid id, CitaActualizacion request, CancellationToken cancellationToken);
        Task EliminarCita(Guid id, CancellationToken cancellationToken);
    }
}
