namespace PsicoAgenda.Domain.Models
{
    public class EntidadBase
    {
        public Guid Id { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaModificacion { get; set; }
        public DateTime FechaEliminacion { get; set; }
    }
}
