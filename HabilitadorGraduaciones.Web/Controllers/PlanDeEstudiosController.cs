using AutoMapper;
using HabilitadorGraduaciones.Core.CustomException;
using HabilitadorGraduaciones.Core.DTO;
using HabilitadorGraduaciones.Data;
using HabilitadorGraduaciones.Data.Interfaces;
using HabilitadorGraduaciones.Services;
using HabilitadorGraduaciones.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace HabilitadorGraduaciones.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlanDeEstudiosController : Controller
    {
        private readonly IPlanDeEstudiosService _planDeEstudiosService;

        public PlanDeEstudiosController(IPlanDeEstudiosService planDeEstudiosService)
        {
            _planDeEstudiosService = planDeEstudiosService;
        }

        [HttpPost("ConsultaPlanDeEstudios/")]
        public async Task<ActionResult<PlanDeEstudiosDto>> ConsultarPlanDeEstudios(EndpointsDto dto) => Ok(await _planDeEstudiosService.GetPlanDeEstudios(dto));
    }
}