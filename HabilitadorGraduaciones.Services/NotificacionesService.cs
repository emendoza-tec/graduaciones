using HabilitadorGraduaciones.Core.CustomException;
using HabilitadorGraduaciones.Core.DTO;
using HabilitadorGraduaciones.Core.DTO.Base;
using HabilitadorGraduaciones.Data.Interfaces;
using HabilitadorGraduaciones.Data.Utils.Enums;
using HabilitadorGraduaciones.Services.Interfaces;
using System.Text;

namespace HabilitadorGraduaciones.Services
{
    public class NotificacionesService : INotificacionesService
    {
        private readonly INotificacionesRepository _notificacionesData;
        private readonly IEmailModuleRepository _emailData;
        public NotificacionesService(INotificacionesRepository notificacionesData, IEmailModuleRepository emailData)
        {
            _notificacionesData = notificacionesData;
            _emailData = emailData;
        }
        /// <summary>Metodo de envio de correo individual o masivo </summary>
        /// <param name="correo">correo del alumno.</param>
        /// <returns>Objeto con inforamación del usuario.</returns>
        public async Task<BaseOutDto> EnviarCorreo(CorreoDto correo)
        {
            BaseOutDto result = new BaseOutDto();
            try
            {
                return await _emailData.EnviarCorreo(
                    correo.Destinatario ?? "",
                    correo.Asunto ?? "",
                    correo.Cuerpo ?? "",
                    correo.Adjuntos ?? "",
                    correo.ConCopia ?? "",
                    (int)TipoCorreo.Notificaciones
                );

            }
            catch (Exception ex)
            {
                result.Result = false;
                result.ErrorMessage = ex.Message;
                return result;
                throw new CustomException("Error al enviar Correo", ex);
            }
        }
        public async Task<NotificacionesDto> GetNotificaciones(Notificacion param)
        {
            return await _notificacionesData.GetNotificaciones(param);
        }
        public async Task<NotificacionesDto> ActualizarEstatus(Notificacion _object)
        {
            return await _notificacionesData.GetNotificaciones(_object);
        }
        public async Task<BaseOutDto> MarcarTodasLeidas(List<Notificacion> lista)
        {
            BaseOutDto result = new BaseOutDto();
            try
            {
                Notificacion _object = new();
                StringBuilder arrNotificaciones = new();
                if(lista.Count > 0)
                {
                    foreach (var element in lista)
                    {
                        if (element.Activo)
                        {
                            if (element.IsNotificacion)
                            {
                                arrNotificaciones.Append(string.Format("{0}:0 ,", element.Id.ToString()));
                            }
                            else
                            {
                                arrNotificaciones.Append(string.Format("0:{0} ,", element.Id.ToString()));
                            }
                        }
                    }
                    _object.IsModificarTodas = true;
                    _object.ListNotificacionesLeidas = arrNotificaciones.ToString();
                    _object.Matricula = lista.First().Matricula;
                    await _notificacionesData.GetNotificaciones(_object);
                    result.Result = true;
                }
               
            }
            catch (Exception ex)
            {
                result.Result = false;
                result.ErrorMessage = ex.Message;
                throw new CustomException("Error al marcar las notificaciones como leídas", ex);
            }
            return result;
        }
        public async Task<NotificacionesDto> InsertarNotificacion(Notificacion _object)
        {
            return await _notificacionesData.InsertarNotificacion(_object);
        }
        public async Task<NotificacionesDto> InsertarNotificacionCorreo(NotificacionCorreoDto data)
        {
            return await _notificacionesData.InsertarNotificacionCorreo(data);
        }
        public async Task<NotificacionesDto> BienvenidoGraduacion(string matricula, int tipoCorreo)
        {
            return await _notificacionesData.BienvenidoGraduacion(matricula, tipoCorreo);
        }
        public async Task<NotificacionesDto> IsCorreoEnviado(int tipo, string matricula)
        {
            return await _notificacionesData.IsCorreoEnviado(tipo, matricula);
        }
        public async Task<NotificacionesDto> EnteradoCreditosInsuficientes(string matricula)
        {
            return await _notificacionesData.EnteradoCreditosInsuficientes(matricula);
        }

        public async Task<BaseOutDto> EnviaCorreoConfirmacion(DatosPersonalesCorreoDto datos)
        {
            return await _notificacionesData.EnviaCorreoConfirmacion(datos);
        }
    }
}
