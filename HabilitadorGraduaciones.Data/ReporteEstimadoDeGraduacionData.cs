using HabilitadorGraduaciones.Core.DTO;
using HabilitadorGraduaciones.Core.Entities;
using HabilitadorGraduaciones.Data.Interfaces;
using HabilitadorGraduaciones.Data.Utils;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace HabilitadorGraduaciones.Data
{
    public class ReporteEstimadoDeGraduacionData : IReporteEstimadoRepository
    {
        public const string ConnectionStrings = "ConnectionStrings:DefaultConnection";
        public IConfiguration Configuration { get; }

        public ReporteEstimadoDeGraduacionData(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        #region Método para consumir el spReporteEstimadoDeGraduacion de base de datos
        public async Task<List<ReporteEstimadoDeGraduacionEntity>> GetReporteEstimadoDeGraduacion(UsuarioAdministradorDto data)
        {
            var listReporteEstimadoG = new List<ReporteEstimadoDeGraduacionEntity>();
            DataTable dtCampusSede = GetDataTableCampus(data);
            DataTable dtNivel = GetDataTableNivel(data);
            IList<ParameterSQl> list = new List<ParameterSQl>
                {
                    DataBase.CreateParameterSql("@UsuarioCampusSede", SqlDbType.Structured, int.MaxValue, ParameterDirection.Input, false, null,DataRowVersion.Default, dtCampusSede),
                    DataBase.CreateParameterSql("@UsuarioNivel", SqlDbType.Structured, int.MaxValue, ParameterDirection.Input, false, null,DataRowVersion.Default, dtNivel),
                };
            using (IDataReader dataReader = await DataBase.GetReaderSql("spReporte_EstimadoDeGraduacion", CommandType.StoredProcedure, list, Configuration[ConnectionStrings]))
            {
                while (dataReader.Read())
                {
                    var entity = new ReporteEstimadoDeGraduacionEntity();
                    entity.Matricula = ComprobarNulos.CheckStringNull(dataReader["MATRICULA"]);
                    entity.Nombre = ComprobarNulos.CheckStringNull(dataReader["NOMBRE"]);
                    entity.Carrera = ComprobarNulos.CheckStringNull(dataReader["CARRERA"]);
                    entity.Campus = ComprobarNulos.CheckStringNull(dataReader["CAMPUS"]);
                    entity.Sede = ComprobarNulos.CheckStringNull(dataReader["SEDE"]);
                    entity.PeriodoEstimado = ComprobarNulos.CheckStringNull(dataReader["PERIODO_ESTIMADO"]);
                    entity.Confirmacion = ComprobarNulos.CheckStringNull(dataReader["CONFIRMACION"]);
                    entity.PeriodoConfirmadoG = ComprobarNulos.CheckStringNull(dataReader["PERIODO_CONFIRMADO_G"]);
                    entity.FechaConfirmacionOCambio = ComprobarNulos.CheckDateTimeNull(dataReader["FECHA_CONFIRMACION_CAMBIO"]);
                    entity.Motivo = ComprobarNulos.CheckStringNull(dataReader["MOTIVO"]);
                    entity.CantidadCambiosRealizados = ComprobarNulos.CheckIntNull(dataReader["CANTIDAD_CAMBIOS_REALIZADOS"]);
                    entity.RegistroAsistenciaPeriodoIntensivo = ComprobarNulos.CheckStringNull(dataReader["REGISTRO_ASISTENCIA_PI"]);
                    entity.MotivoPeriodoIntensivo = ComprobarNulos.CheckStringNull(dataReader["MOTIVO_PERIODO_INTENSIVO"]);

                    listReporteEstimadoG.Add(entity);
                }
            }
            return listReporteEstimadoG;
        }
        #endregion

        public static DataTable GetDataTableCampus(UsuarioAdministradorDto usuario)
        {
            DataRow row;
            DataTable dtCampusSede = new DataTable("UsuarioCampusSedeType");

            DataColumn column = new DataColumn
            {
                DataType = typeof(int),
                ColumnName = "IdUsuario",
                ReadOnly = true
            };
            dtCampusSede.Columns.Add(column);
            column = new DataColumn
            {
                DataType = typeof(string),
                ColumnName = "ClaveCampus",
                ReadOnly = true
            };
            dtCampusSede.Columns.Add(column);
            column = new DataColumn
            {
                DataType = typeof(string),
                ColumnName = "ClaveSede",
                ReadOnly = false
            };
            dtCampusSede.Columns.Add(column);
            foreach (var sede in usuario.Sedes)
            {
                row = dtCampusSede.NewRow();
                row["IdUsuario"] = usuario.IdUsuario;
                row["ClaveCampus"] = sede.ClaveCampus;
                row["ClaveSede"] = sede.ClaveSede;
                dtCampusSede.Rows.Add(row);
            }
            return dtCampusSede;
        }
        public static DataTable GetDataTableNivel(UsuarioAdministradorDto usuario)
        {
            DataRow row;
            DataTable dtNivel = new DataTable("UsuarioNivelType");

            DataColumn column = new DataColumn
            {
                DataType = typeof(int),
                ColumnName = "IdUsuario",
                ReadOnly = true
            };
            dtNivel.Columns.Add(column);
            column = new DataColumn
            {
                DataType = typeof(string),
                ColumnName = "ClaveNivel",
                ReadOnly = true
            };
            dtNivel.Columns.Add(column);
            foreach (var nivel in usuario.Niveles)
            {
                row = dtNivel.NewRow();
                row["IdUsuario"] = usuario.IdUsuario;
                row["ClaveNivel"] = nivel.ClaveNivel;
                dtNivel.Rows.Add(row);
            }
            return dtNivel;
        }
    }
}