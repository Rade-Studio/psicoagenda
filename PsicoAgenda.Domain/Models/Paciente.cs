namespace PsicoAgenda.Domain.Models
{
    public class Paciente: EntidadBase
    {
        public string PrimerNombre { get; set; }
        public string? SegundoNombre { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string? Email { get; set; }
        public string? Telefono { get; set; }
        public string? ContactoEmergencia { get; set; }
        public string? TagsJson { get; set; }
    }
}
