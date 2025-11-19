using PsicoAgenda.Application.Dtos.Citas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PsicoAgenda.Application.Dtos.Dashboard
{
    public class DashboardRespuesta
    {
        public int TotalPacientes { get; set; }
        public int TotalSesiones { get; set; }
        public int TotalCitasHoy { get; set; }
        public IEnumerable<CitaRespuesta> CitasProximas { get; set; } = Enumerable.Empty<CitaRespuesta>();
    }
}
