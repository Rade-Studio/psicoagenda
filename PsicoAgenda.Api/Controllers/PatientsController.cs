using Microsoft.AspNetCore.Mvc;

namespace PsicoAgenda.Api.Controllers;

public record PatientDto(
    Guid Id,
    string FirstName,
    string LastName,
    string? Email,
    string? Phone,
    DateTime? BirthDate
);

[ApiController]
[Route("api/[controller]")]
public class PatientsController : ControllerBase
{
    // Datos de prueba en memoria
    private static readonly List<PatientDto> _patients = new()
    {
        new PatientDto(Guid.Parse("11111111-1111-1111-1111-111111111111"), "Ana", "Ramírez", "ana@mail.com", "3001111111", new DateTime(1995, 5, 10)),
        new PatientDto(Guid.Parse("22222222-2222-2222-2222-222222222222"), "Luis", "Torres",  "luis@mail.com", "3002222222", null),
        new PatientDto(Guid.Parse("33333333-3333-3333-3333-333333333333"), "María","Gómez",   null,             null,           new DateTime(2001, 12, 1))
    };

    // GET: /api/patients?q=ana
    [HttpGet]
    public IActionResult Get([FromQuery] string? q)
    {
        var list = _patients.AsEnumerable();

        if (!string.IsNullOrWhiteSpace(q))
        {
            var s = q.Trim().ToLowerInvariant();
            list = list.Where(p =>
                (p.FirstName + " " + p.LastName).ToLowerInvariant().Contains(s) ||
                (p.Email ?? "").ToLowerInvariant().Contains(s) ||
                (p.Phone ?? "").Contains(s));
        }

        return Ok(list.OrderBy(p => p.LastName).ThenBy(p => p.FirstName));
    }

    // GET: /api/patients/{id}
    [HttpGet("{id:guid}")]
    public IActionResult GetById(Guid id)
    {
        var p = _patients.FirstOrDefault(x => x.Id == id);
        return p is null ? NotFound() : Ok(p);
    }
}
