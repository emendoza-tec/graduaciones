using HabilitadorGraduaciones.Core.CustomException;
using HabilitadorGraduaciones.Core.DTO;
using HabilitadorGraduaciones.Data.Interfaces;
using HabilitadorGraduaciones.Data.Utils;
using HabilitadorGraduaciones.Data.Utils.Enums;
using HabilitadorGraduaciones.Data.Utils.Extensions;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace HabilitadorGraduaciones.Data
{
    public class ServicioSocialData : IServicioSocialRepository
    {
        public IConfiguration configuration { get; }
        public const string ConnectionStrings = "ConnectionStrings:DefaultConnection";

        public ServicioSocialData(IConfiguration _configuration)
        {
            configuration = _configuration;
        }

        public async Task<ServicioSocialDto> ConsultarHorasServicioSocial(EndpointsDto alumno)
        {
            ServicioSocialDto result = new ServicioSocialDto();
            int HorasAcreditadas = 0;
            int HorasRequisito = 0;
            result.Result = false;
            try
            {
                IList<Parameter> list = new List<Parameter>
                {
                    DataBase.CreateParameter("@Matricula", DbType.String, 9, ParameterDirection.Input, false, null, DataRowVersion.Default, alumno.NumeroMatricula)
                };

                using (IDataReader reader = await DataBase.GetReader("spServicioSocial_ObtenerHoras", CommandType.StoredProcedure, list, configuration[ConnectionStrings]))
                {
                    while (reader.Read())
                    {
                        HorasAcreditadas = ComprobarNulos.CheckIntNull(reader["HorasAcumuladas"]);
                        HorasRequisito = ComprobarNulos.CheckIntNull(configuration["ServicioSocial:HorasRequeridasParaAcreditar"]);
                    }
                }

                var listaHoras = new List<HorasDto>();
                result.ClaveIdentidad = alumno.NumeroMatricula;
                result.Carrera = alumno.ClaveCarrera;
                result.UltimaActualizacionSS = DateTime.UtcNow;

                result.Carrera = result.Carrera.ToUpper();

                if (result.Carrera == Carreras.Medicina.GetString() || result.Carrera == Carreras.Odontologia.GetString())
                {
                    listaHoras.Add(new HorasDto { HoraAcreditada = "Has cursado 1 año de tu servicio social clínico" });
                    result.Lista_Horas = listaHoras;
                    result.isCumpleSS = true;
                    result.isServicioSocial = false;
                }
                else if (HorasAcreditadas >= HorasRequisito)
                {
                    listaHoras.Add(new HorasDto { HoraAcreditada = "Horas acreditadas ", ValorAcreditada = HorasAcreditadas, HoraRequisito = "Horas requisito", ValorRequisito = HorasRequisito });
                    result.Lista_Horas = listaHoras;
                    result.isCumpleSS = true;
                    result.isServicioSocial = true;
                }
                else
                {
                    listaHoras.Add(new HorasDto { HoraAcreditada = "Horas acreditadas ", ValorAcreditada = HorasAcreditadas, HoraRequisito = "Horas requisito", ValorRequisito = HorasRequisito });
                    result.Lista_Horas = listaHoras;
                    result.isCumpleSS = false;
                    result.isServicioSocial = true;
                }
                result.Result = true;
            }
            catch (Exception ex) 
            {
                throw new CustomException("ConsultarHorasServicioSocial", ex);
            }

            return result;
        }
    }
}
