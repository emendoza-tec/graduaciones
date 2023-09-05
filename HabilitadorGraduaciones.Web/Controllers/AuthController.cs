using HabilitadorGraduaciones.Core.Claims;
using HabilitadorGraduaciones.Core.CustomException;
using HabilitadorGraduaciones.Web.Common;
using HabilitadorGraduaciones.Web.Identity;
using HabilitadorGraduaciones.Web.Models.Common;
using ITfoxtec.Identity.Saml2;
using ITfoxtec.Identity.Saml2.MvcCore;
using ITfoxtec.Identity.Saml2.Schemas;
using ITfoxtec.Identity.Saml2.Schemas.Metadata;
using ITfoxtec.Identity.Saml2.Util;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel.Security;

namespace HabilitadorGraduaciones.Web.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    [AllowAnonymous]
    //[EnableCors("CorsPolicy")]
    public class AuthController : ControllerBase
    {
        const string relayStateReturnUrl = "ReturnUrl";
        private readonly Saml2Configuration Saml2Config;
        private readonly IConfiguration Configuration;
        private readonly string SingleLogoutDestination;
        private readonly string SingleLogoutDestinationReturn;

        public AuthController(IOptions<Saml2Configuration> saml2Config, IConfiguration configuration)
        {
            Saml2Config = saml2Config.Value;
            Configuration = configuration;
            SingleLogoutDestination = configuration.GetSection("Saml2:SingleLogoutDestination").Get<string>();
            SingleLogoutDestinationReturn = configuration.GetSection("Saml2:SingleLogoutDestinationReturn").Get<string>();
        }

        public IActionResult Login(string returnUrl = null)
        {
            var binding = new Saml2RedirectBinding();
            binding.SetRelayStateQuery(new Dictionary<string, string> { { relayStateReturnUrl, returnUrl ?? Url.Content("~/") } });
            return binding.Bind(new Saml2AuthnRequest(Saml2Config)).ToActionResult();
        }

        public async Task<IActionResult> AssertionConsumerService()
        {
            try
            {
                var binding = new Saml2PostBinding();
                var saml2AuthnResponse = new Saml2AuthnResponse(Saml2Config);

                binding.ReadSamlResponse(Request.ToGenericHttpRequest(), saml2AuthnResponse);

                if (saml2AuthnResponse.Status != Saml2StatusCodes.Success)
                    throw new AuthenticationException($"SAML Response status: {saml2AuthnResponse.Status}");

                binding.Unbind(Request.ToGenericHttpRequest(), saml2AuthnResponse);
                await saml2AuthnResponse.CreateSession(HttpContext, claimsTransform: (claimsPrincipal) => ClaimsTransform.Transform(claimsPrincipal));

                var relayStateQuery = binding.GetRelayStateQuery();
                var returnUrl = relayStateQuery.ContainsKey(relayStateReturnUrl) ? relayStateQuery[relayStateReturnUrl] : Url.Content("~/");

                if (saml2AuthnResponse.Status == Saml2StatusCodes.Success)
                {
                    GetUsuarioSesion();
                }

                return Redirect(returnUrl);
            }
            catch (Exception e)
            {
                throw new CustomException("Ocurrió un error en AssertionConsumerService", e);
            }
        }

        public async Task<IActionResult> Logout()
        {
            try
            {
                var binding = new Saml2PostBinding();
                var saml2LogoutRequest = await new Saml2LogoutRequest(Saml2Config, User).DeleteSession(HttpContext);
                return binding.Bind(saml2LogoutRequest).ToActionResult();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw new CustomException("Ocurrió un error en Logout", e);
            }

            return Ok();
        }

        public IActionResult LoggedOut()
        {
            var binding = new Saml2PostBinding();
            binding.Unbind(Request.ToGenericHttpRequest(), new Saml2LogoutResponse(Saml2Config));
            return Redirect(Url.Content("~/"));
        }

        public async Task<IActionResult> SingleLogout()
        {
            Saml2StatusCodes status = Saml2StatusCodes.Success;
            var genericHttpRequest = Request.ToGenericHttpRequest();

            var requestBinding = new Saml2PostBinding();
            var logoutRequest = new Saml2LogoutRequest(Saml2Config, User);

            if (new Saml2PostBinding().IsResponse(genericHttpRequest) || new Saml2RedirectBinding().IsResponse(genericHttpRequest))
            {
                try
                {
                    // Flujo del Single Logout desde el Service Provider (SP). Lo dispara esta misma aplicación.
                    var saml2ConfigSP = GetNewSaml2Configuration();
                    // En este escenario es necesario que el Single Logout Destination sea: https://amfsdevl.tec.mx/nidp/saml2/slo
                    saml2ConfigSP.SingleLogoutDestination = new Uri(SingleLogoutDestination);


                    var logoutRequestSP = new Saml2LogoutRequest(saml2ConfigSP, User);
                    var requestBindingSP = new Saml2PostBinding();
                    requestBindingSP.Unbind(Request.ToGenericHttpRequest(), logoutRequestSP);
                    await logoutRequestSP.DeleteSession(HttpContext);

                    var responsebindingSP = new Saml2PostBinding
                    {
                        RelayState = requestBindingSP.RelayState
                    };
                    var saml2LogoutResponse = new Saml2LogoutResponse(saml2ConfigSP)
                    {
                        InResponseToAsString = logoutRequestSP.IdAsString,
                        Status = Saml2StatusCodes.Success
                    };

                    return requestBindingSP.Bind(saml2LogoutResponse).ToActionResult();

                }
                catch (Exception e)
                {
                    throw new CustomException("Ocurrió un error en SingleLogout", e);
                }
            }
            else
            {
                try
                {
                    // Flujo del Single Logout desde el Identity Provider (IdP). Lo dispara NAM posterior al Logout desde otra aplicación.
                    var saml2ConfigIdP = GetNewSaml2Configuration();
                    // En este escenario es necesario que el Single Logout Destination sea: https://amfsdevl.tec.mx/nidp/saml2/slo_return
                    saml2ConfigIdP.SingleLogoutDestination = new Uri(SingleLogoutDestinationReturn);


                    var logoutRequestIdP = new Saml2LogoutRequest(saml2ConfigIdP, User);
                    var requestBindingIdP = new Saml2PostBinding();
                    requestBindingIdP.Unbind(Request.ToGenericHttpRequest(), logoutRequestIdP);
                    await logoutRequestIdP.DeleteSession(HttpContext);

                    var responsebindingSP = new Saml2PostBinding
                    {
                        RelayState = requestBindingIdP.RelayState
                    };
                    var saml2LogoutResponseIdP = new Saml2LogoutResponse(saml2ConfigIdP)
                    {
                        InResponseToAsString = logoutRequestIdP.IdAsString,
                        Status = Saml2StatusCodes.Success
                    };

                    return requestBindingIdP.Bind(saml2LogoutResponseIdP).ToActionResult();
                }
                catch (Exception e)
                {
                    throw new CustomException("Ocurrió un error en SingleLogout", e);
                }
            }
        }

        private Saml2Configuration GetNewSaml2Configuration()
        {
            try
            {
                var saml2Configuration = new Saml2Configuration
                {
                    Issuer = Configuration["Saml2:Issuer"],
                    SingleSignOnDestination = new Uri(Configuration["Saml2:SingleSignOnDestination"]),
                    SingleLogoutDestination = new Uri(Configuration["Saml2:SingleLogoutDestination"]),
                    SignatureAlgorithm = Configuration["Saml2:SignatureAlgorithm"],
                    SignAuthnRequest = Convert.ToBoolean(Configuration["Saml2:SignAuthnRequest"]),
                    SigningCertificate = CertificateUtil.Load(CommonHelper.GetCertificateAbsolutePath(Configuration["Saml2:SigningCertificateFile"]),
                                                                                                                Configuration["Saml2:SigningCertificateCode"],
                                                                                                                X509KeyStorageFlags.Exportable | X509KeyStorageFlags.MachineKeySet |
                                                                                                                X509KeyStorageFlags.PersistKeySet),
                    CertificateValidationMode = (X509CertificateValidationMode)Enum.Parse(typeof(X509CertificateValidationMode), Configuration["Saml2:CertificateValidationMode"]),
                    RevocationMode = (X509RevocationMode)Enum.Parse(typeof(X509RevocationMode), Configuration["Saml2:RevocationMode"])
                };
                saml2Configuration.AllowedAudienceUris.Add(saml2Configuration.Issuer);
                var entityDescriptor = new EntityDescriptor();
                entityDescriptor.ReadIdPSsoDescriptorFromUrl(new Uri(Configuration["Saml2:IdPMetadata"]));

                if (entityDescriptor.IdPSsoDescriptor != null)
                {
                    saml2Configuration.SingleSignOnDestination = entityDescriptor.IdPSsoDescriptor.SingleSignOnServices.First().Location;
                    saml2Configuration.SingleLogoutDestination = entityDescriptor.IdPSsoDescriptor.SingleLogoutServices.First().Location;
                    saml2Configuration.SignatureValidationCertificates.AddRange(entityDescriptor.IdPSsoDescriptor.SigningCertificates);
                }

                return saml2Configuration;
            }
            catch (Exception e)
            {
                throw new CustomException("Ocurrió un error en el método GetNewSaml2Configuration() en la clase Authentication", e);
            }
        }

        private UserClaims GetUsuarioSesion()
        {
            UserClaims logeado = new UserClaims() { };

            try
            {
                logeado = HttpContext.Session.GetObjectFromJson<UserClaims>("SesionUsuario");
#if DEBUG

                if (logeado == null)
                {
                    logeado = new UserClaims();
                    logeado.Matricula = "A00828764";
                    logeado.NombreCompleto = "Carlos Bernadac";
                    logeado.Correo = "t-juabernadec@tec.mx";
                    logeado.Pidm = "4606826";
                    logeado.Nomina = "A00828764";

                    HttpContext.Session.SetObjectAsJson("SesionUsuario", logeado);
                }
#else

                logeado = new UserClaims();
                    logeado.Matricula = User.Claims.FirstOrDefault(propiedad => propiedad.Type == TipoClaims.Matricula())?.Value;
                    logeado.NombreCompleto = User.Claims.FirstOrDefault(propiedad => propiedad.Type == TipoClaims.NombreCompleto())?.Value;
                    logeado.Correo = User.Claims.FirstOrDefault(propiedad => propiedad.Type == TipoClaims.Correo())?.Value;
                    logeado.Pidm = User.Claims.FirstOrDefault(propiedad => propiedad.Type == TipoClaims.Pidm())?.Value;
                    logeado.Nomina = User.Claims.FirstOrDefault(propiedad => propiedad.Type == TipoClaims.Nomina())?.Value;
                    HttpContext.Session.SetObjectAsJson("SesionUsuario", logeado);
#endif

            }
            catch (Exception e)
            {
                throw new CustomException("Ocurrió un error en el método GetUsuarioSesion() en la clase Authentication", e);
            }

            return logeado;
        }

    }
}