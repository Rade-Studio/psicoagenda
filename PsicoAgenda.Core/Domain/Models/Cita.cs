using PsicoAgenda.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PsicoAgenda.Core.Domain.Models
{
    public class Cita : EntidadBase
    {
        public Guid PacienteId { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public ModoCita Modo { get; set; }
        public EstadoCita Estado { get; set; }
        public string? UbicacionLink { get; set; }
        public string? Notas { get; set; }

        // relations
        public Paciente? Paciente { get; set; }

    }
}
