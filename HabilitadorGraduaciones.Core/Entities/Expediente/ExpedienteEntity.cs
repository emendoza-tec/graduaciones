using HabilitadorGraduaciones.Core.Entities.Bases;
namespace HabilitadorGraduaciones.Core.Entities.Expediente
{
    public class ExpedienteEntity : BaseEntity
    {
        public string Estatus { get; set; }
        public string Detalle { get; set; }
        public DateTime UltimaActualizacion { get; set; }
        public bool isModificarAlumno { get; set; }
    }
}