using HabilitadorGraduaciones.Core.Entities.Bases;

namespace HabilitadorGraduaciones.Core.Entities
{
    public class ServicioSocialEntity : BaseEntity
    {
        public ServicioSocialEntity()
        {
            this.Lista_Horas = new List<Horas>();
        }
        public string ClaveIdentidad { get; set; }
        public string Carrera { get; set; }
        public List<Horas> Lista_Horas { get; set; }
        public DateTime UltimaActualizacionSS { get; set; }
        public bool isCumpleSS { get; set; }
        public bool isServicioSocial { get; set; }
    }

    public class Horas
    {
        public string HoraAcreditada { get; set; }
        public int ValorAcreditada { get; set; }
        public string HoraRequisito { get; set; }
        public int ValorRequisito { get; set; }
    }

    public class DetalleServicioSocialEntity : TarjetaDetalle
    {
        public string TituloSS { get; set; }
        public string DetalleSS { get; set; }
        public string EstadoDetalleSS { get; set; }
        public DudaSS Dudas_SS { get; set; }
    }

    public class DudaSS
    {
        public List<ContactoSS> ContactosSS { get; set; }
    }

    public class ContactoSS
    {
        public string Nombre { get; set; }
        public string Correo { get; set; }
    }
}