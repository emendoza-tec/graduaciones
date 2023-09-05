using HabilitadorGraduaciones.Core.DTO;
using HabilitadorGraduaciones.Core.Entities;
using HabilitadorGraduaciones.Data.Interfaces;
using HabilitadorGraduaciones.Data.Utils;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace HabilitadorGraduaciones.Data
{
    public class SabanaData : IReporteSabanaRepository
    {
        private readonly string _connectionString;
        
        public SabanaData(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<List<SabanaEntity>> GetReporteSabana(UsuarioAdministradorDto data)
        {
            var reg = new List<SabanaEntity>();
            DataTable dtCampusSede = GetDataTableCampus(data);
            DataTable dtNivel = GetDataTableNivel(data);
            IList<ParameterSQl> list = new List<ParameterSQl>
            {
                DataBase.CreateParameterSql("@UsuarioCampusSede", SqlDbType.Structured, int.MaxValue, ParameterDirection.Input, false, null,DataRowVersion.Default, dtCampusSede),
                DataBase.CreateParameterSql("@UsuarioNivel", SqlDbType.Structured, int.MaxValue, ParameterDirection.Input, false, null,DataRowVersion.Default, dtNivel),
            };

            using (IDataReader reader = await DataBase.GetReaderSql("spReporteSabana", CommandType.StoredProcedure, list, _connectionString))
            {
                while (reader.Read())
                {
                    var entity = new SabanaEntity();
                    entity.Matricula = ComprobarNulos.CheckStringNull(reader["MATRICULA"]);
                    entity.NombreCompleto = ComprobarNulos.CheckStringNull(reader["NOMBRE_COMPLETO"]);
                    entity.ClaveProgramaAcademico = ComprobarNulos.CheckStringNull(reader["CLAVE_PROGRAMA_ACADEMICO"]);
                    entity.ConcentracionUno = ComprobarNulos.CheckStringNull(reader["CONCENTRACION_UNO"]);
                    entity.ConcentracionDos = ComprobarNulos.CheckStringNull(reader["CONCENTRACION_DOS"]);
                    entity.ConcentracionTres = ComprobarNulos.CheckStringNull(reader["CONCENTRACION_TRES"]);
                    entity.Ulead = ComprobarNulos.CheckStringNull(reader["ULEAD"]);
                    entity.DiplomaInternacional = ComprobarNulos.CheckStringNull(reader["DIPLOMA_INTERNACIONAL"]);
                    entity.Genero = ComprobarNulos.CheckStringNull(reader["GENERO"]);
                    entity.Nacionalidad = ComprobarNulos.CheckStringNull(reader["NACIONALIDAD"]);
                    entity.Telefono = ComprobarNulos.CheckStringNull(reader["TELEFONO"]);
                    entity.Correo = ComprobarNulos.CheckStringNull(reader["CORREO"]);
                    entity.Periodo = ComprobarNulos.CheckStringNull(reader["PERIODO"]);
                    entity.Campus = ComprobarNulos.CheckStringNull(reader["CAMPUS"]);
                    entity.NivelAcademico = ComprobarNulos.CheckStringNull(reader["NIVEL_ACADEMICO"]);
                    entity.CreditosPlan = ComprobarNulos.CheckStringNull(reader["CREDITOS_PLAN"]);
                    entity.CreditosPendientes = ComprobarNulos.CheckStringNull(reader["CREDITOS_PENDIENTES"]);
                    entity.CreditosAcreditados = ComprobarNulos.CheckStringNull(reader["CREDITOS_ACREDITADOS"]);
                    entity.CreditosFaltantes = ComprobarNulos.CheckStringNull(reader["CREDITOS_FALTANTES"]);
                    entity.CreditosPeriodo = ComprobarNulos.CheckStringNull(reader["CREDITOS_PERIODO"]);
                    entity.SemanasTec = ComprobarNulos.CheckStringNull(reader["SEMANAS_TEC"]);
                    entity.ServicioSocialHt = ComprobarNulos.CheckStringNull(reader["SERVICIO_SOCIAL_HT"]);
                    entity.ServicioSocialEstatus = ComprobarNulos.CheckStringNull(reader["SERVICIO_SOCIAL_ESTATUS"]);
                    entity.ExamenIngles = ComprobarNulos.CheckStringNull(reader["EXAMEN_INGLES"]);
                    entity.ExamenInglesEstatus = ComprobarNulos.CheckStringNull(reader["EXAMEN_INGLES_ESTATUS"]);
                    entity.ExamenInglesFecha = ComprobarNulos.CheckStringNull(reader["EXAMEN_INGLES_FECHA"]);
                    entity.ExamenInglesPuntaje = ComprobarNulos.CheckStringNull(reader["EXAMEN_INGLES_PUNTAJE"]);
                    entity.Ceneval = ComprobarNulos.CheckStringNull(reader["CENEVAL"]);
                    entity.CenevalEstatus = ComprobarNulos.CheckStringNull(reader["CENEVALE_ESTATUS"]);
                    entity.ExamenIntegrador = ComprobarNulos.CheckStringNull(reader["EXAMEN_INTEGRADOR"]);
                    entity.NivelIdiomaRequerido = ComprobarNulos.CheckStringNull(reader["NIVEL_IDIOMA_REQ_GRAD"]);
                    entity.IdiomaDistEsp = ComprobarNulos.CheckStringNull(reader["IDIOMA_DIST_ESP"]);
                    entity.CreditosCursadosExtranjero = ComprobarNulos.CheckIntNull(reader["CREDITOS_CURSA_EXTRANJERO"]);
                    entity.Promedio = ComprobarNulos.CheckStringNull(reader["PROMEDIO"]);
                    entity.FechaRegistro = ComprobarNulos.CheckDateTimeNull(reader["FECHA_REGISTRO"]);
                    entity.Shadegr = ComprobarNulos.CheckStringNull(reader["SHADEGR"]);
                    entity.PeriodoCeremonia = ComprobarNulos.CheckStringNull(reader["PERIODO_CEREMONIA"]);

                    reg.Add(entity);
                }
            }
            return reg;
        }
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
