using HabilitadorGraduaciones.Core.DTO;
using HabilitadorGraduaciones.Core.DTO.Base;
using HabilitadorGraduaciones.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HabilitadorGraduaciones.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificacionesController : Controller
    {
        private readonly INotificacionesService _notificacionesService;

        public NotificacionesController(INotificacionesService notificacionesService)
        {
            _notificacionesService = notificacionesService;
        }

        [HttpPost("EnviarCorreo/")]
        public async Task<ActionResult<BaseOutDto>> EnviarCorreo(CorreoDto correo)
        {
            BaseOutDto result = await _notificacionesService.EnviarCorreo(correo);
            return Ok(result);
        }

        [HttpGet("GetNotificaciones/{param}/{matricula}")]
        public async Task<ActionResult<NotificacionesDto>> GetNotificaciones(bool param, string matricula)
        {
            Notificacion _object = new Notificacion();
            _object.IsConsultaNotificacionesNoLeidas = param;
            _object.Matricula = matricula;
            NotificacionesDto result = await _notificacionesService.GetNotificaciones(_object);
            return Ok(result);
                      
            
        }

        [HttpPost("ActualizarEstatus/")]
        public async Task<ActionResult<NotificacionesDto>> ActualizarEstatusNotificacion(Notificacion param)
        {
            NotificacionesDto result = await _notificacionesService.ActualizarEstatus(param);
            return Ok(result);
        }

        [HttpPost("MarcarTodasLeidas/")]
        public async Task<ActionResult<BaseOutDto>> MarcarTodasLeidas(List<Notificacion> lista)
        {
            BaseOutDto result = await _notificacionesService.MarcarTodasLeidas(lista);
            return Ok(result);
        }

        [HttpPost("InsertarNotificacion/")]
        public async Task<ActionResult<NotificacionesDto>> InsertarNotificacion(Notificacion param)
        {
            NotificacionesDto result = await _notificacionesService.InsertarNotificacion(param);
            return Ok(result);           
        }

        [HttpPost("InsertarNotificacionCorreo/")]
        public async Task<ActionResult<NotificacionesDto>> InsertarNotificacionCorreo(NotificacionCorreoDto data)
        {
            NotificacionesDto result = await _notificacionesService.InsertarNotificacionCorreo(data);
            return Ok(result);
        }

        [HttpGet("BienvenidoGraduacion/{matricula}/{tipoCorreo}")]
        public async Task<ActionResult<NotificacionesDto>> BienvenidoGraduacion(string matricula, int tipoCorreo)
        {
            NotificacionesDto result = await _notificacionesService.BienvenidoGraduacion(matricula, tipoCorreo);
            return Ok(result);
        }

        [HttpGet("IsCorreoEnviado/{tipo}/{matricula}")]
        public async Task<ActionResult<NotificacionesDto>> IsCorreoEnviado(int tipo, string matricula)
        {
            NotificacionesDto result = await _notificacionesService.IsCorreoEnviado(tipo, matricula);
            return Ok(result);
            
        }
        [HttpGet("EnteradoCreditosInsuficientes/{matricula}")]
        public async Task<ActionResult<NotificacionesDto>> EnteradoCreditosInsuficientes(string matricula)
        {
            NotificacionesDto  result = await _notificacionesService.EnteradoCreditosInsuficientes(matricula);
            return Ok(result);
        }

        [HttpPost("EnviaCorreoConfirmacion/")]
        public async Task<ActionResult<BaseOutDto>> EnviaCorreoConfirmacion(DatosPersonalesCorreoDto dto) 
        {
            BaseOutDto result = await _notificacionesService.EnviaCorreoConfirmacion(dto);
            return Ok(result);
        }
    }
}
