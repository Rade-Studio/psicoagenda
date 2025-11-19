using PsicoAgenda.Application.Dtos.Citas;
using PsicoAgenda.Application.Dtos.Pacientes;
using PsicoAgenda.Application.Dtos.SesionNota;

namespace PsicoAgenda.Application.Dtos.Sesiones
{
    public class SesionRespuesta
    {
        public Guid Id { get; set; }
        public Guid PacienteId { get; set; }
        public Guid CitaId { get; set; }
        public string? SoapSubj { get; set; }
        public string? Observaciones { get; set; }
        public string? Analasis { get; set; }
        public string? PlanAccion { get; set; }
        public string? ArchivosJson { get; set; }
        public PacienteRespuesta Paciente { get; set; }
        public CitaRespuesta Cita { get; set; }
        public List<SesionNotaRespuesta> Notas { get; set; } = new ();
    }
}
