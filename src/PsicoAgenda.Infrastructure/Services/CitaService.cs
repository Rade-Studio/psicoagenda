using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PsicoAgenda.Application.Dtos.Citas;
using PsicoAgenda.Application.Interfaces;
using PsicoAgenda.Domain.Interfaces;
using PsicoAgenda.Domain.Models;

namespace PsicoAgenda.Infrastructure.Services
{
    public class CitaService(IUnitOfWork unitOfWork, IMapper mapper): ICitaService
    {
        public async Task<CitaRespuesta> ObtenerCita(Guid id, CancellationToken cancellationToken)
        {
            var cita = await unitOfWork.Citas.SeleccionarPorEIncluir(
                c => c.Id == id,
                cancellationToken,
                q => q.Include(c => c.Paciente)
            );
            return mapper.Map<CitaRespuesta>(cita);
        }
        public async Task<IEnumerable<CitaRespuesta>> ObtenerCitas(CancellationToken cancellationToken)
        {
            var citas = await unitOfWork.Citas.SeleccionarTodosPorEIncluir(c => true, includes: q => q.Include(c => c.Paciente));
            return mapper.Map<IEnumerable<CitaRespuesta>>(citas);
        }
        public async Task CrearCita(CitaCreacion request)
        {
            var cita = mapper.Map<Cita>(request);
            if (cita == null) {
                throw new Exception("Error al mapear la cita");
            }
            var paciente = await unitOfWork.Pacientes.SeleccionarPorId(cita.PacienteId, CancellationToken.None);
            if (paciente == null)
                throw new KeyNotFoundException($"No se encontró el paciente con id {cita.PacienteId}");

            await unitOfWork.Citas.Crear(cita);
            await unitOfWork.GuardarCambios();
        }
        public async Task<CitaRespuesta> ActualizarCita(Guid id, CitaActualizacion request, CancellationToken cancellationToken)
        {
            var cita = await unitOfWork.Citas.SeleccionarPorId(id, cancellationToken);
            if (cita == null)
                throw new Exception("Cita no encontrada");
            mapper.Map(request, cita);
            unitOfWork.Citas.Actualizar(cita);
            await unitOfWork.GuardarCambios();
            return mapper.Map<CitaRespuesta>(cita);
        }
        public async Task EliminarCita(Guid id, CancellationToken cancellationToken)
        {
            var cita = await unitOfWork.Citas.SeleccionarPorId(id, cancellationToken);
            if (cita == null)
                throw new Exception("Cita no encontrada");
            unitOfWork.Citas.Eliminar(cita);
            await unitOfWork.GuardarCambios();
        }
    }
}
