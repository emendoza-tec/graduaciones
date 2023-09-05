using HabilitadorGraduaciones.Core.DTO;
using HabilitadorGraduaciones.Core.Entities;
using HabilitadorGraduaciones.Data.Interfaces;
using HabilitadorGraduaciones.Data.Utils;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace HabilitadorGraduaciones.Data
{
    public class ExamenIntegradorData : IExamenIntegradorRepository
    {
        private readonly string _connectionString;

        public ExamenIntegradorData(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        public async Task<List<ExamenIntegradorEntity>> GetExamenesIntegrador(int idUsuario)
        {
            var expedientes = new List<ExamenIntegradorEntity>();
            IList<Parameter> list = new List<Parameter>
            {
                DataBase.CreateParameter("@pIdUsuario", DbType.Int32, 10, ParameterDirection.Input, false, null, DataRowVersion.Default, idUsuario)
            };

            using (IDataReader reader = await DataBase.GetReader("spExamenIntegrador_ObtenExamenesActivos", CommandType.StoredProcedure, list, _connectionString))
            {
                while (reader.Read())
                {
                    var entity = new ExamenIntegradorEntity();
                    entity.Matricula = ComprobarNulos.CheckStringNull(reader["MATRICULA"]);
                    entity.PeriodoGraduacion = ComprobarNulos.CheckStringNull(reader["PERIODO_GRADUACION"]);
                    entity.Nivel = ComprobarNulos.CheckStringNull(reader["NIVEL_ACADEMICO"]);
                    entity.NombreRequisito = ComprobarNulos.CheckStringNull(reader["NOMBRE_REQUISITO"]);
                    entity.Estatus = ComprobarNulos.CheckStringNull(reader["ESTATUS"]);
                    entity.FechaExamenDate = ComprobarNulos.CheckDateTimeNull(reader["FECHA_EXAMEN"]);
                    if (!reader.IsDBNull(reader.GetOrdinal("FECHA_EXAMEN")))
                    {
                        DateTime dt = reader.GetDateTime(reader.GetOrdinal("FECHA_EXAMEN"));
                        entity.FechaExamen = dt.ToShortDateString();
                    }
                    expedientes.Add(entity);
                    entity.Result = true;
                }
            }
            return expedientes;
        }
        public async Task<ExamenIntegradorEntity> GetMatricula(string matricula)
        {
            ExamenIntegradorEntity entity = new ExamenIntegradorEntity();
            IList<Parameter> list = new List<Parameter>
            {
                DataBase.CreateParameter("@MATRICULA", DbType.String, 9, ParameterDirection.Input, false, null, DataRowVersion.Default, matricula)
            };
            using (IDataReader reader = await DataBase.GetReader("spExamenIntegrador_ObtenerPorMatricula", CommandType.StoredProcedure, list, _connectionString))
            {
                while (reader.Read())
                {
                    entity.Matricula = ComprobarNulos.CheckStringNull(reader["MATRICULA"]);
                    entity.PeriodoGraduacion = ComprobarNulos.CheckStringNull(reader["PERIODO_GRADUACION"]);
                    entity.Nivel = ComprobarNulos.CheckStringNull(reader["NIVEL_ACADEMICO"]);
                    entity.NombreRequisito = ComprobarNulos.CheckStringNull(reader["NOMBRE_REQUISITO"]);
                    entity.Estatus = ComprobarNulos.CheckStringNull(reader["ESTATUS"]);
                    entity.FechaExamenDate = ComprobarNulos.CheckDateTimeNull(reader["FECHA_EXAMEN"]);
                    entity.FechaExamen = entity.FechaExamenDate.ToString("dd-MM-yyyy");
                    entity.UltimaActualizacion = ComprobarNulos.CheckDateTimeNull(reader["FECHA_REGISTRO"]);
                    entity.Aplica = ComprobarNulos.CheckBooleanNull(reader["APLICA"]);
                    if (entity.FechaExamen == null)
                        entity.FechaExamen = "01/01/0001";
                    entity.Result = true;
                }
            }
            return entity;
        }
        public async Task GuardaExamenesIntegrador(List<ExamenIntegradorEntity> expedientes, string usuarioAplicacion)
        {
            foreach (var expediente in expedientes)
            {
                IList<Parameter> list = new List<Parameter>
                {
                    DataBase.CreateParameter("@MATRICULA", DbType.AnsiString, 9, ParameterDirection.Input, false, null, DataRowVersion.Default, expediente.Matricula ),
                    DataBase.CreateParameter("@PERIODO_GRADUACION", DbType.AnsiString, 30, ParameterDirection.Input, false, null, DataRowVersion.Default, expediente.PeriodoGraduacion ),
                    DataBase.CreateParameter("@NIVEL_ACADEMICO", DbType.AnsiString, 250, ParameterDirection.Input, false, null, DataRowVersion.Default, expediente.Nivel ),
                    DataBase.CreateParameter("@NOMBRE_REQUISITO", DbType.AnsiString, 250, ParameterDirection.Input, false, null, DataRowVersion.Default, expediente.NombreRequisito ),
                    DataBase.CreateParameter("@ESTATUS", DbType.AnsiString, 250, ParameterDirection.Input, false, null, DataRowVersion.Default, expediente.Estatus ),
                    DataBase.CreateParameter("@FECHA_EXAMEN", DbType.DateTime, 250, ParameterDirection.Input, false, null, DataRowVersion.Default, expediente.FechaExamen != string.Empty ? expediente.FechaExamenDate : null ),
                    DataBase.CreateParameter("@APP_USUARIO", DbType.AnsiString, 50, ParameterDirection.Input, false, null, DataRowVersion.Default, usuarioAplicacion ),
                    DataBase.CreateParameter("@UPDATEFLAG", DbType.Boolean, 2, ParameterDirection.Input, false, null, DataRowVersion.Default, expediente.UpdateFlag )
                };
              await  DataBase.InsertOut("spExamenIntegrador_InsertarActualizar", CommandType.StoredProcedure, list, _connectionString);
            }
        }

        public async Task<ExisteAlumnoDto> ExisteAlumno(int idUsuario, string matricula)
        {
            ExisteAlumnoDto dto = new ExisteAlumnoDto();
            IList<Parameter> list = new List<Parameter>
            {
                DataBase.CreateParameter("@pIdUsuario", DbType.Int32, 10, ParameterDirection.Input, false, null, DataRowVersion.Default, idUsuario),
                DataBase.CreateParameter("@pMatricula", DbType.AnsiString, 9, ParameterDirection.Input, false, null, DataRowVersion.Default, matricula)
            };
            using (IDataReader reader = await DataBase.GetReader("spAlumnosProspCandidatos_ObtenerPorIdUsuarioMatricula", CommandType.StoredProcedure, list, _connectionString))
            {
                while (reader.Read())
                {
                    dto.Existe = ComprobarNulos.CheckIntNull(reader["Existe"]);
                    dto.Result = true;
                }
            }
            return dto;
        }
    }
}