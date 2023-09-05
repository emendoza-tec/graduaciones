using HabilitadorGraduaciones.Core.CustomException.Filters;
using HabilitadorGraduaciones.Data;
using HabilitadorGraduaciones.Data.Interfaces;
using HabilitadorGraduaciones.Data.Utils;
using HabilitadorGraduaciones.Services;
using HabilitadorGraduaciones.Services.Interfaces;
using HabilitadorGraduaciones.Services.Utils;
using HabilitadorGraduaciones.Web.Interfaces;
using HabilitadorGraduaciones.Web.Utils;
using ITfoxtec.Identity.Saml2;
using ITfoxtec.Identity.Saml2.MvcCore.Configuration;
using ITfoxtec.Identity.Saml2.Schemas.Metadata;
using ITfoxtec.Identity.Saml2.Util;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel.Security;

namespace HabilitadorGraduaciones.Web.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddConfigurations(this IServiceCollection services, IConfiguration Configuration)
        {
            services.Configure<Saml2Configuration>(Configuration.GetSection("Saml2"));
            services.Configure<Saml2Configuration>(saml2Configuration =>
            {
                try
                {
                    saml2Configuration.Issuer = Configuration["Saml2:Issuer"];
                    saml2Configuration.SingleSignOnDestination = new Uri(Configuration["Saml2:SingleSignOnDestination"]);
                    saml2Configuration.SingleLogoutDestination = new Uri(Configuration["Saml2:SingleLogoutDestination"]);
                    saml2Configuration.SignatureAlgorithm = Configuration["Saml2:SignatureAlgorithm"];
                    saml2Configuration.SignAuthnRequest = Convert.ToBoolean(Configuration["Saml2:SignAuthnRequest"]);
                    saml2Configuration.SigningCertificate = CertificateUtil.Load(Configuration["Saml2:SigningCertificateFile"], Configuration["Saml2:SigningCertificateCode"],
                                                                                                                                X509KeyStorageFlags.Exportable | X509KeyStorageFlags.MachineKeySet |
                                                                                                                                X509KeyStorageFlags.PersistKeySet);
                    saml2Configuration.CertificateValidationMode = (X509CertificateValidationMode)Enum.Parse(typeof(X509CertificateValidationMode), Configuration["Saml2:CertificateValidationMode"]);
                    saml2Configuration.RevocationMode = (X509RevocationMode)Enum.Parse(typeof(X509RevocationMode), Configuration["Saml2:RevocationMode"]);
                    saml2Configuration.AllowedAudienceUris.Add(saml2Configuration.Issuer);
                    var entityDescriptor = new EntityDescriptor();
                    entityDescriptor.ReadIdPSsoDescriptorFromUrl(new Uri(Configuration["Saml2:IdPMetadata"]));
                    if (entityDescriptor.IdPSsoDescriptor != null)
                    {
                        saml2Configuration.SingleSignOnDestination = entityDescriptor.IdPSsoDescriptor.SingleSignOnServices.First().Location;
                        saml2Configuration.SingleLogoutDestination = entityDescriptor.IdPSsoDescriptor.SingleLogoutServices.First().Location;
                        saml2Configuration.SignatureValidationCertificates.AddRange(entityDescriptor.IdPSsoDescriptor.SigningCertificates);
                    }
                    else
                        throw new Exception("No se cargó el IdPSsoDescriptor del metadata");
                }
                catch (Exception e)
                {
                    throw;
                }
            });
            services.AddSaml2(slidingExpiration: true);

            return services;
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddTransient<IArchivoStorage, ArchivoLocalStorage>();
            services.AddTransient<IAccesosNominaService, AccesosNominaService>();
            services.AddTransient<IAccesosNominaRepository, AccesosNominaData>();
            services.AddTransient<ICalendariosService, CalendariosService>();
            services.AddTransient<ICalendariosRepository, CalendariosData>();
            services.AddTransient<ISesionRepository, SesionData>();
            services.AddTransient<IApiService, ApiService>();
            services.AddTransient<IExamenConocimientosService, ExamenConocimientosService>();
            services.AddTransient<IExamenConocimientosRepository, ExamenConocimientosData>();
            services.AddTransient<IPeriodosService, PeriodosService>();
            services.AddTransient<IPeriodosRepository, PeriodosData>();
            services.AddTransient<IRolesService, RolesService>();
            services.AddTransient<IRolesRepository, RolesData>();
            services.AddTransient<IUsuarioRepository, UsuarioData>();
            services.AddTransient<IUsuarioService, UsuarioService>();
            services.AddTransient<IAvisosService, AvisosService>();
            services.AddTransient<IAvisosRepository, AvisosData>();
            services.AddTransient<IExpedienteService, ExpedienteService>();
            services.AddTransient<IExpedienteRepository, ExpedienteData>();
            services.AddTransient<IPermisosNominaService, PermisosNominaService>();
            services.AddTransient<IPermisosNominaRepository, PermisosNominaData>();
            services.AddTransient<INivelInglesService, NivelInglesService>();
            services.AddTransient<INivelInglesRepository, NivelInglesData>();
            services.AddTransient<IReporteEstimadoService, ReporteEstimadoDeGraduacionService>();
            services.AddTransient<IReporteEstimadoRepository, ReporteEstimadoDeGraduacionData>();
            services.AddTransient<IReporteSabanaService, SabanaService>();
            services.AddTransient<IReporteSabanaRepository, SabanaData>();
            services.AddTransient<ISolicitudDeCambioDeDatosRepository, SolicitudDeCambioDeDatosData>();
            services.AddTransient<ISolicitudDeCambioDeDatosService, SolicitudDeCambioDeDatosService>();
            services.AddTransient<ISemanasTecService, SemanasTecService>();
            services.AddTransient<ISemanasTecRepository, SemanasTecData>();
            services.AddTransient<IDistincionesRepository, DistincionesData>();
            services.AddTransient<IDistincionesService, DistincionesService>();
            services.AddTransient<ILinksRepository, LinksData>();
            services.AddTransient<ILinksService, LinksService>();
            services.AddTransient<ICampusCeremoniaRepository, CampusCeremoniaGraduacionData>();
            services.AddTransient<ICampusCeremoniaGraduacionService, CampusCeremoniaGraduacionService>();
            services.AddTransient<INotificacionesRepository, NotificacionesData>();
            services.AddTransient<INotificacionesService, NotificacionesService>();
            services.AddTransient<ILogRepository, LogData>();
            services.AddTransient<ILogService, LogService>();
            services.AddTransient<IPrestamoEducativoRepository, PrestamoEducativoData>();
            services.AddTransient<IPrestamoEducativoService, PrestamoEducativoService>();
            services.AddTransient<IEmailModuleRepository, EmailModule>();
            services.AddTransient<ITarjetaRepository, TarjetaData>();
            services.AddTransient<ITarjetaService, TarjetaService>();
            services.AddTransient<IPlanDeEstudiosRepository, PlanDeEstudiosData>();
            services.AddTransient<IPlanDeEstudiosService, PlanDeEstudiosService>();
            services.AddTransient<IProgramaBgbRepository, ProgramaBgbData>();
            services.AddTransient<IProgramaBgbService, ProgramaBgbService>();
            services.AddTransient<IExamenIntegradorRepository, ExamenIntegradorData>();
            services.AddTransient<IExamenIntegradorService, ExamenIntegradorService>();
            services.AddTransient<IServicioSocialRepository, ServicioSocialData>();
            services.AddTransient<IServicioSocialService, ServicioSocialService>();
            services.AddTransient<IArchivoLocalStorageService, ArchivoLocalStorageService>();
            services.AddTransient<IAlmacenadorAzureStorageService, AlmacenadorAzureStorageService>();


            return services;
        }

        public static IServiceCollection AddFilters(this IServiceCollection services)
        {

            services.AddControllers(options =>
            {
                options.Filters.Add<GlobalExceptionFilterAttribute>();
            })
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNamingPolicy = null;
                options.JsonSerializerOptions.DictionaryKeyPolicy = null;
            })
            .AddNewtonsoftJson(options =>
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            );

            return services;
        }
    }
}
