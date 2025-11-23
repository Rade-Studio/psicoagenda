namespace PsicoAgenda.Application.Dtos.Sesiones;
public record SesionActualizacion
(
    Guid PacienteId,
    string? SoapSubj,
    string? Observaciones,
    string? Analasis,
    string? PlanAccion
);
