using HabilitadorGraduaciones.Core.CustomException;
using HabilitadorGraduaciones.Core.DTO;
using HabilitadorGraduaciones.Data.Interfaces;
using Microsoft.Extensions.Configuration;

namespace HabilitadorGraduaciones.Data
{
    public class LinksData : ILinksRepository
    {
        public IConfiguration Configuration { get; }

        public LinksData(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public LinksDto GetLinks()
        {
            try
            {
                LinksDto dto = new()
                {
                  PrestamoEducativo = Configuration["ConfiguracionLinks:PrestamoEducativo"],
                  Tesoreria = Configuration["ConfiguracionLinks:Tesoreria"],
                  DatosPersonales = Configuration["ConfiguracionLinks:DatosPersonales"],
                  Distinciones = Configuration["ConfiguracionLinks:Distinciones"],
                  Result = true
                };
                
                return dto;
            }
            catch (Exception ex)
            {
                throw new CustomException("Ocurrió un error en el metodo GetLinks()", ex);
            }
        }
    }
}
