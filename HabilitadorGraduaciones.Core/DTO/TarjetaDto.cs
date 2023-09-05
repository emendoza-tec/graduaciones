using HabilitadorGraduaciones.Core.DTO.Base;

namespace HabilitadorGraduaciones.Core.DTO
{
    public class TarjetaDto : BaseOutDto
    {
        public int IdTarjeta { get; set; }
        public string Tarjeta { get; set; }
        public string Nota { get; set; }
        public string Contacto { get; set; }
        public string Correo { get; set; }
        public string Link { get; set; }
        public List<DocumentosDto> Documentos { get; set; }
        public string Idioma { get; set; }
    }

    public class DocumentosDto
    {
        public string Descripcion { get; set; }
        public Boolean Mexicano { get; set; }
        public Boolean Extranjero { get; set; }
        public int Orden { get; set; }
    }
}