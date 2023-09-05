using HabilitadorGraduaciones.Core.CustomException;
using HabilitadorGraduaciones.Core.DTO;
using HabilitadorGraduaciones.Core.DTO.Base;
using HabilitadorGraduaciones.Core.Entities;
using HabilitadorGraduaciones.Data.Interfaces;
using HabilitadorGraduaciones.Data.Utils;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Net;

namespace HabilitadorGraduaciones.Data
{
    public class CalendariosData : ICalendariosRepository
    {
        private readonly IConfiguration _configuration =
           new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

        public async Task<CalendarioDto> GetCalendarioAlumno(CalendarioEntity entity)
        {
            CalendarioDto calendario = new CalendarioDto();

            IList<Parameter> list = new List<Parameter>
            {
                DataBase.CreateParameter("@MATRICULA", DbType.String, 9, ParameterDirection.Input, false, null, DataRowVersion.Default, entity.Matricula)
            };

            using (IDataReader reader = await DataBase.GetReader("spCalendarios_ObtenerCalendarioAlumno", CommandType.StoredProcedure, list, _configuration.GetConnectionString("DefaultConnection")))
            {
                while (reader.Read())
                {
                    calendario.CalendarioId = ComprobarNulos.CheckNull<string>(reader["CalendarioId"]);
                    calendario.ClaveCampus = ComprobarNulos.CheckNull<string>(reader["ClaveCampus"]);
                    calendario.Campus = ComprobarNulos.CheckNull<string>(reader["Campus"]);
                    calendario.LinkProspecto = ComprobarNulos.CheckNull<string>(reader["LinkProspecto"]);
                    calendario.LinkCandidato = ComprobarNulos.CheckNull<string>(reader["LinkCandidato"]);
                    calendario.Result = true;
                }
            }

            return calendario;
        }

        public async Task<CalendariosDto> GetCalendarios(CalendariosDto entity)
        {
            CalendariosDto result = new CalendariosDto();
            List<CalendarioDto> lstCalendarios = new List<CalendarioDto>();
            using (IDataReader reader = await DataBase.GetReader("spCalendarios_ObtenerCalendarios", CommandType.StoredProcedure, _configuration.GetConnectionString("DefaultConnection")))
            {
                while (reader.Read())
                {
                    CalendarioDto calendario = new CalendarioDto();
                    calendario.CalendarioId = ComprobarNulos.CheckNull<string>(reader["CalendarioId"]);
                    calendario.ClaveCampus = ComprobarNulos.CheckNull<string>(reader["ClaveCampus"]);
                    calendario.Campus = ComprobarNulos.CheckNull<string>(reader["Campus"]);
                    calendario.LinkProspecto = ComprobarNulos.CheckNull<string>(reader["LinkProspecto"]);
                    calendario.LinkCandidato = ComprobarNulos.CheckNull<string>(reader["LinkCandidato"]);
                    lstCalendarios.Add(calendario);
                }
            }
            result.Result = true;
            result.Calendarios = lstCalendarios;
            return result;
        }

        public async Task<BaseOutDto> ModificarCalendarios(List<CalendariosEntity> guardarCalendarios)
        {
            BaseOutDto update = new BaseOutDto();
            try
            {
                foreach (var guardarCalendario in guardarCalendarios)
                {
                    IList<Parameter> list = new List<Parameter>
                    {
                      DataBase.CreateParameter("@CLAVE_CAMPUS", DbType.AnsiString, 3, ParameterDirection.Input, false, null, DataRowVersion.Default, guardarCalendario.ClaveCampus ),
                      DataBase.CreateParameter("@LINK_PROSPECTO", DbType.AnsiString, -1, ParameterDirection.Input, false, null, DataRowVersion.Default, guardarCalendario.LinkProspecto ),
                      DataBase.CreateParameter("@LINK_CANDIDATO", DbType.AnsiString, -1, ParameterDirection.Input, false, null, DataRowVersion.Default, guardarCalendario.LinkCandidato),
                      DataBase.CreateParameter("@ID_USUARIO", DbType.AnsiString, 50, ParameterDirection.Input, false, null, DataRowVersion.Default, guardarCalendario.IdUsuario),
                    };
                
                    await DataBase.InsertOut("spCalendarios_InsertarCalendario", CommandType.StoredProcedure, list, _configuration.GetConnectionString("DefaultConnection"));

                }
                update.Result = true;
            }
            catch (Exception ex)
            {
                throw new CustomException("Ocurrio un error en el metodo ModificarCalendarios", ex);
            }            
            return update;
        }
    }
}
