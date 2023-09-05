using HabilitadorGraduaciones.Core.Entities.Expediente;

namespace HabilitadorGraduaciones.Core.Entities
{
    public class ProcesosExpedienteEntity
    {
        public List<ExpedienteEntity> ExpedienteNuevos { get; set; }
        public List<ExpedienteEntity> ExpedienteAtualizados { get; set; }
        public List<ExpedienteEntity> ExpedienteIncompletoSinDetalle { get; set; }
        public List<ExpedienteEntity> ExpedienteCambioaCompleto { get; set; }
        public List<ExpedienteEntity> ExpedienteCambiodeCompleto { get; set; }
        public List<ExpedienteEntity> ExpedienteErrorFiltro { get; set; }
    }
}
