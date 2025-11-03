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
    }
}