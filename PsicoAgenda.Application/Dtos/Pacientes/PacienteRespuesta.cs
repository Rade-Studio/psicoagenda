namespace PsicoAgenda.Application.Dtos.Pacientes;

public record PacienteRespuesta(
    Guid Id,
    string Nombre,
    string Apellidos,
    string Email,
    string Telefono,
    DateTime FechaNacimiento
);
