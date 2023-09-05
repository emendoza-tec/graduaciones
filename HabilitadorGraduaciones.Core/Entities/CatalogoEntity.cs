using HabilitadorGraduaciones.Core.Entities.Bases;

namespace HabilitadorGraduaciones.Core.Entities
{
    public class CatalogoEntity : BaseEntity
    {
        public string Clave { get; set; }
        public string Descripcion { get; set; }
    }
}