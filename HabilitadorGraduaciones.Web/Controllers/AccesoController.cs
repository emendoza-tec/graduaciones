using AutoMapper;
using HabilitadorGraduaciones.Core.DTO;
using HabilitadorGraduaciones.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HabilitadorGraduaciones.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccesoController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IAccesosNominaService _accesosNominaService;

        public AccesoController(IMapper mapper, IAccesosNominaService accesosNominaService)
        {
            _mapper = mapper;
            _accesosNominaService = accesosNominaService;
        }

        [HttpGet("{matricula}")]
        public async Task<ActionResult<AccesosNominaDto>> GetAcceso(string matricula) =>
            Ok(_mapper.Map<AccesosNominaDto>(await _accesosNominaService.GetAcceso(matricula)));
    }
}
