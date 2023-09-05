using HabilitadorGraduaciones.Core.DTO.Base;

namespace HabilitadorGraduaciones.Core.DTO
{
    public class ProgramaDto : BaseOutDto
    {
        public List<Programa> Programa { get; set; }
    }
    public class Programa
    {
        public string NombrePrograma { get; set; }
        public string NivelIngles { get; set; }
    }
}
