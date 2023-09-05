using HabilitadorGraduaciones.Core.Entities.Bases;

namespace HabilitadorGraduaciones.Core.Entities
{
    public class PlanDeEstudiosEntity : BaseEntity
    {
        public int CreditosRequisito { get; set; }
        public int CreditosAcreditados { get; set; }
        public DateTime UltimaActualizacionPE { get; set; }
        public bool isCumplePlanDeEstudios { get; set; }
    }

    public class DetallePlanDeEstudiosEntity : TarjetaDetalle
    {
        public string TituloPE { get; set; }
        public string DetallePE { get; set; }
        public string EstadoDetallePE { get; set; }
        public DudaPE Dudas_PE { get; set; }
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