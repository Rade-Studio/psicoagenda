using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PsicoAgenda.Application.Dtos.SesionNota
{
    public class SesionNotaCreacion
    {
        public Guid SesionId { get; set; }
        public string Contenido { get; set; } = string.Empty;
    }
}
