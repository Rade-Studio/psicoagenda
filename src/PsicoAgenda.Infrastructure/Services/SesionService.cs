using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PsicoAgenda.Application.Dtos.Sesiones;
using PsicoAgenda.Application.Interfaces;
using PsicoAgenda.Domain.Interfaces;
using PsicoAgenda.Domain.Models;

namespace PsicoAgenda.Infrastructure.Services
{
    public class SesionService(IUnitOfWork unitOfWork, IMapper mapper) : ISesionService
    {
        public async Task<SesionRespuesta?> ObtenerSesion(Guid id, CancellationToken cancellationToken)
        {
            var sesion = await unitOfWork.Sesiones.SeleccionarPorEIncluir(
                s => s.Id == id,
                cancellationToken,
                s => s.Include(x => x.Notas),
                s => s.Include(x => x.Paciente),
                s => s.Include(x => x.Cita)
            );

            if (sesion is null)
                throw new KeyNotFoundException($"No se encontró la sesión con id {id}");

            return mapper.Map<SesionRespuesta>(sesion);
        }
        public async Task<SesionRespuesta> ActualizarSesion(Guid id, SesionActualizacion request, CancellationToken cancellationToken)
        {
            var sesion = await unitOfWork.Sesiones.SeleccionarPorId(id, cancellationToken);
            if (sesion is null)
                throw new KeyNotFoundException($"No se encontró la sesión con id {id}");
            mapper.Map(request, sesion);
            unitOfWork.Sesiones.Actualizar(sesion);
            await unitOfWork.GuardarCambios(cancellationToken);
            return mapper.Map<SesionRespuesta>(sesion);
        }

        public async Task CrearSesion(SesionCreacion request)
        {
            var sesion = mapper.Map<Sesion>(request);
            unitOfWork.Sesiones.Crear(sesion);
            await unitOfWork.GuardarCambios();
        }

        public async Task EliminarSesion(Guid id, CancellationToken cancellationToken)
        {
            var sesion = await unitOfWork.Sesiones.SeleccionarPorId(id, cancellationToken);
            if (sesion is null)
                throw new KeyNotFoundException($"No se encontró la sesión con id {id}");
            unitOfWork.Sesiones.Eliminar(sesion);
            await unitOfWork.GuardarCambios(cancellationToken);
        }

        public async Task<IEnumerable<SesionRespuesta>> ObtenerSesiones(CancellationToken cancellationToken)
        {
            var sesiones = await unitOfWork.Sesiones.SeleccionarTodosPorEIncluir(
                _ => true,
                s => s.Include(x => x.Notas),
                s => s.Include(x => x.Paciente),
                s => s.Include(x => x.Cita)
            );
            return sesiones.Select(s => mapper.Map<SesionRespuesta>(s));
        }
    }
}
