using HabilitadorGraduaciones.Core.Entities;

namespace HabilitadorGraduaciones.Core.DTO
{
    public class PlanDeEstudiosDto : Base.BaseOutDto
    {
        public decimal CreditosRequisito { get; set; }
        public decimal CreditosAcreditados { get; set; }
        public decimal CreditosFaltantes { get; set; }
        public List<CreditosPorCampus> CreditosPorCampus { get; set; }
        public DateTime UltimaActualizacionPE { get; set; }
        public decimal CreditosCursadosExtranjero { get; set; }
        public decimal CreditosInscritos { get; set; }
        public bool isCumplePlanDeEstudios { get; set; }
        public int TotalDeCampus { get; set; }
    }
    public class DetallePlanDeEstudios : TarjetaDetalle
    {
        public string TituloPE { get; set; }
        public string DetallePE { get; set; }
        public string EstadoDetallePE { get; set; }
        public DudaPE Dudas_PE { get; set; }
    }
    public class CreditosPorCampus
    {
        public string ClaveCampus { get; set; }
        public decimal CreditosCampus { get; set; }
    }
    public class DudaPE
    {
        public List<ContactoPE> ContactosPE { get; set; }
    }

    public class ContactoPE
    {
        public string Nombre { get; set; }
        public string Correo { get; set; }
    }
}