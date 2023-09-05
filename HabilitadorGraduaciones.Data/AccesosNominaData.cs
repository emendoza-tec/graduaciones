using HabilitadorGraduaciones.Core.Entities;
using HabilitadorGraduaciones.Data.Interfaces;
using HabilitadorGraduaciones.Data.Utils;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace HabilitadorGraduaciones.Data
{
    public class AccesosNominaData : IAccesosNominaRepository
    {

        private readonly string connectionString;

        public AccesosNominaData(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<AccesosNominaEntity> GetAcceso(string matricula)
        {
            var result = new AccesosNominaEntity();
            IList<Parameter> list = new List<Parameter>
                {
                    DataBase.CreateParameter("@Matricula", DbType.String, 9, ParameterDirection.Input, false, null, DataRowVersion.Default, matricula)
                };

            using (IDataReader reader = await DataBase.GetReader("spAccesoNomina_ObtenerNomina", CommandType.StoredProcedure, list, connectionString))
            {
                while (reader.Read())
                {
                    result.Ambiente = ComprobarNulos.CheckStringNull(reader["Ambiente"]);
                    result.Acceso = ComprobarNulos.CheckBooleanNull(reader["Acceso"]);
                }
            }
            return result;
        }
        public async Task<AccesosNominaEntity> GetAccesoUsuarioAdmin(string nomina)
        {
            var result = new AccesosNominaEntity();
            IList<Parameter> list = new List<Parameter>
            {
                DataBase.CreateParameter("@Nomina", DbType.String, 9, ParameterDirection.Input, false, null, DataRowVersion.Default, nomina)
            };

            using (IDataReader reader = await DataBase.GetReader("spAccesoNomina_ObtenerUsuarioAdministrador", CommandType.StoredProcedure, list, connectionString))
            {
                while (reader.Read())
                {
                    result.Ambiente = ComprobarNulos.CheckStringNull(reader["Ambiente"]);
                    result.Acceso = ComprobarNulos.CheckBooleanNull(reader["Acceso"]);
                }
            }
            return result;
        }
    }
}
