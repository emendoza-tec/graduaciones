using HabilitadorGraduaciones.Core.DTO;
using HabilitadorGraduaciones.Core.DTO.Base;
using HabilitadorGraduaciones.Data.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace HabilitadorGraduaciones.Data
{
    public class LogData : ILogRepository
    {
        public const string ConnectionStrings = "ConnectionStrings:DefaultConnection";
        public IConfiguration Configuration { get; }

        public LogData(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public async Task<BaseOutDto> GuardarLog(LogEnteradoDto data)
        {
            BaseOutDto insert = new BaseOutDto();
            try
            {
                IList<Parameter> _params = new List<Parameter>
                {
                    DataBase.CreateParameter("@Matricula", DbType.String, 50, ParameterDirection.Input, true, null, DataRowVersion.Default, data.Matricula ),
                    DataBase.CreateParameter("@Periodo", DbType.String, 250, ParameterDirection.Input, true, null, DataRowVersion.Default, data.Periodo ),
                    DataBase.CreateParameter("@PeriodoId", DbType.String, 10, ParameterDirection.Input, true, null, DataRowVersion.Default, data.PeriodoId ),
                 };
                await DataBase.InsertOut("spLogEnteradoCreditosInsuficientes_Insertar", CommandType.StoredProcedure, _params, Configuration[ConnectionStrings]);
                insert.Result = true;
            }
            catch (Exception ex)
            {
                insert.Result = false;
                insert.ErrorMessage = ex.Message;
            }
            return insert;
        }
    }
}
