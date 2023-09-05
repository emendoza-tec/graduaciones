using HabilitadorGraduaciones.Core.Entities.Bases;

namespace HabilitadorGraduaciones.Core.Entities
{
    public class TarjetaDetalle : BaseEntity
    {
        public int IdTarjeta { get; set; }
        public string Nota { get; set; }
        public string Contacto { get; set; }
        public string Correo { get; set; }
        public string Link { get; set; }
    }
}