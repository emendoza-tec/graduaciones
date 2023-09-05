using HabilitadorGraduaciones.Core.DTO.Base;

namespace HabilitadorGraduaciones.Core.DTO
{
    public class MenuOutDto : BaseOutDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Path { get; set; }
        public string Icono { get; set; }
        public int IdMenu { get; set; }
        public List<MenuHijoOutDto> MenuHijo { get; set; }
    }

    public class MenuHijoOutDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Path { get; set; }
        public string Icono { get; set; }
        public int IdMenu { get; set; }
        public bool Result { get; set; }
    }
}