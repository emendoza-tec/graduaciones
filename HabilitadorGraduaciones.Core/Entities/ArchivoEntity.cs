using Microsoft.AspNetCore.Http;

namespace HabilitadorGraduaciones.Core.Entities
{
    public class ArchivoEntity

    {
        public IFormFile ArchivoRecibido { get; set; }

        public string UsuarioApplicacion { get; set; }

        public string Result { get; set; }

    }
}