using HabilitadorGraduaciones.Core.DTO.Base;
using HabilitadorGraduaciones.Core.Entities;
using HabilitadorGraduaciones.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HabilitadorGraduaciones.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CampusCeremoniaGraduacionController : Controller
    {
        private readonly ICampusCeremoniaGraduacionService _campusCeremoniaService;

        public CampusCeremoniaGraduacionController(ICampusCeremoniaGraduacionService campusCeremoniaService)
        {
            _campusCeremoniaService = campusCeremoniaService;
        }
        [HttpPost()]
        public async Task<ActionResult<BaseOutDto>> GuardaCeremonia(CampusCeremoniaGraduacionEntity ceremonia)
            => Ok(await _campusCeremoniaService.GuardaCeremonia(ceremonia));
    }
}
