using PsicoAgenda.Application.Dtos.Pacientes;

namespace PsicoAgenda.Application.Interfaces;

public interface IPacienteService
{
    Task<PacienteRespuesta> ObtenerPaciente(Guid id, CancellationToken cancellationToken);
    Task CrearPaciente(PacienteCreacion request);
}