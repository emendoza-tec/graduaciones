using HabilitadorGraduaciones.Core.CustomException;
using HabilitadorGraduaciones.Core.DTO;
using HabilitadorGraduaciones.Core.DTO.Base;
using HabilitadorGraduaciones.Data.Interfaces;
using HabilitadorGraduaciones.Data.Utils;
using HabilitadorGraduaciones.Data.Utils.Enums;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Text;

namespace HabilitadorGraduaciones.Data
{
    public class NotificacionesData : INotificacionesRepository
    {
        public const string ConnectionStrings = "ConnectionStrings:DefaultConnection";
        public IConfiguration Configuration { get; }
        private readonly IEmailModuleRepository _emailData;

        public NotificacionesData(IConfiguration configuration, IEmailModuleRepository emailData)
        {
            Configuration = configuration;
            _emailData = emailData;
        }
        public async Task<NotificacionesDto> GetNotificaciones(Notificacion notificacion)
        {
            NotificacionesDto Notificaciones = new NotificacionesDto();
            List<Notificacion> lista = new List<Notificacion>();
            IList<Parameter> _params = new List<Parameter>
            {
                DataBase.CreateParameter("@Id", DbType.Int32, 9, ParameterDirection.Input, true, null, DataRowVersion.Default, notificacion.Id ),
                DataBase.CreateParameter("@Matricula", DbType.String, 1000, ParameterDirection.Input, true, null, DataRowVersion.Default, notificacion.Matricula ),
                DataBase.CreateParameter("@IsModificarTodos", DbType.Boolean, 2, ParameterDirection.Input, true, null, DataRowVersion.Default, notificacion.IsModificarTodas ),
                DataBase.CreateParameter("@IsModificarNotificacion", DbType.Boolean, 2, ParameterDirection.Input, true, null, DataRowVersion.Default, notificacion.IsModificarNotificacion ),
                DataBase.CreateParameter("@IsNotificacion", DbType.Boolean, 2, ParameterDirection.Input, true, null, DataRowVersion.Default, notificacion.IsNotificacion ),
                DataBase.CreateParameter("@IsConsultaNotificacionesNoLeidas", DbType.Boolean, 2, ParameterDirection.Input, true, null, DataRowVersion.Default, notificacion.IsConsultaNotificacionesNoLeidas ),
                DataBase.CreateParameter("@NotificacionesList", DbType.String, 1000, ParameterDirection.Input, true, null, DataRowVersion.Default, notificacion.ListNotificacionesLeidas )
             };

            if (notificacion.IsModificarNotificacion || notificacion.IsModificarTodas)
            {
                await DataBase.InsertOut("spNotificaciones_GeneralNotificaciones", CommandType.StoredProcedure, _params, Configuration[ConnectionStrings]);
            }
            else
            {
                using (IDataReader reader = await DataBase.GetReader("spNotificaciones_GeneralNotificaciones", CommandType.StoredProcedure, _params, Configuration[ConnectionStrings]))
                {
                    while (reader.Read())
                    {
                        var _notificacion = new Notificacion
                        {
                            Id = ComprobarNulos.CheckIntNull(reader["ID"]),
                            Titulo = ComprobarNulos.CheckStringNull(reader["TITULO"]),
                            Descripcion = ComprobarNulos.CheckStringNull(reader["DESCRIPCION"]),
                            Activo = ComprobarNulos.CheckBooleanNull(reader["ACTIVO"]),
                            FechaRegistro = ComprobarNulos.CheckDateTimeNull(reader["FECHA_REGISTRO"])
                        };
                        lista.Add(_notificacion);
                    }
                    Notificaciones.ListaNotificaciones = lista;
                    Notificaciones.Result = true;
                }
            }
            return Notificaciones;
        }
        public async Task<NotificacionesDto> InsertarNotificacion(Notificacion notificacion)
        {
            NotificacionesDto Notificaciones = new NotificacionesDto();
            try
            {
                IList<Parameter> _params = new List<Parameter>
                {
                    DataBase.CreateParameter("@Titulo", DbType.String, 250, ParameterDirection.Input, true, null, DataRowVersion.Default, notificacion.Titulo ),
                    DataBase.CreateParameter("@Descripcion", DbType.String, 1000, ParameterDirection.Input, true, null, DataRowVersion.Default, notificacion.Descripcion ),
                    DataBase.CreateParameter("@Matricula", DbType.String, 50, ParameterDirection.Input, true, null, DataRowVersion.Default, notificacion.Matricula )
                };
                await DataBase.InsertOut("spNotificaciones_InsertarNotificacion", CommandType.StoredProcedure, _params, Configuration[ConnectionStrings]);
                Notificaciones.Result = true;
            }
            catch (Exception ex)
            {
                Notificaciones.Result = false;
                Notificaciones.ErrorMessage = ex.Message;
                throw new CustomException("Error al insertar la notificación en el método InsertarNotificacion", ex);
            }
            return Notificaciones;
        }

        public async Task<NotificacionesDto> InsertarNotificacionCorreo(NotificacionCorreoDto data)
        {
            NotificacionesDto Notificaciones = new NotificacionesDto();
            try
            {
                IList<Parameter> _params = new List<Parameter>
                {
                    DataBase.CreateParameter("@Matricula", DbType.String, 50, ParameterDirection.Input, true, null, DataRowVersion.Default, data.Matricula),
                    DataBase.CreateParameter("@TipoCorreo", DbType.Int32, 50, ParameterDirection.Input, true, null, DataRowVersion.Default, data.TipoCorreo)
                };
                await DataBase.InsertOut("spNotificaciones_InsertarNotificacionCorreo", CommandType.StoredProcedure, _params, Configuration[ConnectionStrings]);
                Notificaciones.Result = true;
            }
            catch (Exception ex)
            {
                Notificaciones.Result = false;
                Notificaciones.ErrorMessage = ex.Message;
                throw new CustomException("Error al insertar la notificación de correo en el método InsertarNotificacionCorreo", ex);
            }
            return Notificaciones;
        }

        public async Task<NotificacionesDto> BienvenidoGraduacion(string matricula, int tipoCorreo)
        {
            NotificacionesDto notificacionBienvenidoG = new NotificacionesDto();
            string query = "select [dbo].[fnNotificaciones_NotificacionBienvenidaRegistrada]('" + matricula + "', "+ tipoCorreo +") as IsMostrarNotificacion";
            using (IDataReader readerBienvenidoG = await DataBase.GetReader(query, CommandType.Text, Configuration[ConnectionStrings]))
            {
                while (readerBienvenidoG.Read())
                {
                    notificacionBienvenidoG.Result = !readerBienvenidoG.IsDBNull(readerBienvenidoG.GetOrdinal("IsMostrarNotificacion")) && readerBienvenidoG.GetBoolean(readerBienvenidoG.GetOrdinal("IsMostrarNotificacion"));
                }
            }
            return notificacionBienvenidoG;
        }
        public async Task<NotificacionesDto> IsCorreoEnviado(int tipo, string matricula)
        {
            NotificacionesDto notificacion = new NotificacionesDto();
            string query = "select [dbo].[fnNotificaciones_NotificacionCorreoEnviada](" + tipo + ", '" + matricula + "') as IsCorreoEnviado";
            using (IDataReader readerBienvenidoG = await DataBase.GetReader(query, CommandType.Text, Configuration[ConnectionStrings]))
            {
                while (readerBienvenidoG.Read())
                {
                    notificacion.Result = !readerBienvenidoG.IsDBNull(readerBienvenidoG.GetOrdinal("IsCorreoEnviado")) && readerBienvenidoG.GetBoolean(readerBienvenidoG.GetOrdinal("IsCorreoEnviado"));

                }
            }
            return notificacion;
        }
        public async Task<NotificacionesDto> EnteradoCreditosInsuficientes(string matricula)
        {
            NotificacionesDto notificacionBienvenidoG = new NotificacionesDto();
            string query = "select [dbo].[fnNotificaciones_EnteradoCreditosInsuficientes]('" + matricula + "') as IsMostrarNotificacion";
            using (IDataReader readerBienvenidoG = await DataBase.GetReader(query, CommandType.Text, Configuration[ConnectionStrings]))
            {
                while (readerBienvenidoG.Read())
                {
                    notificacionBienvenidoG.Result = !readerBienvenidoG.IsDBNull(readerBienvenidoG.GetOrdinal("IsMostrarNotificacion")) && readerBienvenidoG.GetBoolean(readerBienvenidoG.GetOrdinal("IsMostrarNotificacion"));
                }
            }
            return notificacionBienvenidoG;
        }

        public async Task<BaseOutDto> EnviaCorreoConfirmacion(DatosPersonalesCorreoDto datos)
        {
            BaseOutDto result = new BaseOutDto();            
            try
            {
                string nombre = datos.Nombre + " " + datos.ApellidoPaterno + " " + datos.ApellidoMaterno;
                StringBuilder template = HtmlTemplate.GetTemplateConfirmacionDatosPersonales();
                template.Replace("HomePage", Configuration["ConfiguracionLinks:Home"]);
                template.Replace("#Usuario#", datos.Nombre);

                StringBuilder concentraciones = new StringBuilder();
                if (datos.Concentracion.Count > 0)
                {
                    foreach (var concentracion in datos.Concentracion)
                    {
                        concentraciones.Append(@"<li style='list-style: none;'>"+ concentracion + "</li>");
                    }
                }
                else
                {
                    concentraciones.Append("<li style='list-style: none;'>Ninguno</li>");
                }

                StringBuilder diplomas = new StringBuilder();
                if (datos.DiplomasAdicionales.Count > 0)
                {
                    foreach (var diploma in datos.DiplomasAdicionales)
                    {
                        diplomas.Append("<li style='list-style: none;'>" + diploma + "</li>");
                    }
                }
                else
                {
                    diplomas.Append("<li style='list-style: none;'>Ninguno</li>");
                }


                template.Replace("#Nombre#", nombre);
                template.Replace("#Programa#", datos.ProgramaAcademico);
                template.Replace("#Curp#", datos.Curp);
                template.Replace("#Concentracion#", concentraciones.ToString());
                template.Replace("#Diploma#", diplomas.ToString());
                string cuerpo = template.ToString();
                var correo = await _emailData.EnviarCorreo(datos.Correo, "Confirmación de la validación de datos personales", cuerpo, "", "", (int)TipoCorreo.ConfirmacionDatos);
                result.Result = correo.Result;
            }
            catch (Exception ex)
            {
                result.Result = false;
                result.ErrorMessage = ex.Message;
                throw new CustomException("Error al enviar correo de confirmación en metodo EnviaCorreoConfirmacion", ex);
            }
            return result;
        }
    }
}
