using HabilitadorGraduaciones.Web.Models.Common;
using System.Text;

namespace HabilitadorGraduaciones.Web.Common
{
    public class AuthHelper
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthHelper(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            this._configuration = configuration;
            this._httpContextAccessor = httpContextAccessor;
        }

        public UserClaims GetClaims()
        {
            UserClaims userClaims = new();

            try
            {
                if (_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
                {
                    var claims = _httpContextAccessor.HttpContext.User.Identities.First().Claims.ToList();

                    /* Con el siguiente código se recorre cada uno de los claims y se escriben en el Log */
                    StringBuilder message = new();
                    claims.ForEach(claim => { message.AppendFormat($"[ {claim.Type} - {claim.Value} ]", "\t"); });

                    userClaims.NombreCompleto = claims?.FirstOrDefault(x => x.Type.ToLower().Contains("fullName".ToLower()))?.Value;
                    userClaims.Matricula = claims?.FirstOrDefault(x => x.Type.ToLower().Contains("NAM_cn".ToLower()))?.Value;
                    userClaims.Correo = claims?.FirstOrDefault(x => x.Type.ToLower().Contains("NAM_upn".ToLower()))?.Value;
                    userClaims.Pidm = claims?.FirstOrDefault(x => x.Type.ToLower().Contains("ITESMProfPIDM".ToLower()))?.Value;
                    userClaims.Nomina = claims?.FirstOrDefault(x => x.Type.ToLower().Contains("NAM_SAMAccountName".ToLower()))?.Value;
                }
            }
            catch (Exception e)
            {
                throw new ArgumentNullException("Ocurrió un error en el método GetUserClaims() en la clase Authentication", e);
            }
            return userClaims;
        }

    }
}
