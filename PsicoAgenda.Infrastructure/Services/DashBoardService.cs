using AutoMapper;
using PsicoAgenda.Application.Dtos.Citas;
using PsicoAgenda.Application.Dtos.Dashboard;
using PsicoAgenda.Application.Interfaces;
using PsicoAgenda.Domain.Interfaces;

namespace PsicoAgenda.Infrastructure.Services
{
    public class DashBoardService(IUnitOfWork unitOfWork, IMapper mapper) : IDashBoardService
    {
        public async Task<DashboardRespuesta> ObtenerDatosDashboardAsync(CancellationToken cancellationToken)
        {
            var Pacientes = await unitOfWork.Pacientes.SeleccionarTodos(cancellationToken);
            var Sesiones = await unitOfWork.Sesiones.SeleccionarTodos(cancellationToken);
            var CitasHoy = await unitOfWork.Citas.SeleccionarTodosPorEIncluir(x => x.FechaInicio == DateTime.UtcNow);
            var ProximasCitas = await unitOfWork.Citas.SeleccionarTodosPorEIncluir(x => x.FechaInicio > DateTime.UtcNow);
            var citasproximas = ProximasCitas
                                .OrderBy(x => x.FechaInicio)
                                .Take(5)
                                .Select(cita => mapper.Map<CitaRespuesta>(cita))
                                .ToList();

            return new DashboardRespuesta
            {
                TotalPacientes = Pacientes.Count(),
                TotalSesiones = Sesiones.Count(),
                TotalCitasHoy = CitasHoy.Count(),
                CitasProximas = citasproximas
            };
        }
    }
}
