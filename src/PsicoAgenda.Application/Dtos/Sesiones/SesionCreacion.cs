using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PsicoAgenda.Application.Dtos.Sesiones
{
    public class SesionCreacion
    {
        public Guid PacienteId { get; set; }
        public Guid? CitaId { get; set; }
        public string? SoapSubj { get; set; }
        public string? Observaciones { get; set; }
        public string? Analasis { get; set; }
        public string? PlanAccion { get; set; }
        public string? ArchivosJson { get; set; }
    }
}
