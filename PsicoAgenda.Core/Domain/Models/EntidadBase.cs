using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PsicoAgenda.Core.Domain.Models
{
    public class EntidadBase
    {
        Guid Id { get; set; }
        DateTime FechaCreacion { get; set; }
        DateTime FechaModificacion { get; set; }
        DateTime FechaEliminacion { get; set; }
    }
}
