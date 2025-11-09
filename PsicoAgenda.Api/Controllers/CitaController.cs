using Microsoft.AspNetCore.Mvc;
using PsicoAgenda.Application.Interfaces;

namespace PsicoAgenda.Api.Controllers;
[ApiController]
[Route("api/[controller]")]

public class CitaController : ControllerBase
{
    private readonly ICitaService _citaService;
    public CitaController(ICitaService citaService)
    {
        _citaService = citaService;
    }
    // Obtener todas las citas
    [HttpGet]
    public async Task<ActionResult> Get(CancellationToken cancellationToken)
    {
        var citas = await _citaService.ObtenerCitas(cancellationToken);
        return Ok(citas);
    }
    // Obtener una cita por ID
    [HttpGet("{id}")]
    public async Task<ActionResult> Get(Guid id, CancellationToken cancellationToken)
    {
        var cita = await _citaService.ObtenerCita(id, cancellationToken);
        return Ok(cita);
    }
    // Crear una nueva cita
    [HttpPost]
    public async Task<ActionResult> Post([FromBody] PsicoAgenda.Application.Dtos.Citas.CitaCreacion request)
    {
        await _citaService.CrearCita(request);
        return Ok("Cita creada con exito");
    }
    // Actualizar una cita existente
    [HttpPut("{id}")]
    public async Task<ActionResult> Put(Guid id, [FromBody] PsicoAgenda.Application.Dtos.Citas.CitaActualizacion request, CancellationToken cancellationToken)
    {
        var cita = await _citaService.ActualizarCita(id, request, cancellationToken);
        return Ok(cita);
    }
    // Eliminar una cita
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        await _citaService.EliminarCita(id, cancellationToken);
        return Ok("Cita eliminada correctamente");
    }
}

