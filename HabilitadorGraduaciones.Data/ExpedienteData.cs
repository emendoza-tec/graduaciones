using HabilitadorGraduaciones.Core.DTO;
using HabilitadorGraduaciones.Core.Entities.Expediente;
using HabilitadorGraduaciones.Data.Interfaces;
using HabilitadorGraduaciones.Data.Utils;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace HabilitadorGraduaciones.Data
{
    public class ExpedienteData : IExpedienteRepository
    {
        private readonly string _connectionString;

        public ExpedienteData(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<List<ExpedienteEntity>> GetExpedientes(int idUsuario)
        {
            var expedientes = new List<ExpedienteEntity>();
            IList<Parameter> list = new List<Parameter>
            {
                DataBase.CreateParameter("@pIdUsuario", DbType.Int32, 10, ParameterDirection.Input, false, null, DataRowVersion.Default, idUsuario)
            };

            using (IDataReader reader = await DataBase.GetReader("spExpediente_ObtenExpedientes", CommandType.StoredProcedure, list, _connectionString))
            {
                while (reader.Read())
                {
                    var entity = new ExpedienteEntity();
                    entity.Matricula = ComprobarNulos.CheckStringNull(reader["MATRICULA"]);
                    entity.Estatus = ComprobarNulos.CheckStringNull(reader["ESTATUS"]);
                    entity.Detalle = ComprobarNulos.CheckStringNull(reader["DETALLE"]);
                    entity.UltimaActualizacion = ComprobarNulos.CheckDateTimeNull(reader["ULTIMAACTUALIZACION"]);
                    expedientes.Add(entity);
                    entity.Result = true;
                }
            }
            return expedientes;
        }
        public async Task<ExpedienteEntity> GetByAlumno(string matricula)
        {
            var result = new ExpedienteEntity();
            IList<Parameter> list = new List<Parameter>
            {
                DataBase.CreateParameter("@Matricula", DbType.String, 9, ParameterDirection.Input, false, null, DataRowVersion.Default, matricula)
            };
            using (IDataReader reader = await DataBase.GetReader("spExpedientes_ObtenerPorAlumno", CommandType.StoredProcedure, list, _connectionString))
            {
                while (reader.Read())
                {
                    result.Matricula = ComprobarNulos.CheckStringNull(reader["MATRICULA"]);
                    result.Estatus = ComprobarNulos.CheckStringNull(reader["ESTATUS"]);
                    result.Detalle = ComprobarNulos.CheckStringNull(reader["DETALLE"]);
                    result.UltimaActualizacion = ComprobarNulos.CheckDateTimeNull(reader["ULTIMAACTUALIZACION"]);

                    result.Result = true;
                }
            }
            return result;
        }

        public async Task<List<ExpedienteEntity>> ConsultarComentarios(string Matricula)
        {
            var expedientes = new List<ExpedienteEntity>();
            IList<Parameter> list = new List<Parameter>
            {
                DataBase.CreateParameter("@Matricula", DbType.String, 9, ParameterDirection.Input, false, null, DataRowVersion.Default, Matricula)
            };
            using (IDataReader reader = await DataBase.GetReader("spExpedientes_ObtenerComentariosExpedientes", CommandType.StoredProcedure, list, _connectionString))
            {
                while (reader.Read())
                {
                    var entity = new ExpedienteEntity();
                    entity.Detalle = ComprobarNulos.CheckStringNull(reader["COMENTARIO"]);
                    entity.UltimaActualizacion = ComprobarNulos.CheckDateTimeNull(reader["FECHA_REGISTRO"]);
                    expedientes.Add(entity);
                    entity.Result = true;
                }
            }
            return expedientes;
        }
        public async Task GuardaExpedientes(List<ExpedienteEntity> expedientes, string usuarioAplicacion)
        {
            foreach (var expediente in expedientes)
            {
                IList<Parameter> list = new List<Parameter>
            {
                DataBase.CreateParameter("@Matricula", DbType.AnsiString, 9, ParameterDirection.Input, false, null, DataRowVersion.Default, expediente.Matricula ),
                DataBase.CreateParameter("@Estatus", DbType.AnsiString, 30, ParameterDirection.Input, false, null, DataRowVersion.Default, expediente.Estatus ),
                DataBase.CreateParameter("@Detalle", DbType.AnsiString, 250, ParameterDirection.Input, false, null, DataRowVersion.Default, expediente.Detalle ),
                DataBase.CreateParameter("@IsModificarAlumno", DbType.Boolean, 2, ParameterDirection.Input, false, null, DataRowVersion.Default, expediente.isModificarAlumno ),
                DataBase.CreateParameter("@UsuarioModifico", DbType.AnsiString, 50, ParameterDirection.Input, false, null, DataRowVersion.Default, usuarioAplicacion )
            };
               await DataBase.InsertOut("spExpediente_InsertaExpedientes", CommandType.StoredProcedure, list, _connectionString);
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