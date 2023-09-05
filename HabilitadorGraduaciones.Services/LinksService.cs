using HabilitadorGraduaciones.Core.DTO;
using HabilitadorGraduaciones.Data.Interfaces;
using HabilitadorGraduaciones.Services.Interfaces;

namespace HabilitadorGraduaciones.Services
{
    public class LinksService : ILinksService
    {
        private readonly ILinksRepository _linksData;

        public LinksService(ILinksRepository linksData)
        {
            _linksData = linksData;
        }

        public LinksDto GetLinks()
        {
            return _linksData.GetLinks();
        }
    }
}
