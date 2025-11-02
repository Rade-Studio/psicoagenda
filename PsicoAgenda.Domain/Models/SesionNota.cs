namespace PsicoAgenda.Domain.Models
{
    public class SesionNota: EntidadBase
    {
        public string Nota { get; set; }
        public Guid SesionId { get; set; }

        // relations
        public Sesion? Sesion { get; set; }

    }
}
