using HabilitadorGraduaciones.Core.DTO.Base;

namespace HabilitadorGraduaciones.Core.DTO
{
    public class SemanasTecExtDto : BaseOutDto
    {
        public int IdTarjeta { get; set; }
        public string Nota { get; set; }
        public string Contacto { get; set; }
        public string Correo { get; set; }
        public string Link { get; set; }
    }
}