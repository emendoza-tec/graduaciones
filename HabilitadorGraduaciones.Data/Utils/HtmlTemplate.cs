using HabilitadorGraduaciones.Core.Token;
using System.Data;
using System.Diagnostics;
using System.Numerics;
using System.Text;
using static HabilitadorGraduaciones.Data.Utils.Apis;
using static System.Net.Mime.MediaTypeNames;
using static System.Net.WebRequestMethods;

namespace HabilitadorGraduaciones.Data.Utils
{
    public static class HtmlTemplate
    {
        public static StringBuilder GetTemplateAvisos()
        {
            StringBuilder _stemplate = new();
            string template;
            template =
                @"<!DOCTYPE html>
                <html lang='en'>

                <head>
                    <meta http - equiv='Content-Type' content='text/html charset=UTF-8' />
                </head>

                <body style='font-family: Helvetica Neue, Helvetica, Arial, sans-serif'>
                    <!--[if mso]>
                        <div id='email' style='width: 800px; margin: auto; background:white; border:1px #0000002d solid; background: #FFFFFF; box-shadow: 0px 3px 10px rgba(0, 0, 0, 0.25); border-radius: 10px;'>
                    <![endif]-- >
                    <!--[if !mso]> <!---->
                    <div id='email' style='width: 800px; margin: auto; background:white; border:1px #0000002d solid; background: #FFFFFF; box-shadow: 0px 3px 10px rgba(0, 0, 0, 0.25); border-radius: 10px;'>
                        <!--<![endif]-->
                        <table role='presentation' width='100%' cellspacing='0'>
                            <tr>
                                <td align='center'>
                                    <img width='100%' src='cid:imgEncabezado' alt=''>
                                </td>
                            </tr>
                        </table>
                        <table role='presentation' width='100%' cellspacing='0'>
                            <tr>
                                <td>
                                    <h2 style='font-size:26px; padding-left: 30px;'>¡Hola, <span style='color: #3E44E7;'>Usuario</span>!</h2>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <h3 style='margin-top: 0; padding-left: 30px;'> No te pierdas los nuevos avisos que Graduación tiene para tí</h3>
                                </td>
                            </tr>
                        </table>
                        <table role='presentation' width='100%' cellspacing='0'>
                            <tr>
                                <td style='width: 10%;'></td>
                                <!--[if mso]>
                                    <td style = 'border:1px gray solid; background: #FFFFFF; box-shadow: 0px 3px 10px rgba(0, 0, 0, 0.25); border-radius: 10px;'>
                                <![endif]-- >
                                <!--[if !mso]> <!---->
                                <td style='border:1px #0000002d solid; background: #FFFFFF; box-shadow: 0px 3px 10px rgba(0, 0, 0, 0.25); border-radius: 10px;'>
                                <!--<![endif]-->
                                    <table cellpadding='10' role='presentation' width='100%' cellspacing='0'>
                                        <tr>
                                            <td width='80%' style='padding-bottom: 0;'>
                                                <h2 style='margin-bottom:0'>Firma de título</h2>
                                            </td>
                                            <td width='20%' style='padding-bottom: 0;'>
                                                <h4 style='text-align: right; color:#707070'>10 / 11 / 2022</h4>
                                            </td>
                                        </tr>
                                    </table>
                                    <table cellpadding='10' role='presentation' width='100%' cellspacing='0'>
                                        <tr>
                                            <td>
                                                <h3 style='font-weight: 100;'>Recuerda que tu firma de título será el día 24 de junio en Aula Magna a las 11:00 a.m. ¡Te esperamos!</h3>
                                            </td>
                                        </tr>
                                    </table>
                                    <table cellpadding='10' role='presentation' width='100%' cellspacing='0'>
                                        <tr>
                                            <td width='20%'></td>
                                            <td width='60%' style='text-align: center;'>
                                                <img width='85%' style='max-width: 404px; max-height: 274px;' src='cid:imgAdjunta'
                                                    alt=''>
                                            </td>
                                            <td width='20%'></td>
                                        </tr>
                                    </table>
                                </td>
                                <td style='width: 10%;'></td>
                            </tr>
                        </table>
                        <br>
                        <table role='presentation' width='100%' cellspacing='0'>
                            <tr>
                                <td style='width: 10%;'></td>
                                <td style='text-align: right;'>
                                    <div><!--[if mso]>
                                    <v:roundrect xmlns:v='urn:schemas-microsoft-com:vml' xmlns:w='urn:schemas-microsoft-com:office:word' href='HomePage' style='height:40px;v-text-anchor:middle;width:150px;' arcsize='20%' stroke='f' fillcolor='#3E44E7'>
                                      <w:anchorlock/>
                                      <center>
                                    <![endif]-->
                                        <a href='HomePage' style='background-color:#3E44E7;border-radius:8px;color:#ffffff;display:inline-block;font-family:sans-serif;font-size:13px;font-weight:bold;line-height:40px;text-align:center;text-decoration:none;width:150px;-webkit-text-size-adjust:none;'>Ir a Graduación</a>
                                    <!--[if mso]>
                                      </center>
                                    </v:roundrect>
                                  <![endif]--></div>
                                </td>
                                <td style='width: 10%;'></td>
                            </tr>
                        </table>
                        <br>
                        <table role='presentation' width='100%' cellspacing='0'>
                            <tr>
                                <td align='center'>
                                    <img width='100%' src='cid:imgPie' alt=''>
                                    <!-- <h1 style = 'font-size: 52px; margin:0 0 20px 0;' > Graduación</h1> -->
                                </td>
                            </tr>
                        </table>
                    </div>
                </body>
                </html>";
            _stemplate.Append(template);
            return _stemplate;
        }
        public static StringBuilder GetTemplateCambiosSolicitadosHabilitador()
        {
            StringBuilder _stemplate = new();
            string template;
            template =
                @"<!DOCTYPE html>
                <html lang='en'>

                <head>
                    <meta http - equiv='Content-Type' content='text/html charset=UTF-8' />
                </head>

                <body style='font-family: Helvetica Neue, Helvetica, Arial, sans-serif'>
                    <!--[if mso]><div id='email' style='width: 800px; margin: auto; background:white; border:1px #0000002d solid; background: #FFFFFF; box-shadow: 0px 3px 10px rgba(0, 0, 0, 0.25); border-radius: 10px;'>
                                                                    <![endif]-- >
                                                                    <!--[if !mso]> <!---->
                    <div id='email'
                        style='width: 800px; margin: auto; background:white; border:1px #0000002d solid; background: #FFFFFF; box-shadow: 0px 3px 10px rgba(0, 0, 0, 0.25); border-radius: 10px;'>
                        <!--<![endif]-->
                        <table role='presentation' width='100%' cellspacing='0'>
                            <tr>
                                <td align='center'>
                                    <img width='100%' src='cid:imgEncabezado' alt=''>
                                </td>
                            </tr>
                        </table>
                        <table role='presentation' width='100%' cellspacing='0'>
                            <tr>
                                <td style='width: 10%;'></td>
                                <td style='width: 80%;'>
                                    <h2 style='font-size:26px; text-align: center; font-weight: 500;'>Hola
                                        #Usuario#,
                                    </h2>
                                </td>
                                <td style='width: 10%;'></td>
                            </tr>
                            <tr>
                                <td style='width: 10%;'></td>
                                <td style='width: 80%;'>
                                    <h3 style='margin-top: 0; text-align: center; font-weight: 500;'> Te confirmamos
                                        que se ha realizado la siguiente actividad
                                        de tu proceso de graduación.</h3>
                                </td>
                                <td style='width: 10%;'></td>
                            </tr>
                        </table>
                        <table role='presentation' width='100%' cellspacing='0'>
                            <tr>
                                <td style='width: 10%;'></td>
                                <td style='width: 80%; text-align: center;'>
                                    <h2 style='font-size: 30px;'><img width='25' height='25' src='cid:imgCheck' alt=''
                                            style='vertical-align: middle; padding: 5px;'>Gracias por tu solicitud</h2>
                                </td>
                                <td style='width: 10%;'></td>
                            </tr>
                            <tr>
                                <td style='width: 10%;'></td>
                                <td style='width: 80%; text-align: center;'>
                                    <p style='margin:0;'>Se ha registrado con éxito la solicitud de ajustes en tus datos.</p>
                                    <p style='margin-top: 0;'>Revisaremos esta información junto con la documentación adjunta, así
                                        como la que ya tenemos en el sistema y la normativa vigente.</p>
                                </td>
                                <td style='width: 10%;'></td>
                            </tr>
                            <tr>
                                <td style='width: 10%;'></td>
                                <td style='width: 80%; text-align: left;'>
                                    <h3 style='margin-bottom: 5px;'>Cambio(s) solicitado(s):</h3>
                                </td>
                                <td style='width: 10%;'></td>
                            </tr>
                        </table>
                        <table role='presentation' width='100%' cellspacing='0'>
                            <tr>
                                <td style='width: 10%;'></td>
                                <!--[if mso]>
                                                                                    <td style = 'border:1px gray solid; background: #FFFFFF; box-shadow: 0px 3px 10px rgba(0, 0, 0, 0.25); border-radius: 10px;'>
                                                                                <![endif]-- >
                                                                                <!--[if !mso]> <!---->
                                <td
                                    style='border:1px #0000002d solid; background: #FFFFFF; box-shadow: 0px 3px 10px rgba(0, 0, 0, 0.25); border-radius: 10px;'>
                                    <!--<![endif]-->
                                    <table cellpadding='10' role='presentation' width='100%' cellspacing='0'>
                                        <tr>
                                            <td width='100%' style='padding-bottom: 0;'>
                                                <p
                                                    style='margin-bottom:0; margin-left: 10px; font-weight: 700; text-align: left; font-size: 17px; color: #1864F2;'>
                                                    Información actual:</p>
                                            </td>
                                        </tr>
                                        #Actual#
                                    </table>
                                    <table cellpadding='10' role='presentation' width='100%' cellspacing='0'>
                                        <tr>
                                            <td width='100%' style='padding-bottom: 0;'>
                                                <p
                                                    style='margin-bottom:0; margin-left: 10px; font-weight: 700; text-align: left; font-size: 17px; color: #1864F2;'>
                                                    Cambio(s) solicitado(s):</p>
                                            </td>
                                        </tr>
                                        #Cambio#
                                    </table>
                                </td>
                                <td style='width: 10%;'></td>
                            </tr>
                        </table>
                        <br>
                        <table role='presentation' width='100%' cellspacing='0'>
                            <tr>
                                <td style='width: 10%;'></td>
                                <td style='width: 80%; text-align: center;'>
                                    <h3 style='font-weight: 100;'> No te pierdas lo que Graduación tiene para tí.
                                        <!--[if mso]>
                                                            <v:roundrect xmlns:v='urn:schemas-microsoft-com:vml' xmlns:w='urn:schemas-microsoft-com:office:word' href='HomePage' style='height:40px;v-text-anchor:middle;width:150px;' arcsize='20%' stroke='f' fillcolor='#3E44E7'>
                                                                <w:anchorlock/>
                                                                <center>
                                                            <![endif]-->
                                        <a href='HomePage'
                                            style='background-color:#3E44E7;border-radius:8px;color:#ffffff;display:inline-block;font-family:sans-serif;font-size:13px;font-weight:bold;line-height:40px;text-align:center;text-decoration:none;width:150px;-webkit-text-size-adjust:none;'>Ir a Graduación</a>
                                        <!--[if mso]>
                                                                </center>
                                                            </v:roundrect>
                                                            <![endif]-->
                                    </h3>
                                </td>
                                <td style='width: 10%;'></td>
                            </tr>
                        </table>
                        <br>
                        <table role='presentation' width='100%' cellspacing='0'>
                            <tr>
                                <td align='center'>
                                    <img width='100%' src='cid:imgPie' alt=''>
                                </td>
                            </tr>
                        </table>
                    </div>
                </body>

                </html>";
            _stemplate.Append(template);
            return _stemplate;
        }
        public static StringBuilder GetTemplateColaboradorAprobado()
        {
            StringBuilder _stemplate = new();
            string template;
            template =
                @"<!DOCTYPE html>
                <html lang='en'>

                <head>
                    <meta http - equiv='Content-Type' content='text/html charset=UTF-8' />
                </head>

                <body style='font-family: Helvetica Neue, Helvetica, Arial, sans-serif'>
                    <!--[if mso]>
                                                                        <div id='email' style='width: 800px; margin: auto; background:white; border:1px #0000002d solid; background: #FFFFFF; box-shadow: 0px 3px 10px rgba(0, 0, 0, 0.25); border-radius: 10px;'>
                                                                    <![endif]-- >
                                                                    <!--[if !mso]> <!---->
                    <div id='email'
                        style='width: 800px; margin: auto; background:white; border:1px #0000002d solid; background: #FFFFFF; box-shadow: 0px 3px 10px rgba(0, 0, 0, 0.25); border-radius: 10px;'>
                        <!--<![endif]-->
                        <table role='presentation' width='100%' cellspacing='0'>
                            <tr>
                                <td align='center'>
                                    <img width='100%' src='cid:imgEncabezado' alt=''>
                                </td>
                            </tr>
                        </table>
                        <table role='presentation' width='100%' cellspacing='0'>
                            <tr>
                                <td style='width: 10%;'></td>
                                <td style='width: 80%;'>
                                    <h2
                                        style='font-size:26px; text-align: center; font-weight: 500;'>
                                        Hola #Usuario#,
                                    </h2>
                                </td>
                                <td style='width: 10%;'></td>
                            </tr>
                            <tr>
                                <td style='width: 10%;'></td>
                                <td style='width: 80%;'>
                                    <h3
                                        style='margin-top: 0; text-align: center; font-weight: 500;'>
                                        Te confirmamos que se ha realizado la siguiente actividad de tu proceso de graduación.</h3>
                                </td>
                                <td style='width: 10%;'></td>
                            </tr>
                        </table>
                        <table role='presentation' width='100%' cellspacing='0'>
                            <tr>
                                <td style='width: 10%;'></td>
                                <td style='width: 80%; text-align: left;'>
                                    <h3 style='margin-bottom: 5px;'>Cambio(s) solicitado(s):</h3>
                                </td>
                                <td style='width: 10%;'></td>
                            </tr>
                        </table>
                        <table role='presentation' width='100%' cellspacing='0'>
                            <tr>
                                <td style='width: 10%;'></td>
                                <!--[if mso]>
                                                                                    <td style = 'border:1px gray solid; background: #FFFFFF; box-shadow: 0px 3px 10px rgba(0, 0, 0, 0.25); border-radius: 10px;'>
                                                                                <![endif]-- >
                                                                                <!--[if !mso]> <!---->
                                <td
                                    style='border:1px #0000002d solid; background: #FFFFFF; box-shadow: 0px 3px 10px rgba(0, 0, 0, 0.25); border-radius: 10px;'>
                                    <!--<![endif]-->
                                    <table cellpadding='10' role='presentation' width='100%' cellspacing='0'>
                                        <tr>
                                            <td width='100%' style='padding-bottom: 0;'>
                                                <p
                                                    style='margin-bottom:0; margin-left: 10px; font-weight: 700; text-align: left; font-size: 17px; color: #1864F2;'>
                                                    Información actual:</p>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width='100%' style='padding-bottom: 0;'>
                                                <p style='margin: 0 0 0 10px; font-weight: 700; text-align: left; font-size: 17px;'>
                                                    #Tipo#:</p>
                                                <p style='margin: 0 0 0 10px; text-align: left; font-size: 17px;'>#Actual#</p>
                                            </td>
                                        </tr>
                                    </table>
                                    <table cellpadding='10' role='presentation' width='100%' cellspacing='0'>
                                        <tr>
                                            <td width='100%' style='padding-bottom: 0;'>
                                                <p
                                                    style='margin-bottom:0; margin-left: 10px; font-weight: 700; text-align: left; font-size: 17px; color: #1864F2;'>
                                                    Cambio(s) solicitado(s):</p>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width='100%' style='padding-bottom: 0;'>
                                                <p style='margin: 0 0 0 10px; font-weight: 700; text-align: left; font-size: 17px;'>
                                                    #Tipo#:</p>
                                                <p style='margin: 0 0 0 10px; text-align: left; font-size: 17px;'>#Cambio#</p>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td style='width: 10%;'></td>
                            </tr>
                        </table>
                        <br>

                        <table role='presentation' width='100%' cellspacing='0'>
                            <tr>
                                <td style='width: 10%;'></td>
                                <td style='width: 80%;'>
                                    <h3
                                        style='margin-top: 0; text-align: center; font-weight: 500;'>
                                        Esta solicitud ha concluido con el estatus de <FONT COLOR='#61BD6D' style='font-weight: 700;'>
                                            aprobado</FONT> y se agregan los siguientes comentarios:</h3>
                                </td>
                                <td style='width: 10%;'></td>
                            </tr>
                            <tr>
                                <td style='width: 10%;'></td>
                                <td style='width: 80%;'>
                                    <h3
                                        style='margin-top: 0; text-align: center; font-weight: 500;'>
                                        #Comentario#</h3>
                                </td>
                                <td style='width: 10%;'></td>
                            </tr>
                        </table>

                        <table role='presentation' width='100%' cellspacing='0'>
                            <tr>
                                <td style='width: 10%;'></td>
                                <td style='width: 80%; text-align: center;'>
                                    <h3 style='font-weight: 100;'> No te pierdas lo que Graduación tiene para tí.
                                        <!--[if mso]>
                                            <v:roundrect xmlns:v='urn:schemas-microsoft-com:vml' xmlns:w='urn:schemas-microsoft-com:office:word' href='HomePage' style='height:40px;v-text-anchor:middle;width:150px;' arcsize='20%' stroke='f' fillcolor='#3E44E7'>
                                                <w:anchorlock/>
                                                <center>
                                            <![endif]-->
                                        <a href='HomePage'
                                            style='background-color:#3E44E7;border-radius:8px;color:#ffffff;display:inline-block;font-family:sans-serif;font-size:13px;font-weight:bold;line-height:40px;text-align:center;text-decoration:none;width:150px;-webkit-text-size-adjust:none;'>Ir a Graduación</a>
                                        <!--[if mso]>
                                                </center>
                                            </v:roundrect>
                                            <![endif]-->
                                    </h3>
                                </td>
                                <td style='width: 10%;'></td>
                            </tr>
                        </table>
                        <br>
                        <table role='presentation' width='100%' cellspacing='0'>
                            <tr>
                                <td align='center'>
                                    <img width='100%' src='cid:imgPie' alt=''>
                                    <!-- <h1 style = 'font-size: 52px; margin:0 0 20px 0;' > Graduación</h1> -->
                                </td>
                            </tr>
                        </table>
                    </div>
                </body>

                </html>";
            _stemplate.Append(template);
            return _stemplate;
        }
        public static StringBuilder GetTemplateColaboradorRevision()
        {
            StringBuilder _stemplate = new();
            string template;
            template =
                @"<!DOCTYPE html>
                <html lang='en'>

                <head>
                    <meta http - equiv='Content-Type' content='text/html charset=UTF-8' />
                </head>

                <body style='font-family: Helvetica Neue, Helvetica, Arial, sans-serif'>
                    <!--[if mso]>
                                                                                                        <div id='email' style='width: 800px; margin: auto; background:white; border:1px #0000002d solid; background: #FFFFFF; box-shadow: 0px 3px 10px rgba(0, 0, 0, 0.25); border-radius: 10px;'>
                                                                                                    <![endif]-- >
                                                                                                    <!--[if !mso]> <!---->
                    <div id='email'
                        style='width: 800px; margin: auto; background:white; border:1px #0000002d solid; background: #FFFFFF; box-shadow: 0px 3px 10px rgba(0, 0, 0, 0.25); border-radius: 10px;'>
                        <!--<![endif]-->
                        <table role='presentation' width='100%' cellspacing='0'>
                            <tr>
                                <td align='center'>
                                    <img width='100%' src='cid:imgEncabezado' alt=''>
                                </td>
                            </tr>
                        </table>
                        <table role='presentation' width='100%' cellspacing='0'>
                            <tr>
                                <td style='width: 10%;'></td>
                                <td style='width: 80%;'>
                                    <h2
                                        style='font-size:26px; text-align: center; font-weight: 500;'>
                                        Hola #Usuario#,
                                    </h2>
                                </td>
                                <td style='width: 10%;'></td>
                            </tr>
                            <tr>
                                <td style='width: 10%;'></td>
                                <td style='width: 80%;'>
                                    <h3
                                        style='margin-top: 0; text-align: center; font-weight: 500;'>
                                        Te confirmamos que se ha realizado la siguiente actividad
                                        de tu proceso de graduación.</h3>
                                </td>
                                <td style='width: 10%;'></td>
                            </tr>
                        </table>
                        <table role='presentation' width='100%' cellspacing='0'>
                            <tr>
                                <td style='width: 10%;'></td>
                                <td style='width: 80%; text-align: left;'>
                                    <h3 style='margin-bottom: 5px;'>Cambio(s) solicitado(s):</h3>
                                </td>
                                <td style='width: 10%;'></td>
                            </tr>
                        </table>
                        <table role='presentation' width='100%' cellspacing='0'>
                            <tr>
                                <td style='width: 10%;'></td>
                                <!--[if mso]>
                                                                                                    <td style = 'border:1px gray solid; background: #FFFFFF; box-shadow: 0px 3px 10px rgba(0, 0, 0, 0.25); border-radius: 10px;'>
                                                                                                <![endif]-- >
                                                                                                <!--[if !mso]> <!---->
                                <td
                                    style='border:1px #0000002d solid; background: #FFFFFF; box-shadow: 0px 3px 10px rgba(0, 0, 0, 0.25); border-radius: 10px;'>
                                    <!--<![endif]-->
                                    <table cellpadding='10' role='presentation' width='100%' cellspacing='0'>
                                        <tr>
                                            <td width='100%' style='padding-bottom: 0;'>
                                                <p
                                                    style='margin-bottom:0; margin-left: 10px; font-weight: 700; text-align: left; font-size: 17px; color: #1864F2;'>
                                                    Información actual:</p>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width='100%' style='padding-bottom: 0;'>
                                                <p style='margin: 0 0 0 10px; font-weight: 700; text-align: left; font-size: 17px;'>
                                                    #Tipo#:</p>
                                                <p style='margin: 0 0 0 10px; text-align: left; font-size: 17px;'>#Actual#</p>
                                            </td>
                                        </tr>
                                    </table>
                                    <table cellpadding='10' role='presentation' width='100%' cellspacing='0'>
                                        <tr>
                                            <td width='100%' style='padding-bottom: 0;'>
                                                <p
                                                    style='margin-bottom:0; margin-left: 10px; font-weight: 700; text-align: left; font-size: 17px; color: #1864F2;'>
                                                    Cambio(s) solicitado(s):</p>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width='100%' style='padding-bottom: 0;'>
                                                <p style='margin: 0 0 0 10px; font-weight: 700; text-align: left; font-size: 17px;'>
                                                    #Tipo#:</p>
                                                <p style='margin: 0 0 0 10px; text-align: left; font-size: 17px;'>#Cambio#</p>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td style='width: 10%;'></td>
                            </tr>
                        </table>
                        <br>

                        <table role='presentation' width='100%' cellspacing='0'>
                            <tr>
                                <td style='width: 10%;'></td>
                                <td style='width: 80%;'>
                                    <h3
                                        style='margin-top: 0; text-align: center; font-weight: 500;'>
                                        Esta solicitud ha concluido con el estatus de <FONT style='font-weight: 700;' COLOR='#D9AD26'>en
                                            revisión</FONT> y se
                                        agregan los siguientes comentarios:</h3>
                                </td>
                                <td style='width: 10%;'></td>
                            </tr>
                            <tr>
                                <td style='width: 10%;'></td>
                                <td style='width: 80%;'>
                                    <h3
                                        style='margin-top: 0; text-align: center; font-weight: 500;'>
                                        #Comentario#</h3>
                                </td>
                                <td style='width: 10%;'></td>
                            </tr>
                            <tr>
                                <td style='width: 10%;'></td>
                                <td style='width: 80%;'>
                                    <h3
                                        style='margin-top: 0; text-align: center; font-weight: 500;'>
                                        Cualquier duda adicional favor de ponerte en contacto a la siguiente cuenta de correo
                                        electrónico:</h3>
                                </td>
                                <td style='width: 10%;'></td>
                            </tr>
                            <tr>
                                <td style='width: 10%;'></td>
                                <td style='width: 80%;'>
                                    <h3
                                        style='margin-top: 0; text-align: center; font-weight: 500;'>
                                        <a href='mailto:graduaciones@servicio.tec.mx'>graduaciones@servicio.tec.mx</a>
                                    </h3>
                                </td>
                                <td style='width: 10%;'></td>
                            </tr>
                        </table>

                        <table role='presentation' width='100%' cellspacing='0'>
                            <tr>
                                <td style='width: 10%;'></td>
                                <td style='width: 80%; text-align: center;'>
                                    <h3 style='font-weight: 100; '> No te pierdas lo que Graduación tiene para tí.
                                        <!--[if mso]>
                                            <v:roundrect xmlns:v='urn:schemas-microsoft-com:vml' xmlns:w='urn:schemas-microsoft-com:office:word' href='HomePage' style='height:40px;v-text-anchor:middle;width:150px;' arcsize='20%' stroke='f' fillcolor='#3E44E7'>
                                                <w:anchorlock/>
                                                <center>
                                            <![endif]-->
                                        <a href='HomePage'
                                            style='background-color:#3E44E7;border-radius:8px;color:#ffffff;display:inline-block;font-family:sans-serif;font-size:13px;font-weight:bold;line-height:40px;text-align:center;text-decoration:none;width:150px;-webkit-text-size-adjust:none;'>Ir a Graduación</a>
                                        <!--[if mso]>
                                                </center>
                                            </v:roundrect>
                                            <![endif]-->
                                    </h3>
                                </td>
                                <td style='width: 10%;'></td>
                            </tr>
                        </table>
                        <br>
                        <table role='presentation' width='100%' cellspacing='0'>
                            <tr>
                                <td align='center'>
                                    <img width='100%' src='cid:imgPie' alt=''>
                                    <!-- <h1 style = 'font-size: 52px; margin:0 0 20px 0;' > Graduación</h1> -->
                                </td>
                            </tr>
                        </table>
                    </div>
                </body>

                </html>";
            _stemplate.Append(template);
            return _stemplate;
        }
        public static StringBuilder GetTemplateColaboradorRechazada()
        {
            StringBuilder _stemplate = new();
            string template;
            template =
               @"<!DOCTYPE html>
                <html lang='en'>

                <head>
                    <meta http - equiv='Content-Type' content='text/html charset=UTF-8' />
                </head>

                <body style='font-family: Helvetica Neue, Helvetica, Arial, sans-serif'>
                    <!--[if mso]>
                                                                                  <div id='email' style='width: 800px; margin: auto; background:white; border:1px #0000002d solid; background: #FFFFFF; box-shadow: 0px 3px 10px rgba(0, 0, 0, 0.25); border-radius: 10px;'>
                                                                              <![endif]-- >
                                                                              <!--[if !mso]> <!---->
                    <div id='email'
                        style='width: 800px; margin: auto; background:white; border:1px #0000002d solid; background: #FFFFFF; box-shadow: 0px 3px 10px rgba(0, 0, 0, 0.25); border-radius: 10px;'>
                        <!--<![endif]-->
                        <table role='presentation' width='100%' cellspacing='0'>
                            <tr>
                                <td align='center'>
                                    <img width='100%' src='cid:imgEncabezado' alt=''>
                                </td>
                            </tr>
                        </table>
                        <table role='presentation' width='100%' cellspacing='0'>
                            <tr>
                                <td style='width: 10%;'></td>
                                <td style='width: 80%;'>
                                    <h2
                                        style='font-size:26px; text-align: center; font-weight: 500;'>
                                        Hola
                                        #Usuario#,
                                    </h2>
                                </td>
                                <td style='width: 10%;'></td>
                            </tr>
                            <tr>
                                <td style='width: 10%;'></td>
                                <td style='width: 80%;'>
                                    <h3
                                        style='margin-top: 0; text-align: center; font-weight: 500;'>
                                        Te confirmamos
                                        que se ha realizado la siguiente actividad
                                        de tu proceso de graduación.</h3>
                                </td>
                                <td style='width: 10%;'></td>
                            </tr>
                        </table>
                        <table role='presentation' width='100%' cellspacing='0'>
                            <tr>
                                <td style='width: 10%;'></td>
                                <td style='width: 80%; text-align: left;'>
                                    <h3 style='margin-bottom: 5px;'>Cambio(s) solicitado(s):</h3>
                                </td>
                                <td style='width: 10%;'></td>
                            </tr>
                        </table>
                        <table role='presentation' width='100%' cellspacing='0'>
                            <tr>
                                <td style='width: 10%;'></td>
                                <!--[if mso]>
                                                                                              <td style = 'border:1px gray solid; background: #FFFFFF; box-shadow: 0px 3px 10px rgba(0, 0, 0, 0.25); border-radius: 10px;'>
                                                                                          <![endif]-- >
                                                                                          <!--[if !mso]> <!---->
                                <td
                                    style='border:1px #0000002d solid; background: #FFFFFF; box-shadow: 0px 3px 10px rgba(0, 0, 0, 0.25); border-radius: 10px;'>
                                    <!--<![endif]-->
                                    <table cellpadding='10' role='presentation' width='100%' cellspacing='0'>
                                        <tr>
                                            <td width='100%' style='padding-bottom: 0;'>
                                                <p
                                                    style='margin-bottom:0; margin-left: 10px; font-weight: 700; text-align: left; font-size: 17px; color: #1864F2;'>
                                                    Información actual:</p>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width='100%' style='padding-bottom: 0;'>
                                                <p style='margin: 0 0 0 10px; font-weight: 700; text-align: left; font-size: 17px;'>
                                                    #Tipo#:</p>
                                                <p style='margin: 0 0 0 10px; text-align: left; font-size: 17px;'>#Actual#</p>
                                            </td>
                                        </tr>
                                    </table>
                                    <table cellpadding='10' role='presentation' width='100%' cellspacing='0'>
                                        <tr>
                                            <td width='100%' style='padding-bottom: 0;'>
                                                <p
                                                    style='margin-bottom:0; margin-left: 10px; font-weight: 700; text-align: left; font-size: 17px; color: #1864F2;'>
                                                    Cambio(s) solicitado(s):</p>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width='100%' style='padding-bottom: 0;'>
                                                <p style='margin: 0 0 0 10px; font-weight: 700; text-align: left; font-size: 17px;'>
                                                    #Tipo#:</p>
                                                <p style='margin: 0 0 0 10px; text-align: left; font-size: 17px;'>#Cambio#</p>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td style='width: 10%;'></td>
                            </tr>
                        </table>
                        <br>

                        <table role='presentation' width='100%' cellspacing='0'>
                            <tr>
                                <td style='width: 10%;'></td>
                                <td style='width: 80%;'>
                                    <h3
                                        style='margin-top: 0; text-align: center; font-weight: 500;'>
                                        Esta solicitud ha concluido con el estatus de <FONT COLOR='red' style='font-weight: 700;'>
                                            rechazada</FONT> y se agregan los siguientes
                                        comentarios:</h3>
                                </td>
                                <td style='width: 10%;'></td>
                            </tr>
                            <tr>
                                <td style='width: 10%;'></td>
                                <td style='width: 80%;'>
                                    <h3
                                        style='margin-top: 0; text-align: center; font-weight: 500;'>
                                        #Comentario#</h3>
                                </td>
                                <td style='width: 10%;'></td>
                            </tr>
                            <tr>
                                <td style='width: 10%;'></td>
                                <td style='width: 80%;'>
                                    <h3
                                        style='margin-top: 0; text-align: center; font-weight: 500;'>
                                        Cualquier duda
                                        adicional favor de ponerte en contacto a la siguiente cuenta de correo electrónico:</h3>
                                </td>
                                <td style='width: 10%;'></td>
                            </tr>
                            <tr>
                                <td style='width: 10%;'></td>
                                <td style='width: 80%;'>
                                    <h3
                                        style='margin-top: 0; text-align: center; font-weight: 500;'>
                                        <a href='mailto:graduaciones@servicio.tec.mx'>graduaciones@servicio.tec.mx</a>
                                    </h3>
                                </td>
                                <td style='width: 10%;'></td>
                            </tr>
                        </table>

                        <table role='presentation' width='100%' cellspacing='0'>
                            <tr>
                                <td style='width: 10%;'></td>
                                <td style='width: 80%; text-align: center;'>
                                    <h3 style='font-weight: 100;'> No te pierdas lo que Graduación tiene para tí.
                                        <!--[if mso]>
                                            <v:roundrect xmlns:v='urn:schemas-microsoft-com:vml' xmlns:w='urn:schemas-microsoft-com:office:word' href='HomePage' style='height:40px;v-text-anchor:middle;width:150px;' arcsize='20%' stroke='f' fillcolor='#3E44E7'>
                                                <w:anchorlock/>
                                                <center>
                                            <![endif]-->
                                        <a href='HomePage'
                                            style='background-color:#3E44E7;border-radius:8px;color:#ffffff;display:inline-block;font-family:sans-serif;font-size:13px;font-weight:bold;line-height:40px;text-align:center;text-decoration:none;width:150px;-webkit-text-size-adjust:none;'>Ir a Graduación</a>
                                        <!--[if mso]>
                                                </center>
                                            </v:roundrect>
                                            <![endif]-->
                                    </h3>
                                </td>
                                <td style='width: 10%;'></td>
                            </tr>
                        </table>
                        <br>
                        <table role='presentation' width='100%' cellspacing='0'>
                            <tr>
                                <td align='center'>
                                    <img width='100%' src='cid:imgPie' alt=''>
                                    <!-- <h1 style = 'font-size: 52px; margin:0 0 20px 0;' > Graduación</h1> -->
                                </td>
                            </tr>
                        </table>
                    </div>
                </body>

                </html>";
            _stemplate.Append(template);
            return _stemplate;
        }
        public static StringBuilder GetTemplateConfirmacionPeriodoGraduacion()
        {
            StringBuilder _stemplatePG = new();
            string templatePG;
            templatePG =
                @"<!DOCTYPE html>
                <html lang='en'>

                <head>
                    <meta http - equiv='Content-Type' content='text/html charset=UTF-8' />
                </head>

                <body style='font-family: Helvetica Neue, Helvetica, Arial, sans-serif'>
                    <!--[if mso]>
                                    <div id='email' style='width: 800px; margin: auto; background:white; border:1px #0000002d solid; background: #FFFFFF; box-shadow: 0px 3px 10px rgba(0, 0, 0, 0.25); border-radius: 10px;'>
                                    <![endif]-- >
                                    <!--[if !mso]> <!---->
                    <div id='email'
                        style='width: 800px; margin: auto; background:white; border:1px #0000002d solid; background: #FFFFFF; box-shadow: 0px 3px 10px rgba(0, 0, 0, 0.25); border-radius: 10px;'>
                        <!--<![endif]-->
                        <table role='presentation' width='100%' cellspacing='0'>
                            <tr>
                                <td align='center'>
                                    <img width='100%' src='cid:imgEncabezado' alt=''>
                                </td>
                            </tr>
                        </table>
                        <table role='presentation' width='100%' cellspacing='0'>
                            <tr>
                                <td style='width: 10%;'></td>
                                <td style='width: 80%;'>
                                    <h2 style='font-size:26px; padding-left: 30px; text-align: center; font-weight: 500;'>Hola
                                        #Usuario#,</h2>
                                </td>
                                <td style='width: 10%;'></td>
                            </tr>
                            <tr>
                                <td style='width: 10%;'></td>
                                <td style='width: 80%;'>
                                    <h3 style='margin-top: 0; padding-left: 30px; text-align: center; font-weight: 500;'> Te confirmamos
                                        que se ha realizado la siguiente actividad de tu proceso de graduación.</h3>
                                </td>
                                <td style='width: 10%;'></td>
                            </tr>
                        </table>
                        <table role='presentation' width='80%' cellspacing='0'>
                            <tr>
                                <td style='width: 30%;'></td>
                                <!--[if mso]>
                                            <td style = 'border:1px gray solid; background: #FFFFFF; box-shadow: 0px 3px 10px rgba(0, 0, 0, 0.25); border-radius: 10px;'>
                                                <![endif]-- >
                                                <!--[if !mso]> <!---->
                                <td
                                    style='border:1px #0000002d solid; background: #FFFFFF; box-shadow: 0px 3px 10px rgba(0, 0, 0, 0.25); border-radius: 10px;'>
                                    <!--<![endif]-->
                                    <table cellpadding='10' role='presentation' width='100%' cellspacing='0'>
                                        <tr>
                                            <td style='width: 10%;'></td>
                                            <td style='width: 80%; text-align: center;'><img width='25' height='25' src='cid:imgCheck'
                                                    alt='' style='vertical-align: middle; padding: 1px;'></td>
                                            <td style='width: 10%;'></td>
                                        </tr>
                                        <tr>
                                            <td style='width: 10%;'></td>
                                            <td style='width: 80%; text-align: center;'>
                                                <h2 style='font-size: 18px;'>Se ha registrado de manera exitosa</h2>
                                            </td>
                                            <td style='width: 10%;'></td>
                                        </tr>
                                        <tr>
                                            <td style='width: 10%;'></td>
                                            <td style='width: 100%; text-align: center;'>
                                                <p style='margin-top: 0;'>Tu plan de graduarte en:</p>
                                                <p style='margin: 10px 50px 20px; padding: 7px; border:1px rgb(62, 68, 231) solid;  border-radius: 10px; font-size: 15px;
                                                   font-weight: bold; text-align: center;'><span>#Periodo#</span></p>
                                            </td>
                                            <td style='width: 10%;'></td>
                                        </tr>
                                        <tr>
                                            <td style='width: 10%;'></td>
                                        </tr>
                                    </table>
                                </td>
                                <td style='width: 10%;'></td>
                            </tr>
                        </table>
                        <br>
                        <table role='presentation' width='100%' cellspacing='0'>
                            <tr>
                                <td style='width: 10%;'></td>
                                <td style='width: 80%; text-align: center;'>
                                    <h3 style='font-weight: 100;'> No te pierdas lo que Graduación tiene para tí.
                                        <!--[if mso]>
                                            <v:roundrect xmlns:v='urn:schemas-microsoft-com:vml' xmlns:w='urn:schemas-microsoft-com:office:word' href='HomePage' style='height:40px;v-text-anchor:middle;width:150px;' arcsize='20%' stroke='f' fillcolor='#3E44E7'>
                                                <w:anchorlock/>
                                                <center>
                                            <![endif]-->
                                        <a href='HomePage'
                                            style='background-color:#3E44E7;border-radius:8px;color:#ffffff;display:inline-block;font-family:sans-serif;font-size:13px;font-weight:bold;line-height:40px;text-align:center;text-decoration:none;width:150px;-webkit-text-size-adjust:none;'>Ir a Graduación</a>
                                        <!--[if mso]>
                                                </center>
                                            </v:roundrect>
                                            <![endif]-->
                                    </h3>
                                </td>
                                <td style='width: 10%;'></td>
                            </tr>
                        </table>
                        <br>
                        <table role='presentation' width='100%' cellspacing='0'>
                            <tr>
                                <td align='center'>
                                    <img width='100%' src='cid:imgPie' alt=''>
                                    <!-- <h1 style = 'font-size: 52px; margin:0 0 20px 0;' > Graduación</h1> -->
                                </td>
                            </tr>
                        </table>
                    </div>
                </body>

                </html>";
            _stemplatePG.Append(templatePG);
            return _stemplatePG;
        }
        public static StringBuilder GetTemplateConfirmacionDatosPersonales()
        {
            StringBuilder _stemplateDP = new();
            string templateDP;
            templateDP =
                @"<!DOCTYPE html>
                <html lang='en'>

                <head>
                    <meta http - equiv='Content-Type' content='text/html charset=UTF-8' />
                </head>

                <body style='font-family: Helvetica Neue, Helvetica, Arial, sans-serif'>
                    <!--[if mso]>
                                                                                                        <div id='email' style='width: 800px; margin: auto; background:white; border:1px #0000002d solid; background: #FFFFFF; box-shadow: 0px 3px 10px rgba(0, 0, 0, 0.25); border-radius: 10px;'>
                                                                                                    <![endif]-- >
                                                                                                    <!--[if !mso]> <!---->
                    <div id='email'
                        style='width: 800px; margin: auto; background:white; border:1px #0000002d solid; background: #FFFFFF; box-shadow: 0px 3px 10px rgba(0, 0, 0, 0.25); border-radius: 10px;'>
                        <!--<![endif]-->
                        <table role='presentation' width='100%' cellspacing='0'>
                            <tr>
                                <td align='center'>
                                    <img width='100%' src='cid:imgEncabezado' alt=''>
                                </td>
                            </tr>
                        </table>
                        <table role='presentation' width='100%' cellspacing='0'>
                            <tr>
                                <td style='width: 10%;'></td>
                                <td style='width: 80%;'>
                                    <h2 style='font-size:26px; text-align: center; font-weight: 500;'>Hola
                                        #Usuario#,
                                    </h2>
                                </td>
                                <td style='width: 10%;'></td>
                            </tr>
                            <tr>
                                <td style='width: 10%;'></td>
                                <td style='width: 80%;'>
                                    <h3
                                        style='margin-top: 0; text-align: center; font-weight: 500;'>
                                        Te confirmamos
                                        que se ha realizado la siguiente actividad
                                        de tu proceso de graduación.</h3>
                                </td>
                                <td style='width: 10%;'></td>
                            </tr>
                        </table>
                        <table role='presentation' width='100%' cellspacing='0'>
                            <tr>
                                <td style='width: 15%;'></td>
                                <td style='width: 70%;'>
                                    <p style=' margin:0; text-align: justify;'><img
                                            style='max-width: 15px !important; width: 15px; height: auto;' src='cid:alerta' alt=''>Toda
                                        tu
                                        documentación oficial será
                                        emitida con tus datos personales validados
                                        y <b>NO</b>
                                        habrá posibilidad de hacer <b>cambios posteriores</b>, por ello te pedimos que la revises muy
                                        bien.
                                    </p>
                                </td>
                                <td style='width: 15%;'></td>
                            </tr>
                            <tr>
                                <td style='width: 15%;'></td>
                                <!--[if mso]>
                                    <td style = 'border:1px gray solid; background: #FFFFFF; box-shadow: 0px 3px 10px rgba(0, 0, 0, 0.25); border-radius: 10px;'>
                                <![endif]-- >
                                <!--[if !mso]> <!---->
                                <td
                                    style='border:1px #0000002d solid; background: #FFFFFF; box-shadow: 0px 3px 10px rgba(0, 0, 0, 0.25); border-radius: 10px;'>
                                    <!--<![endif]-->
                                    <table cellpadding='8' role='presentation' width='100%' cellspacing='0'>
                                        <tr>
                                            <td rowspan='9' style='vertical-align: top; width: 15%;'><img
                                                    style='margin-top: 15px; margin-left: 5px;' width='64px' src='cid:usuario' alt=''>
                                            </td>
                                            <td style='width: 85%;'>
                                                <span style='font-size: 16px;'><b>#Nombre#</b></span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style='width: 100%;'>
                                                <span><b>Programa acádemico actual:</b></span>
                                                <span> #Programa#</span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style='width: 100%;'>
                                                <span><b>CURP:</b></span><span>#Curp#</span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan='2' style='width: 100%;'>
                                                <hr class='line' style='margin: 10px'>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style='width: 100%;'>
                                                <span><b>Concentración(es) inscrita(s):</b></span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style='width: 100%;'>
                                                #Concentracion#
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style='width: 100%; margin-bottom: 0;'>
                                                <span><b>Diploma(s) adicionale(s) inscrito(s):</b></span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style='width: 100%;'>
                                                #Diploma#
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td style='width: 15%;'></td>
                            </tr>
                            <tr>
                                <td style='width: 15%;'></td>
                                <td style='width: 70%;'>
                                    <p style='text-align: justify;'>
                                        <img style='max-width: 15px !important; width: 15px; height: auto;' src='cid:alerta'
                                            alt=''>Recuerda que los cambios que
                                        hagas en tu nombre
                                        deberán
                                        estar <b>respaldados por tu acta
                                            de nacimiento o documento oficial que lo acredite</b> y tu solicitud estará sujeta a la
                                        <b>corroboración</b> con tu documentación oficial.
                                    </p>
                                </td>
                                <td style='width: 15%;'></td>
                            </tr>
                        </table>
                        <table>
                            <tr>
                                <td style='width: 10%;'></td>
                                <td style='width: 80%;'>
                                    <h3 style='font-weight: 100; text-align: center;'>Con este
                                        proceso ya eres <b>considerado candidato a graduar</b>, asegúrate de haber cumplido todos los
                                        requisitos
                                        de graduación al finalizar el periodo para poder graduarte/titularte en el periodo seleccionado
                                    </h3>
                                </td>
                                <td style='width: 10%;'></td>
                            </tr>
                        </table>
                        <table role='presentation' width='100%' cellspacing='0'>
                            <tr>
                                <td style='width: 10%;'></td>
                                <td style='width: 80%; text-align: center;'>
                                    <h3 style='font-weight: 100;'> No te pierdas lo que Graduación tiene para tí.
                                        <!--[if mso]>
                                            <v:roundrect xmlns:v='urn:schemas-microsoft-com:vml' xmlns:w='urn:schemas-microsoft-com:office:word' href='HomePage' style='height:40px;v-text-anchor:middle;width:150px;' arcsize='20%' stroke='f' fillcolor='#3E44E7'>
                                                <w:anchorlock/>
                                                <center>
                                            <![endif]-->
                                        <a href='HomePage'
                                            style='background-color:#3E44E7;border-radius:8px;color:#ffffff;display:inline-block;font-family:sans-serif;font-size:13px;font-weight:bold;line-height:40px;text-align:center;text-decoration:none;width:150px;-webkit-text-size-adjust:none;'>Ir a Graduación</a>
                                        <!--[if mso]>
                                                </center>
                                            </v:roundrect>
                                            <![endif]-->
                                    </h3>
                                </td>
                                <td style='width: 10%;'></td>
                            </tr>
                        </table>
                        <br>
                        <table role='presentation' width='100%' cellspacing='0'>
                            <tr>
                                <td align='center'>
                                    <img width='100%' src='cid:imgPie' alt=''>
                                    <!-- <h1 style = 'font-size: 52px; margin:0 0 20px 0;' > Graduación</h1> -->
                                </td>
                            </tr>
                        </table>
                    </div>
                </body>

                </html>";
            _stemplateDP.Append(templateDP);
            return _stemplateDP;
        }
    }
}
