using Microsoft.AspNetCore.Mvc;
using PsicoAgenda.Application.Dtos.Sesiones;
using PsicoAgenda.Application.Interfaces;

namespace PsicoAgenda.Api.Controllers;
[ApiController]
[Route("api/[controller]")]
public class SesionController: ControllerBase
{
    private readonly ISesionService _sesionService;
    public SesionController(ISesionService sesionService)
    {
        _sesionService = sesionService;
    }
    // Obtener todas las sesiones
    [HttpGet]
    public async Task<ActionResult> Get(CancellationToken cancellationToken)
    {
        var sesiones = await _sesionService.ObtenerSesiones(cancellationToken);
        return Ok(sesiones);
    }
    // Obtener una sesion por ID
    [HttpGet("{id}")]
    public async Task<ActionResult> Get(Guid id, CancellationToken cancellationToken)
    {
        var sesion = await _sesionService.ObtenerSesion(id, cancellationToken);
        return Ok(sesion);
    }
    // Crear una nueva sesion
    [HttpPost]
    public async Task<ActionResult> Post([FromBody] SesionCreacion request)
    {
        await _sesionService.CrearSesion(request);
        return Ok("Sesion creada con exito");
    }
    // Actualizar una sesion existente
    [HttpPut("{id}")]
    public async Task<ActionResult> Put(Guid id, [FromBody] SesionActualizacion request, CancellationToken cancellationToken)
    {
        var sesion = await _sesionService.ActualizarSesion(id, request, cancellationToken);
        return Ok(sesion);
    }
    // Eliminar una sesion
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        await _sesionService.EliminarSesion(id, cancellationToken);
        return Ok("Sesion eliminada correctamente");
    }
}

