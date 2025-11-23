namespace PsicoAgenda.Application.Dtos.Pacientes;

public class PacienteCreacion
{
    public string Nombre { get; set; }
    public string Apellidos { get; set; }
    public string Email { get; set; }
    public string Telefono { get; set; }
    public string ContactoEmergencia { get; set; }
    public string TagsJson { get; set; }
    public DateTime FechaNacimiento { get; set; }
}