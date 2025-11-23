using Microsoft.AspNetCore.Mvc;
using PsicoAgenda.Application.Dtos.SesionNota;
using PsicoAgenda.Application.Interfaces;

namespace PsicoAgenda.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SesionNotaController : ControllerBase
{
    private readonly ISesionNotaService _sesionNotaService;
    public SesionNotaController(ISesionNotaService sesionNotaService)
    {
        _sesionNotaService = sesionNotaService;
    }
    // Obtener todas las notas de SesionNota
    [HttpGet]
    public async Task<ActionResult> Get(Guid sesionId, CancellationToken cancellationToken)
    {
        var notas = await _sesionNotaService.ObtenerSesionNotasPorSesionAsync(sesionId, cancellationToken);
        return Ok(notas);
    }
    // Crear una nueva nota de SesionNota
    [HttpPost("{id}")]
    public async Task<ActionResult> Post(Guid id, [FromBody] SesionNotaCreacion sesionNotaCreacion, CancellationToken cancellationToken)
    {
        var nota = await _sesionNotaService.CrearSesionNotaAsync(id, sesionNotaCreacion, cancellationToken);
        return Ok(nota);
    }
    // Actualizar una nota de SesionNota
    [HttpPut("{id}")]
    public async Task<ActionResult> Put(Guid id, [FromBody] SesionNotaActualizacion sesionNotaActualizacion, CancellationToken cancellationToken)
    {
        var nota = await _sesionNotaService.ActualizarSesionNotaAsync(id, sesionNotaActualizacion, cancellationToken);
        return Ok(nota);
    }
    // Eliminar una nota de SesionNota
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        await _sesionNotaService.EliminarSesionNotaAsync(id, cancellationToken);
        return Ok("Nota de sesion eliminada correctamente");
    }
}

