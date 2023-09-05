using HabilitadorGraduaciones.Core.DTO.Base;
using HabilitadorGraduaciones.Core.Entities.Bases;

namespace HabilitadorGraduaciones.Core.DTO
{
    public class AvisosDto : BaseOutDto
    {
        public AvisosDto()
        {
            lstAvisos = new List<Aviso>();
        }
        public List<Aviso> lstAvisos { get; set; }
    }
}