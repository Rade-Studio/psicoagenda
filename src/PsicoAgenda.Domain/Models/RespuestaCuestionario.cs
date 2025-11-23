namespace PsicoAgenda.Domain.Models
{
    public class RespuestaCuestionario: EntidadBase
    {
        public Guid CuestionarioId { get; set; }
        public Guid PacienteId { get; set; }
        public Guid SesionId { get; set; }
        public string RespuestasJson { get; set; }
        public string? Puntaje { get; set; }

        // relations
        public Paciente? Paciente { get; set; }
        public Sesion? Sesion { get; set; }
        public Cuestionario? Cuestionario { get; set; }
    }
}
