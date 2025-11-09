using PsicoAgenda.Application.Dtos.Pacientes;
using PsicoAgenda.Domain.Enums;

namespace PsicoAgenda.Application.Dtos.Citas;

public class CitaRespuesta
{
    public Guid Id { get; set; }
    public Guid PacienteId { get; set; }
    public DateTime FechaInicio { get; set; }
    public DateTime FechaFin { get; set; }
    public ModoCita Modo { get; set; }
    public EstadoCita Estado { get; set; }
    public string? UbicacionLink { get; set; }
    public string? Notas { get; set; }
    public PacienteRespuesta Paciente { get; set; }
}

