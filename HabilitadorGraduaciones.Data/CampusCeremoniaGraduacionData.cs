using HabilitadorGraduaciones.Core.DTO.Base;
using HabilitadorGraduaciones.Core.Entities;
using HabilitadorGraduaciones.Data.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace HabilitadorGraduaciones.Data
{
    public class CampusCeremoniaGraduacionData : ICampusCeremoniaRepository
    {
        public const string ConnectionStrings = "ConnectionStrings:DefaultConnection";
        public IConfiguration Configuration { get; }

        public CampusCeremoniaGraduacionData(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public async Task<BaseOutDto> GuardaCeremonia(CampusCeremoniaGraduacionEntity ceremonia)
        {
            BaseOutDto result = new BaseOutDto();
            try
            {
                IList<Parameter> list = new List<Parameter>
               {
                    DataBase.CreateParameter("@pClaveCampus", DbType.AnsiString, 3, ParameterDirection.Input, false, null, DataRowVersion.Default, ceremonia.ClaveCampus),
                    DataBase.CreateParameter("@pMatricula", DbType.AnsiString, 9, ParameterDirection.Input, false, null, DataRowVersion.Default, ceremonia.Matricula),
                    DataBase.CreateParameter("@pPeriodoGraduacion", DbType.AnsiString, 6, ParameterDirection.Input, false, null, DataRowVersion.Default, ceremonia.PeriodoGraduacion)
               };


                await DataBase.Insert("spCampusCeremoniaGraduacion_Insertar", CommandType.StoredProcedure, list, Configuration[ConnectionStrings]);
                result.Result = true;
            }
            catch (Exception ex)
            {
                result.ErrorMessage = ex.Message;
                result.Result = false;
            }

            return result;
            
        }
    }
}
