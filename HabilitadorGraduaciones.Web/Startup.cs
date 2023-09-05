using AutoMapper;
using HabilitadorGraduaciones.Core.Automapper;
using HabilitadorGraduaciones.Web.Extensions;
using HabilitadorGraduaciones.Web.Identity;
using HabilitadorGraduaciones.Web.Interfaces;
using HabilitadorGraduaciones.Web.Models.Common;
using ITfoxtec.Identity.Saml2.MvcCore.Configuration;
using ITfoxtec.Identity.Saml2.Schemas;
using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Logging;
using Microsoft.OpenApi.Models;
using System.Net;

namespace HabilitadorGraduaciones.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //#region Sesion
            services.AddDistributedMemoryCache();

            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(20);
            });

            services.AddHttpContextAccessor();
            services.AddControllersWithViews();

            services.AddCors(opt =>
            {
                opt.AddDefaultPolicy(builder =>
                {
                    builder.AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddConfigurations(Configuration).AddServices().AddFilters();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebAPIGraduaciones", Version = "v1" });
            });
            var mapperConfig = new MapperConfiguration(m =>
            {
                m.AddProfile(new MappingProfile());
            });

            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);
            services.AddMvc(c => c.Conventions.Add(new ApiExplorerIgnores()));

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            IdentityModelEventSource.ShowPII = true; // Esto muestra el detalle de las posibles fallas en la federación.

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebAPIGraduaciones v1"));
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            if (!env.IsDevelopment())
            {
                app.UseDefaultFiles();
                app.UseStaticFiles();
            }

            app.UseCors();

            app.UseRouting();
            app.UseSaml2();
            app.UseSession();
#if DEBUG
            //DO NOTHING
#else
                        app.MapWhen(
                            context =>
                            {
                                if (!context.User.Identity.IsAuthenticated && context.Request.Path.Value.StartsWith("/api/", StringComparison.OrdinalIgnoreCase))
                                {
                                    var logeado = context.Session.GetObjectFromJson<UserClaims>("SesionUsuario");
                                    if (logeado == null)
                                        return true;
                                    else
                                        return false;
                                }
                                else
                                    return false;
                            },
                            config =>
                            {
                                config.Run(async context =>
                                {
                                    context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                                    await Task.FromResult(string.Empty);
                                });
                            }
                        );
                      
#endif

            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
            });

            app.Use(async (context, next) =>
            {
                if (!context.User.Identity.IsAuthenticated)
                {
                    await context.ChallengeAsync(Saml2Constants.AuthenticationScheme);
                }
                else
                {
                    await next();
                }
            });
        }
    }
}