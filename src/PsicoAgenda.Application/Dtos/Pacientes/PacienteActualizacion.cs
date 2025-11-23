namespace PsicoAgenda.Application.Dtos.Pacientes;

public record PacienteActualizacion
(
    string Nombre,
    string Apellidos,
    string Email,
    string Telefono,
    string ContactoEmergencia,
    string TagsJson,
    DateTime FechaNacimiento
);



