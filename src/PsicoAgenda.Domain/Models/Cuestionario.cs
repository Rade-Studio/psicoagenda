namespace PsicoAgenda.Domain.Models
{
    public class Cuestionario: EntidadBase
    {
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string PreguntasJson { get; set; }
        public bool Activo { get; set; }

    }
}
