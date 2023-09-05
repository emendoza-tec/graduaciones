using HabilitadorGraduaciones.Core.Entities;
using HabilitadorGraduaciones.Services;
using Microsoft.AspNetCore.Mvc;

namespace HabilitadorGraduaciones.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenuController : Controller
    {
        public IConfiguration _configuration { get; }

        public MenuController(IConfiguration configuration)
        {
            this._configuration = configuration;
        }

        [HttpGet]
        public async Task<ActionResult<List<MenuEntity>>> Get()
        {
            var menuService = new MenuService(_configuration);
            var menu = await menuService.Get();

            return menu;
        }
    }
}