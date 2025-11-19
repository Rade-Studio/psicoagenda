using AutoMapper;
using PsicoAgenda.Application.Dtos.SesionNota;
using PsicoAgenda.Application.Interfaces;
using PsicoAgenda.Domain.Interfaces;
using PsicoAgenda.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PsicoAgenda.Infrastructure.Services
{
    public class SesionNotaService(IUnitOfWork unitOfWork, IMapper mapper) : ISesionNotaService
    {
        public async Task<SesionNotaRespuesta> ActualizarSesionNotaAsync(Guid notaId, SesionNotaActualizacion request, CancellationToken cancellationToken)
        {
            var nota = await unitOfWork.SesionNotas.SeleccionarPorId(notaId, cancellationToken);
            if (nota is null)
                throw new KeyNotFoundException($"No se encontró la nota de sesión con id {notaId}");

            mapper.Map(request, nota);
            unitOfWork.SesionNotas.Actualizar(nota);
            await unitOfWork.GuardarCambios(cancellationToken);
            return mapper.Map<SesionNotaRespuesta>(nota);
        }

        public async Task<SesionNotaRespuesta> CrearSesionNotaAsync(SesionNotaCreacion request, CancellationToken cancellationToken)
        {
            // Verificar que la sesión exista
            var sesion = await unitOfWork.Sesiones.SeleccionarPorId(request.SesionId, cancellationToken);
            if (sesion is null)
                throw new KeyNotFoundException($"No se encontró la sesión con id {request.SesionId}");

            var nota = mapper.Map<SesionNota>(request);
            await unitOfWork.SesionNotas.Crear(nota);
            await unitOfWork.GuardarCambios(cancellationToken);
            return mapper.Map<SesionNotaRespuesta>(nota);
        }

        public async Task EliminarSesionNotaAsync(Guid notaId, CancellationToken cancellationToken)
        {
            var nota = await unitOfWork.SesionNotas.SeleccionarPorId(notaId, cancellationToken);
            if (nota is null)
                throw new KeyNotFoundException($"No se encontró la nota de sesión con id {notaId}");

            unitOfWork.SesionNotas.Eliminar(nota);
            await unitOfWork.GuardarCambios(cancellationToken);
        }

        public async Task<IEnumerable<SesionNotaRespuesta>> ObtenerSesionNotasPorSesionAsync(Guid sesionId, CancellationToken cancellationToken)
        {
            var notas = await unitOfWork.SesionNotas.SeleccionarTodosPorEIncluir(n => n.SesionId == sesionId);
            return notas.Select(n => mapper.Map<SesionNotaRespuesta>(n));
        }
    }
}
