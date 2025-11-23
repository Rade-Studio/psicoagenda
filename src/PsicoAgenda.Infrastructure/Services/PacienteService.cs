using AutoMapper;
using PsicoAgenda.Application.Dtos.Pacientes;
using PsicoAgenda.Application.Interfaces;
using PsicoAgenda.Domain.Interfaces;
using PsicoAgenda.Domain.Models;

namespace PsicoAgenda.Infrastructure.Services;

public class PacienteService(IUnitOfWork unitOfWork, IMapper mapper) : IPacienteService
{
    public async Task<PacienteRespuesta> ObtenerPaciente(Guid id, CancellationToken cancellationToken)
    {
        var paciente = await unitOfWork.Pacientes.SeleccionarPorId(id, cancellationToken);
        
        return mapper.Map<PacienteRespuesta>(paciente);
        
    }

    public async Task CrearPaciente(PacienteCreacion request)
    {
        var paciente = mapper.Map<Paciente>(request);
        
        await unitOfWork.Pacientes.Crear(paciente);

        await unitOfWork.GuardarCambios();
    }

    public async Task<IEnumerable<PacienteRespuesta>> ObtenerPacientes(CancellationToken cancellationToken)
    {
        var pacientes = await unitOfWork.Pacientes.SeleccionarTodos(cancellationToken);

        return mapper.Map<IEnumerable<PacienteRespuesta>>(pacientes);
    }

    public async Task<PacienteRespuesta> ActualizarPaciente(Guid id, PacienteActualizacion request, CancellationToken cancellationToken)
    {
        var paciente = await unitOfWork.Pacientes.SeleccionarPorId(id, cancellationToken);
        
        if (paciente == null)
            throw new Exception("Paciente no encontrado");
        
        mapper.Map(request, paciente);
        
        unitOfWork.Pacientes.Actualizar(paciente);
        await unitOfWork.GuardarCambios();

        return mapper.Map<PacienteRespuesta>(paciente);

    }

    public async Task EliminarPaciente(Guid id, CancellationToken cancellationToken)
    {
        var paciente = await unitOfWork.Pacientes.SeleccionarPorId(id, cancellationToken);
        
        if (paciente == null)
            throw new Exception("Paciente no encontrado");
        
        unitOfWork.Pacientes.Eliminar(paciente);
        await unitOfWork.GuardarCambios();
    }
}