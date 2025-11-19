using Microsoft.AspNetCore.Mvc;
using PsicoAgenda.Application.Interfaces;

namespace PsicoAgenda.Api.Controllers;

public class DashBoardController : ControllerBase
{
    private readonly IDashBoardService _dashBoardService;
    public DashBoardController(IDashBoardService dashBoardService)
    {
        _dashBoardService = dashBoardService;
    }
    [HttpGet("api/dashboard/summary")]
    public async Task<ActionResult> GetSummary(CancellationToken cancellationToken)
    {
        var summary = await _dashBoardService.ObtenerDatosDashboardAsync(cancellationToken);
        return Ok(summary);
    }
}


