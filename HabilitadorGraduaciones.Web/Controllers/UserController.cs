using HabilitadorGraduaciones.Web.Common;
using HabilitadorGraduaciones.Web.Models.Common;
using Microsoft.AspNetCore.Mvc;

namespace HabilitadorGraduaciones.Web.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    //[EnableCors("CorsPolicy")]
    public class UserController : ControllerBase
    {
        private readonly AuthHelper AuthHelper;

        public UserController(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            AuthHelper = new AuthHelper(configuration, httpContextAccessor);
        }

        [HttpGet]
        public UserClaims GetUserClaims()
        {
            return AuthHelper.GetClaims();
        }
    }
}
