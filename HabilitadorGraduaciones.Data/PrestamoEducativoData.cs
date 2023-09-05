using HabilitadorGraduaciones.Core.DTO;
using HabilitadorGraduaciones.Core.Entities;
using HabilitadorGraduaciones.Data.Interfaces;
using HabilitadorGraduaciones.Data.Utils;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace HabilitadorGraduaciones.Data
{
    public class PrestamoEducativoData : IPrestamoEducativoRepository
    {
        private readonly string _connectionString;

        public PrestamoEducativoData(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<PrestamoEducativoDto> GetPrestamoEducativo(PrestamoEducativoEntity entity)
        {
            PrestamoEducativoDto result = new PrestamoEducativoDto();
            result.Result = false;
            IList<Parameter> list = new List<Parameter>
            {
                DataBase.CreateParameter("@MATRICULA", DbType.String, 9, ParameterDirection.Input, false, null, DataRowVersion.Default, entity.Matricula)
            };

            using (IDataReader reader = await DataBase.GetReader("spTramitesAdmin_ObtenerPrestamo", CommandType.StoredProcedure, list, _connectionString))
            {
                while (reader.Read())
                {
                    result.EstatusContrato = ComprobarNulos.CheckNull<string>(reader["ESTATUS_CONTRATO"]);
                }

                result.Result = true;

                if (!string.IsNullOrEmpty(entity.EstatusContrato))
                    result.TienePrestamo = true;
            }

            return result;
        }
    }
}
