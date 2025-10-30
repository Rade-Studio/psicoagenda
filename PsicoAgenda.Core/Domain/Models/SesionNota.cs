using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PsicoAgenda.Core.Domain.Models
{
    public class SesionNota: EntidadBase
    {
        public string Nota { get; set; }
        public Guid SesionId { get; set; }

        // relations
        public Sesion? Sesion { get; set; }

    }
}
