using HabilitadorGraduaciones.Core.DTO.Base;
using HabilitadorGraduaciones.Core.Entities;

namespace HabilitadorGraduaciones.Core.DTO
{
    public class ServicioSocialDto : BaseOutDto
    {
        public ServicioSocialDto()
        {
            Lista_Horas = new List<HorasDto>();
        }
        public string ClaveIdentidad { get; set; }
        public string Carrera { get; set; }
        public List<HorasDto> Lista_Horas { get; set; }
        public DateTime UltimaActualizacionSS { get; set; }
        public bool isCumpleSS { get; set; }
        public bool isServicioSocial { get; set; }
    }

    public class HorasDto
    {
        public string HoraAcreditada { get; set; }
        public int ValorAcreditada { get; set; }
        public string HoraRequisito { get; set; }
        public int ValorRequisito { get; set; }
    }

    public class DetalleServicioSocialDto: TarjetaDetalle
    {
        public string TituloSS { get; set; }
        public string DetalleSS { get; set; }
        public string EstadoDetalleSS { get; set; }
        public DudaSS Dudas_SS { get; set; }
    }

    public class DudaSSDto
    {
        public List<ContactoSSDto> ContactosSS { get; set; }
    }

    public class ContactoSSDto
    {
        public string Nombre { get; set; }
        public string Correo { get; set; }
    }
}