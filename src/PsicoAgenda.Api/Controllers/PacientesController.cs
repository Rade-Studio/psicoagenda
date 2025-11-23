using Microsoft.AspNetCore.Mvc;
using PsicoAgenda.Application.Dtos.Pacientes;
using PsicoAgenda.Application.Interfaces;
using PsicoAgenda.Domain.Models;

namespace PsicoAgenda.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PacientesController : ControllerBase
{
    private readonly IPacienteService _pacienteService;
    
    public PacientesController(IPacienteService pacienteService)
    {
        _pacienteService = pacienteService;
    }
    
    [HttpGet]
    public async Task<ActionResult<PacienteRespuesta>> Get(CancellationToken cancellationToken)
    {
        var pacientes = await _pacienteService.ObtenerPacientes(cancellationToken);
        return Ok(pacientes);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<PacienteRespuesta>> Get(Guid id, CancellationToken cancellationToken)
    {
        var paciente = await _pacienteService.ObtenerPaciente(id, cancellationToken);
        
        return Ok(paciente);
    }
    
    [HttpPost]
    public async Task<ActionResult<PacienteRespuesta>> Post([FromBody] PacienteCreacion request)
    {
        await _pacienteService.CrearPaciente(request);
        
        return Ok();
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Put(Guid id, [FromBody] PacienteActualizacion request, CancellationToken cancellationToken)
    {
        var pacientes = await _pacienteService.ActualizarPaciente(id, request, cancellationToken);
        
        return Ok(pacientes);
    }
    
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        await _pacienteService.EliminarPaciente(id, cancellationToken);
        
        return Ok("Eliminado Correctamente");
    }
}