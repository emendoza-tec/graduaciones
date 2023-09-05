using HabilitadorGraduaciones.Core.Entities;
using System.Diagnostics.CodeAnalysis;
using System.Net;

namespace HabilitadorGraduaciones.Core.CustomException
{
    [ExcludeFromCodeCoverage]
    [Serializable]
    public class CustomException : Exception
    {
        public BitacoraLog BitacoraLog { get; set; }

        public CustomException(string message, Exception ex) : base(message)
        {
            this.BitacoraLog = new BitacoraLog()
            {
                MensajeUsuario = message,
                MensajeExcepcion = ex.Message,
                HttpStatusCode = HttpStatusCode.InternalServerError,
                InnerException = ex.InnerException?.Message,
                StackTrace = ex.StackTrace,
                HResult = ex.HResult,
                ErrorControlado = true
            };
        }

        public CustomException(string message, Exception ex, HttpStatusCode httpStatusCode) : base(message)
        {
            this.BitacoraLog = new BitacoraLog()
            {
                MensajeUsuario = message,
                MensajeExcepcion = ex.Message,
                HttpStatusCode = httpStatusCode,
                InnerException = ex.InnerException?.Message,
                StackTrace = ex.StackTrace,
                HResult = ex.HResult,
                ErrorControlado = true
            };
        }
        public CustomException(string message, HttpStatusCode httpStatusCode) : base(message)
        {
            this.BitacoraLog = new BitacoraLog()
            {
                MensajeUsuario = message,
                HttpStatusCode = httpStatusCode,
                ErrorControlado = true
            };
        }
    }
}
