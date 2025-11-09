using PsicoAgenda.Application.Dtos.Pacientes;

namespace PsicoAgenda.Application.Interfaces;

public interface IPacienteService
{
    //Obtiene un paciente por su ID
    Task<PacienteRespuesta> ObtenerPaciente(Guid id, CancellationToken cancellationToken);
    //Obtiene todos los pacientes
    Task<IEnumerable<PacienteRespuesta>> ObtenerPacientes(CancellationToken cancellationToken);
    //Crea un nuevo paciente
    Task CrearPaciente(PacienteCreacion request);
    //Actualiza un paciente existente
    Task<PacienteRespuesta> ActualizarPaciente(Guid id, PacienteActualizacion request, CancellationToken cancellationToken);
    //Elimina
    Task EliminarPaciente(Guid id, CancellationToken cancellationToken);
}