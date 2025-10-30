using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PsicoAgenda.Core.Domain.Models
{
    public class Cuestionario: EntidadBase
    {
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string PreguntasJson { get; set; }
        public bool Activo { get; set; }

    }
}
