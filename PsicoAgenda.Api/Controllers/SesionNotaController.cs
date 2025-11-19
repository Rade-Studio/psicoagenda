using Microsoft.AspNetCore.Mvc;
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
}

