using HabilitadorGraduaciones.Core.Entities;
using HabilitadorGraduaciones.Data.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace HabilitadorGraduaciones.Data
{
    public class CustomExceptionData : ICustomExceptionRepository
    {
        private readonly string _connectionString;

        public CustomExceptionData(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public void GuardarExcepcion(BitacoraLog data)
        {
            IList<Parameter> list = new List<Parameter>
             {
                DataBase.CreateParameter("@pErrorControlado", DbType.Boolean, 1, ParameterDirection.Output, false, null, DataRowVersion.Default, data.ErrorControlado ),
                DataBase.CreateParameter("@pMensajeUsuario", DbType.AnsiString, 500, ParameterDirection.Output, false, null, DataRowVersion.Default, data.MensajeUsuario ),
                DataBase.CreateParameter("@pMensajeExcepcion", DbType.Int32,-1, ParameterDirection.Output, false, null, DataRowVersion.Default, data.MensajeExcepcion),
                DataBase.CreateParameter("@pStackTrace", DbType.AnsiString, -1, ParameterDirection.Input, false, null, DataRowVersion.Default, data.StackTrace),
                DataBase.CreateParameter("@pInnerException", DbType.AnsiString, -1, ParameterDirection.Input, false, null, DataRowVersion.Default, data.InnerException),
                DataBase.CreateParameter("@pHttpStatusCode", DbType.AnsiString, 50, ParameterDirection.Input, false, null, DataRowVersion.Default,data.HttpStatusCode),
                DataBase.CreateParameter("@pUsuarioAlta", DbType.AnsiString, 9, ParameterDirection.Input, false, null, DataRowVersion.Default,data.UsuarioAlta)
             };

            DataBase.Insert("spLogExcepciones_InsertaExcepcion", CommandType.StoredProcedure, list, _connectionString);

        }
    }
}
