using Microsoft.AspNetCore.Mvc;

namespace PsicoAgenda.Api.Controllers

{
    [ApiController]
    [Route("api/[controller]")]
    public class HealthController: ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new 
            { 
                service = "PsicoAgenda API",
                status = "Healthy",
                utc = DateTime.UtcNow
            });
        }
    }
}
