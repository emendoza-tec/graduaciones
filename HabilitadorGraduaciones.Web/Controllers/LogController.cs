using HabilitadorGraduaciones.Core.DTO;
using HabilitadorGraduaciones.Core.DTO.Base;
using HabilitadorGraduaciones.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HabilitadorGraduaciones.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogController : Controller
    {
        private readonly ILogService _logService;

        public LogController(ILogService logService)
        {
            _logService = logService;
        }
        [HttpPost("GuardarLog/")]
        public async Task<ActionResult<BaseOutDto>> GuardarLog(LogEnteradoDto data)
           => Ok(await _logService.GuardarLog(data));
    }
}
