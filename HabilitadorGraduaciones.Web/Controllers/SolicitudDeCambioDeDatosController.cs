using HabilitadorGraduaciones.Core.DTO;
using HabilitadorGraduaciones.Core.DTO.Base;
using HabilitadorGraduaciones.Core.Entities;
using HabilitadorGraduaciones.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HabilitadorGraduaciones.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SolicitudDeCambioDeDatosController : Controller
    {
        private readonly IWebHostEnvironment _env;
        private readonly ISolicitudDeCambioDeDatosService _solicitudessoliService;

        public SolicitudDeCambioDeDatosController(IWebHostEnvironment env, ISolicitudDeCambioDeDatosService solicitudessoliService)
        {
            _solicitudessoliService = solicitudessoliService;
            _env = env;
        }

        [HttpGet("GetEstatusSolicitudes")]
        public async Task<ActionResult<List<EstatusSolicitudDatosPersonalesEntity>>> GetEstatusSolicitudes()
            => Ok(await _solicitudessoliService.GetEstatusSolicitudes());


        [HttpGet("GetPendientes/{idUsuario}")]
        public async Task<ActionResult<List<SolicitudDeCambioDeDatosEntity>>> GetPendientes(int idUsuario)
            => Ok(await _solicitudessoliService.GetPendientes(idUsuario));

        [HttpGet("{idEstatusSolicitud}/{idUsuario}")]
        public async Task<ActionResult<List<SolicitudDeCambioDeDatosEntity>>> Get(int idEstatusSolicitud, int idUsuario)
            => Ok(await _solicitudessoliService.Get(idEstatusSolicitud, idUsuario));

        [HttpGet("GetDetalle/{idSolicitud}")]
        public async Task<ActionResult<List<DetalleSolicitudDeCambioDeDatosEntity>>> GetDetalle(int idSolicitud)
            => Ok(await _solicitudessoliService.GetDetalle(idSolicitud));

        [HttpPost()]
        public async Task<ActionResult<object>> GuardaSolicitudes(List<SolicitudDeCambioDeDatosDto> solicitudes)
           => Ok(await _solicitudessoliService.GuardaSolicitudes(solicitudes));

        [HttpPost("ModificaSolicitud")]
        public async Task<ActionResult<BaseOutDto>> ModificaSolicitud(ModificarEstatusSolicitudDto solicitud)
            => Ok(await _solicitudessoliService.ModificaSolicitud(solicitud));

        [HttpGet("GetConteoPendientes/{idUsuario}")]
        public async Task<ActionResult<TotalSolicitudesDto>> GetConteoPendientes(int idUsuario)
            => Ok(await _solicitudessoliService.GetConteoPendientes(idUsuario));

        [HttpGet("DownloadFile")]
        public async Task<IActionResult> DownloadFile(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                return Content("El nombre del archivo está vacío...");
            }

            var filePath = Path.Combine(_env.WebRootPath, fileName);

            var memoryStream = new MemoryStream();

            using (var stream = new FileStream(filePath, FileMode.Open))
            {
                await stream.CopyToAsync(memoryStream);
            }

            memoryStream.Position = 0;

            return File(memoryStream, "APPLICATION/octet-stream", Path.GetFileName(filePath));
        }

        [HttpGet("GetCorreo")]
        public async Task<ActionResult<CorreoSolicitudDatosPersonalesDto>> GetCorreo(int idCorreo)
            => Ok(await _solicitudessoliService.GetCorreo(idCorreo));
    }
}
