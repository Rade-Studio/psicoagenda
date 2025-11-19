using PsicoAgenda.Application.Dtos.Dashboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PsicoAgenda.Application.Interfaces
{
    public interface IDashBoardService
    {
        Task<DashboardRespuesta> ObtenerDatosDashboardAsync(CancellationToken cancellationToken);
    }
}
