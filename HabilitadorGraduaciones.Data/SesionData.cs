using HabilitadorGraduaciones.Core.CustomException;
using HabilitadorGraduaciones.Core.Token;
using HabilitadorGraduaciones.Data.Interfaces;
using HabilitadorGraduaciones.Data.Utils;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace HabilitadorGraduaciones.Data
{
    public class SesionData : ISesionRepository
    {
        private readonly IConfiguration _configuration =
            new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

        public async Task<Sesion> GetSesion(string matricula)
        {
            var sesion = new Sesion();
            try
            {
                IList<Parameter> list = new List<Parameter>
                {
                    DataBase.CreateParameter("@pMatricula", DbType.String, 9, ParameterDirection.Input, false, null, DataRowVersion.Default, matricula),
                };

                using (IDataReader reader = await DataBase.GetReader("spSesiones_ObtenerSesion", CommandType.StoredProcedure, list, _configuration.GetConnectionString("DefaultConnection")))
                {
                    while (reader.Read())
                    {
                        sesion.Matricula = ComprobarNulos.CheckStringNull(reader["Matricula"]);
                        sesion.OAuthToken = ComprobarNulos.CheckStringNull(reader["OAuthToken"]);
                        sesion.JwtToken = ComprobarNulos.CheckStringNull(reader["JwtToken"]);
                        sesion.FechaCreacion = ComprobarNulos.CheckDateTimeNull(reader["FechaCreacion"]);
                        sesion.FechaExpiracion = ComprobarNulos.CheckDateTimeNull(reader["FechaExpiracion"]);
                    }
                }
                return sesion;
            }
            catch (Exception ex)
            {
                throw new CustomException("Error en el Método GetSesion", ex);
            }    
        }

        public async Task GuardaSesion(Sesion sesion)
        {
            try
            {
                IList<Parameter> list = new List<Parameter>
                {
                    DataBase.CreateParameter("@pMatricula", DbType.AnsiString, 9, ParameterDirection.Input, false, null, DataRowVersion.Default, sesion.Matricula),
                    DataBase.CreateParameter("@pOAuth", DbType.AnsiString, 500, ParameterDirection.Input, false, null, DataRowVersion.Default, sesion.OAuthToken),
                    DataBase.CreateParameter("@pJwt",  DbType.AnsiString, 500, ParameterDirection.Input, false, null, DataRowVersion.Default, sesion.JwtToken),
                    DataBase.CreateParameter("@pFechaCreacion", DbType.DateTime, 8, ParameterDirection.Input, false, null, DataRowVersion.Default, sesion.FechaCreacion),
                    DataBase.CreateParameter("@pFechaExpiracion", DbType.DateTime, 8, ParameterDirection.Input, false, null, DataRowVersion.Default, sesion.FechaExpiracion)
                };

                await DataBase.Insert("spSesiones_Insertar", CommandType.StoredProcedure, list, _configuration.GetConnectionString("DefaultConnection"));
            }
            catch (Exception ex)
            {
                throw new CustomException("Error en el Método GuardaSesion", ex);
            }
            
        }

        public async Task ModificaSesion(Sesion sesion)
        {
            try
            {
                IList<Parameter> list = new List<Parameter>
            {
                DataBase.CreateParameter("@pMatricula", DbType.AnsiString, 9, ParameterDirection.Input, false, null, DataRowVersion.Default, sesion.Matricula),
                DataBase.CreateParameter("@pOAuth", DbType.AnsiString, 500, ParameterDirection.Input, false, null, DataRowVersion.Default, sesion.OAuthToken),
                DataBase.CreateParameter("@pJwt",  DbType.AnsiString, 500, ParameterDirection.Input, false, null, DataRowVersion.Default, sesion.JwtToken),
                DataBase.CreateParameter("@pFechaCreacion", DbType.DateTime, 8, ParameterDirection.Input, false, null, DataRowVersion.Default, sesion.FechaCreacion),
                DataBase.CreateParameter("@pFechaExpiracion", DbType.DateTime, 8, ParameterDirection.Input, false, null, DataRowVersion.Default, sesion.FechaExpiracion)
            };

                await DataBase.Update("spSesiones_ActualizarTokens", CommandType.StoredProcedure, list, _configuration.GetConnectionString("DefaultConnection"));
            }
            catch (Exception ex)
            {
                throw new CustomException("Error en el Método ModificaSesion", ex);
            }
            
        }
    }
}
