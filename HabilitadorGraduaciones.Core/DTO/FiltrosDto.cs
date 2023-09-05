using HabilitadorGraduaciones.Core.DTO.Base;

namespace HabilitadorGraduaciones.Core.DTO
{
    public class FiltrosDto : BaseOutDto
    {
        public FiltrosDto(List<CatalogoDto> nivel, List<CatalogoDto> campus, List<CatalogoDto> sedes,
            List<CatalogoDto> escuelas, List<CatalogoDto> programas, List<CatalogoDto> requisitos,
            List<CatalogoDto> roll)
        {
            this.Nivel = nivel;
            this.Campus = campus;
            this.Sedes = sedes;
            this.Escuelas = escuelas;
            this.Programas = programas;
            this.Requisitos = requisitos;
            this.Roll = roll;
        }
        public List<CatalogoDto> Nivel { get; set; }
        public List<CatalogoDto> Campus { get; set; }
        public List<CatalogoDto> Sedes { get; set; }
        public List<CatalogoDto> Escuelas { get; set; }
        public List<CatalogoDto> Programas { get; set; }
        public List<CatalogoDto> Requisitos { get; set; }
        public List<CatalogoDto> Roll { get; set; }
    }
}