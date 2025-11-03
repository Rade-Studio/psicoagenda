namespace PsicoAgenda.Application.Dtos.Pacientes;

public record PacienteCreacion(
    string Nombre,
    string Apellidos,
    string Email,
    string Telefono,
    DateTime FechaNacimiento
);