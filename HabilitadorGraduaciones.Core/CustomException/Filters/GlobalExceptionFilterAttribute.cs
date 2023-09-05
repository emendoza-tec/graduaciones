
using HabilitadorGraduaciones.Core.Entities;
using HabilitadorGraduaciones.Core.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Text;

namespace HabilitadorGraduaciones.Core.CustomException.Filters
{
    [ExcludeFromCodeCoverage]
    public class GlobalExceptionFilterAttribute : ExceptionFilterAttribute
    {
        private readonly string _connectionString;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string mensajeError = "Ha surgido un error, consulte al administrador";
        public GlobalExceptionFilterAttribute(ILogger<GlobalExceptionFilterAttribute> logger, IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            _httpContextAccessor = httpContextAccessor;
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public override async Task OnExceptionAsync(ExceptionContext context)
        {
            HttpContext httpContext = _httpContextAccessor.HttpContext;
            var bitacoraLog = new BitacoraLog();
            try
            {
                if (context.Exception.GetType() == typeof(CustomException))
                {
                    bitacoraLog = ((CustomException)context.Exception).BitacoraLog;
                }
                else
                {
                    bitacoraLog.HttpStatusCode = HttpStatusCode.InternalServerError;
                    bitacoraLog.MensajeUsuario = mensajeError;
                    bitacoraLog.MensajeExcepcion = context.Exception.Message;
                    bitacoraLog.HResult = context.Exception.HResult;
                }

                bitacoraLog.InnerException = bitacoraLog.InnerException ?? context.Exception.InnerException?.Message;
                bitacoraLog.StackTrace = bitacoraLog.StackTrace ?? context.Exception.StackTrace;
                bitacoraLog.UsuarioAlta = httpContext.Session.GetString("usuarioId");

                var apiError = new HttpErrorResponse()
                {
                    HttpStatus = (int)bitacoraLog.HttpStatusCode,
                    Mensaje = bitacoraLog.MensajeUsuario
                };

                var response = apiError;
                context.Result = new ObjectResult(response) { StatusCode = (int)bitacoraLog.HttpStatusCode };
                context.ExceptionHandled = true;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                GuardarBitacora(bitacoraLog);
            }
        }

        private void GuardarBitacora(BitacoraLog bitacoraErrorDTO)
        {
            var json = JsonConvert.SerializeObject(bitacoraErrorDTO);
            try
            {
                //Guardar en DB 
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    using (SqlTransaction transaction = connection.BeginTransaction())
                    {
                        using (var command = new SqlCommand("spLogExcepciones_InsertaExcepcion"))
                        {

                            SqlParameter[] parameters =
                            {
                               new SqlParameter("@pErrorControlado", bitacoraErrorDTO.ErrorControlado),
                               new SqlParameter("@pMensajeUsuario", bitacoraErrorDTO.MensajeUsuario),
                               new SqlParameter("@pMensajeExcepcion", bitacoraErrorDTO.MensajeExcepcion),
                               new SqlParameter("@pStackTrace", bitacoraErrorDTO.StackTrace),
                               new SqlParameter("@pInnerException", bitacoraErrorDTO.InnerException),
                               new SqlParameter("@pHttpStatusCode", bitacoraErrorDTO.HttpStatusCode),
                               new SqlParameter("@pUsuarioAlta", bitacoraErrorDTO.UsuarioAlta)
                            };

                            foreach (var param in parameters)
                            {
                                var p = new SqlParameter
                                {
                                    ParameterName = param.ParameterName,
                                    Value = param.Value ?? DBNull.Value,
                                    SqlDbType = param.SqlDbType
                                };
                                command.Parameters.Add(p);
                            }

                            command.Connection = connection;
                            command.Transaction = transaction;
                            command.CommandType = CommandType.StoredProcedure;

                            try
                            {
                                command.ExecuteNonQuery();
                                transaction.Commit();
                            }
                            catch (Exception)
                            {
                                transaction.Rollback();
                                connection.Close();
                                throw;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //como no se pudo guardar en la db, guardar en un .txt
                FileStream fs = new FileStream(@AppDomain.CurrentDomain.BaseDirectory + "log.txt", FileMode.OpenOrCreate, FileAccess.Write);
                StreamWriter sw = new StreamWriter(fs);
                sw.BaseStream.Seek(0, SeekOrigin.End);
                sw.WriteLine(DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss") + Environment.NewLine + ex + Environment.NewLine + json);
                sw.Flush();
                sw.Close();
            }
        }
    }
}
