namespace PsicoAgenda.Domain.Models
{
    public class Sesion : EntidadBase
    {
        public Guid PacienteId { get; set; }
        public Guid? CitaId { get; set; }
        public string? SoapSubj { get; set; }
        public string? Observaciones { get; set; }
        public string? Analasis { get; set; }
        public string? PlanAccion { get; set; }
        public string? ArchivosJson { get; set; }

        // relations
        public Paciente? Paciente { get; set; }
        public Cita? Cita { get; set; }
        public List<SesionNota> Notas { get; set; } = new ();
    }
}
