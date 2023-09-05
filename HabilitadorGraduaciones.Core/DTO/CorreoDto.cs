using HabilitadorGraduaciones.Core.Entities.Bases;

namespace HabilitadorGraduaciones.Core.DTO
{
    public class CorreoDto : BaseEntity
    {
        public string Destinatario { get; set; }
        public string Asunto { get; set; }
        public string Cuerpo { get; set; }
        public string Adjuntos { get; set; }
        public string ConCopia { get; set; }
    }
}
