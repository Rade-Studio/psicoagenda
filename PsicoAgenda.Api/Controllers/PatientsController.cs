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

public record PatientUpdateRequestDto(
    string? FirstName,
    string? LastName,
    string? Email,
    string? Phone,
    DateTime? BirthDate
);

public static class PatientDtoExtensions
{
    public static PatientDto ToDto(this PatientRequestDto reuest)
    {
        return new PatientDto(
            Guid.NewGuid(),
            reuest.FirstName,
            reuest.LastName,
            reuest.Email,
            reuest.Phone,
            reuest.BirthDate
        );
    }
    
    public static PatientDto ToDto(this PatientUpdateRequestDto request, Guid id)
    {
        return new PatientDto(
            id,
            request.FirstName ?? "",
            request.LastName ?? "",
            request.Email,
            request.Phone,
            request.BirthDate
        );
    }
}

public record PatientRequestDto(
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
    private static List<PatientDto> _patients = new()
    {
        new PatientDto(Guid.Parse("11111111-1111-1111-1111-111111111111"), "Ana", "Ramírez", "ana@mail.com",
            "3001111111", new DateTime(1995, 5, 10)),
        new PatientDto(Guid.Parse("22222222-2222-2222-2222-222222222222"), "Luis", "Torres", "luis@mail.com",
            "3002222222", null),
        new PatientDto(Guid.Parse("33333333-3333-3333-3333-333333333333"), "María", "Gómez", null, null,
            new DateTime(2001, 12, 1))
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

    [HttpPost]
    public IActionResult Post([FromBody] PatientRequestDto? request)
    {
        try
        {
            if (request is null)
            {
                return BadRequest();
            }

            var patient = request.ToDto();

            _patients.Add(patient);

            return CreatedAtAction(nameof(GetById), new { id = patient.Id }, patient);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPut]
    public IActionResult Put([FromQuery] Guid id, [FromBody] PatientUpdateRequestDto? request)
    {
        try
        {
            if (request is null)
            {
                return BadRequest();
            }

            if (id == Guid.Empty)
            {
                return BadRequest();
            }

            if (_patients.All(x => x.Id != id))
            {
                return NotFound();
            }
            
            var patient = _patients.First(x => x.Id == id);
            _patients.Remove(patient);
            var updatedPatient = request.ToDto(patient.Id);
            _patients.Add(updatedPatient);
            
            return Ok(updatedPatient);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return StatusCode(500, ex.Message);
        }
    }
}