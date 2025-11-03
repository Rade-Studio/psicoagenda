using Microsoft.AspNetCore.Mvc;
using PsicoAgenda.Application.Dtos.Pacientes;
using PsicoAgenda.Application.Interfaces;

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
    public IActionResult Get()
    {
        return Ok(new List<PacienteRespuesta>()
        {
            new(
                Guid.Parse("11111111-1111-1111-1111-111111111111"),
                "Ana",
                "Ramírez",
                "ana@mail.com",
                "3001111111",
                new DateTime(1995, 5, 10)
            ),
            new(
                Guid.Parse("22222222-2222-2222-2222-222222222222"),
                "Luis",
                "Torres",
                "luis@mail.com",
                "3002222222",
                new DateTime(1995, 5, 10)
            ),
        });
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
    
    
}