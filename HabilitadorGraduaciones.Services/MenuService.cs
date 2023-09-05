using HabilitadorGraduaciones.Core.Entities;
using HabilitadorGraduaciones.Data;
using Microsoft.Extensions.Configuration;

namespace HabilitadorGraduaciones.Services
{
    public class MenuService
    {
        public IConfiguration Configuration { get; }

        public MenuService(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public async Task<List<MenuEntity>> Get()
        {
            var dao = new MenuData(Configuration);
            var menuPadre = dao.GetMenuPadre();
            return await menuPadre;
        }
    }
}