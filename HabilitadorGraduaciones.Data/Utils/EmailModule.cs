using HabilitadorGraduaciones.Core.DTO.Base;
using HabilitadorGraduaciones.Data.Interfaces;
using HabilitadorGraduaciones.Data.Utils.Enums;
using Microsoft.Extensions.Configuration;
using System.Net.Mail;

namespace HabilitadorGraduaciones.Data.Utils
{
    public class EmailModule : IEmailModuleRepository
    {
        public IConfiguration _configuration { get; }
        private readonly string _txtremitente;
        private readonly string _smtp;

        public EmailModule(IConfiguration configuration)
        {
            _configuration = configuration;
            _txtremitente = configuration["EmailServerTec:RTE_Correo"];
            _smtp = configuration["EmailServerTec:SMTP_Server"];
        }

        public async Task<BaseOutDto> EnviarCorreo(string destinatario, string asunto, string cuerpo, string adjuntos, string cc, int tipo = 0)
        {
            BaseOutDto result = new BaseOutDto();
            string nombreRemitente = string.Empty;
            char[] delimitador_cc = { ',' };
            int port = 25;

            try
            {
                SmtpClient cliente = new(_smtp, port);
                MailMessage correo = new()
                {
                    From = new MailAddress(_txtremitente, nombreRemitente),
                    Subject = asunto,
                    Body = cuerpo,
                    IsBodyHtml = true
                };
                correo.To.Add(destinatario);
                if (cc != "")
                {
                    string[] cadena1 = cc.Split(delimitador_cc);
                    foreach (string word in cadena1) correo.CC.Add(word.Trim());
                }

                string imgEncabezado = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot", "PlantillaCorreos/encabezado-correo.png");
                string imgPiePagina = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot", "PlantillaCorreos/pie-correo.png");
                string imgCheck = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot", "PlantillaCorreos/check.png");

                switch (tipo)
                {
                    case (int)TipoCorreo.Avisos:
                        nombreRemitente = "Avisos Graduaciones";

                        imgEncabezado = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot", "PlantillaAvisos/correo-avisos-encabezado.png");
                        imgPiePagina = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot", "PlantillaAvisos/correo-avisos-pie.png");
                        AlternateView body = AlternateView.CreateAlternateViewFromString(correo.Body, null, "text/html");
                        LinkedResource lrImageEncabezado = new(imgEncabezado, "image/jpg")
                        {
                            ContentId = "imgEncabezado"
                        };
                        body.LinkedResources.Add(lrImageEncabezado);
                        LinkedResource lrImagePie = new(imgPiePagina, "image/jpg")
                        {
                            ContentId = "imgPie"
                        };
                        body.LinkedResources.Add(lrImagePie);
                        if (!string.IsNullOrEmpty(adjuntos))
                        {
                            string pathImg = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot", adjuntos);
                            LinkedResource lrImage = new(pathImg, "image/jpg")
                            {
                                ContentId = "imgAdjunta"
                            };
                            body.LinkedResources.Add(lrImage);
                        }
                        correo.AlternateViews.Add(body);
                        break;
                    case (int)TipoCorreo.SolicitudCambiosHab:
                        nombreRemitente = "Habilitador Graduaciones";

                        body = AlternateView.CreateAlternateViewFromString(correo.Body, null, "text/html");
                        lrImageEncabezado = new(imgEncabezado, "image/jpg")
                        {
                            ContentId = "imgEncabezado"
                        };
                        body.LinkedResources.Add(lrImageEncabezado);
                        lrImagePie = new(imgPiePagina, "image/jpg")
                        {
                            ContentId = "imgPie"
                        };
                        body.LinkedResources.Add(lrImagePie);
                        LinkedResource check = new(imgCheck, "image/jpg")
                        {
                            ContentId = "imgCheck"
                        };
                        body.LinkedResources.Add(check);
                        correo.AlternateViews.Add(body);
                        break;
                    case (int)TipoCorreo.ConfirmacionDatos:
                        nombreRemitente = "Habilitador Graduaciones";

                        imgCheck = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot", "PlantillaCorreos/check.png");
                        string imgAlerta = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot", "PlantillaCorreos/alerta.png");
                        string imgUsuario = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot", "PlantillaCorreos/usuario.png");
                        body = AlternateView.CreateAlternateViewFromString(correo.Body, null, "text/html");
                        lrImageEncabezado = new(imgEncabezado, "image/jpg")
                        {
                            ContentId = "imgEncabezado"
                        };
                        body.LinkedResources.Add(lrImageEncabezado);
                        lrImagePie = new(imgPiePagina, "image/jpg")
                        {
                            ContentId = "imgPie"
                        };
                        body.LinkedResources.Add(lrImagePie);
                        check = new(imgCheck, "image/jpg")
                        {
                            ContentId = "imgCheck"
                        };
                        body.LinkedResources.Add(check);
                        LinkedResource alerta = new(imgAlerta, "image/jpg")
                        {
                            ContentId = "alerta"
                        };
                        body.LinkedResources.Add(alerta);
                        LinkedResource usuarioImg = new(imgUsuario, "image/jpg")
                        {
                            ContentId = "usuario"
                        };
                        body.LinkedResources.Add(usuarioImg);
                        correo.AlternateViews.Add(body);
                        break;
                    default:
                        nombreRemitente = "Habilitador Graduaciones";
                        if (!string.IsNullOrEmpty(adjuntos.Trim()))
                        {
                            string[] cadena2 = adjuntos.Split(",");
                            foreach (string word in cadena2) correo.Attachments.Add(new Attachment(word));
                        }
                        break;
                }

                await cliente.SendMailAsync(correo);
            }
            catch (Exception ex)
            {
                result.Result = false;
                result.ErrorMessage += "Correo:" + destinatario + " Error: " + ex.Message + " InnerE:" + ex.InnerException;
            }

            return result;
        }
    }
}