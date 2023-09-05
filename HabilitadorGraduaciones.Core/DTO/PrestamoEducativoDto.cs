using HabilitadorGraduaciones.Core.DTO.Base;

namespace HabilitadorGraduaciones.Core.DTO
{
    public class PrestamoEducativoDto : BaseOutDto
    {
        public string EstatusContrato { get; set; }
        public bool TienePrestamo { get; set; }
    }
}
