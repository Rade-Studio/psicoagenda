using PsicoAgenda.Domain.Enums;

namespace PsicoAgenda.Application.Dtos.Citas;

public record CitaActualizacion
(
    Guid PacienteId,
    DateTime FechaInicio,
    DateTime FechaFin,
    ModoCita Modo,
    EstadoCita Estado,
    string? UbicacionLink,
    string? Notas
);

