using HabilitadorGraduaciones.Core.CustomException;
using HabilitadorGraduaciones.Core.DTO;
using HabilitadorGraduaciones.Core.DTO.Base;
using HabilitadorGraduaciones.Core.Entities;
using HabilitadorGraduaciones.Data.Interfaces;
using HabilitadorGraduaciones.Data.Utils;
using HabilitadorGraduaciones.Data.Utils.Enums;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Text;

namespace HabilitadorGraduaciones.Data
{
    public class PeriodosData : IPeriodosRepository
    {
        public const string ConnectionStrings = "ConnectionStrings:DefaultConnection";
        public IConfiguration Configuration { get; }
        private readonly IEmailModuleRepository _emailData;

        public PeriodosData(IConfiguration configuration, IEmailModuleRepository emailData)
        {
            Configuration = configuration;
            _emailData = emailData;
        }
        public async Task<List<PeriodosEntity>> GetPeriodos(PeriodosEntity data)
        {
            List<PeriodosEntity> periodos = new List<PeriodosEntity>();
            IList<Parameter> _params = new List<Parameter>
            {
                DataBase.CreateParameter("@Matricula", DbType.String, 50, ParameterDirection.Input, true, null, DataRowVersion.Default, data.Matricula ),
                DataBase.CreateParameter("@Periodo", DbType.String, 50, ParameterDirection.Input, true, null, DataRowVersion.Default, data.PeriodoId ),
            };
            using (IDataReader reader = await DataBase.GetReader("spPeriodos_ObtenerPeriodos", CommandType.StoredProcedure, _params, Configuration[ConnectionStrings]))
            {
                while (reader.Read())
                {
                    var _periodo = new PeriodosEntity
                    {
                        PeriodoId = ComprobarNulos.CheckStringNull(reader["ID"]),
                        Descripcion = ComprobarNulos.CheckStringNull(reader["DESCRIPCION"]),
                        IsRegular = ComprobarNulos.CheckBooleanNull(reader["REGULAR"]),
                        CreditosPeriodo = Convert.ToDecimal(reader["CREDITOS_PERIODO"]),
                        TipoPeriodo = ComprobarNulos.CheckIntNull(reader["INDICADOR_PERIODO"]),
                        FechaInicio = ComprobarNulos.CheckDateTimeNull(reader["FECHA_INICIO"]),
                        FechaFin = ComprobarNulos.CheckDateTimeNull(reader["FECHA_FIN"])
                    };
                    periodos.Add(_periodo);
                }
            }
            return periodos;
        }
        public async Task<PeriodosEntity> GetPeriodoAlumno(string matricula)
        {
            PeriodosEntity periodo = new PeriodosEntity();
            IList<Parameter> _params = new List<Parameter>
            {
                DataBase.CreateParameter("@Matricula", DbType.String, 1000, ParameterDirection.Input, true, null, DataRowVersion.Default, matricula ),
            };
            using (IDataReader reader = await DataBase.GetReader("spPeriodos_ObtenerPeriodo", CommandType.StoredProcedure, _params, Configuration[ConnectionStrings]))
            {
                while (reader.Read())
                {
                    var _periodo = new PeriodosEntity
                    {
                        PeriodoId = ComprobarNulos.CheckStringNull(reader["ID"]),
                        Descripcion = ComprobarNulos.CheckStringNull(reader["DESCRIPCION"]),
                        IsRegular = ComprobarNulos.CheckBooleanNull(reader["REGULAR"]),
                        CreditosPeriodo = Convert.ToDecimal(reader["CREDITOS_PERIODO"]),
                        TipoPeriodo = ComprobarNulos.CheckIntNull(reader["INDICADOR_PERIODO"]),
                        FechaInicio = ComprobarNulos.CheckDateTimeNull(reader["FECHA_INICIO"]),
                        FechaFin = ComprobarNulos.CheckDateTimeNull(reader["FECHA_FIN"])
                    };
                    periodo = _periodo;
                }
            }
            return periodo;
        }
        public async Task<BaseOutDto> GuardarPeriodo(PeriodosDto data)
        {
            BaseOutDto insert = new BaseOutDto();
            try
            {
                if (!string.IsNullOrEmpty(data.Matricula))
                {
                    IList<Parameter> _params = new List<Parameter>
                    {
                        DataBase.CreateParameter("@Matricula", DbType.String, 50, ParameterDirection.Input, true, null, DataRowVersion.Default, data.Matricula ),
                        DataBase.CreateParameter("@PeriodoElegido", DbType.String, 50, ParameterDirection.Input, true, null, DataRowVersion.Default, data.PeriodoElegido ),
                        DataBase.CreateParameter("@PeriodoEstimado", DbType.String, 50, ParameterDirection.Input, true, null, DataRowVersion.Default, data.PeriodoEstimado ),
                        DataBase.CreateParameter("@PeriodoCeremonia", DbType.String, 50, ParameterDirection.Input, true, null, DataRowVersion.Default, data.PeriodoCeremonia == null || data.PeriodoCeremonia == "" ? data.PeriodoElegido : data.PeriodoCeremonia ),
                        DataBase.CreateParameter("@MotivoCambioPeriodo", DbType.String, 255, ParameterDirection.Input, true, null, DataRowVersion.Default, data.MotivoCambioPeriodo ),
                        DataBase.CreateParameter("@EleccionAsistenciaCeremonia", DbType.String, 255, ParameterDirection.Input, true, null, DataRowVersion.Default, data.EleccionAsistenciaCeremonia ),
                        DataBase.CreateParameter("@MotivoNoAsistirCeremonia", DbType.String, 255, ParameterDirection.Input, true, null, DataRowVersion.Default, data.MotivoNoAsistirCeremonia ),
                        DataBase.CreateParameter("@OrigenActualiacionPeriodoId", DbType.Int32, 2, ParameterDirection.Input, true, null, DataRowVersion.Default, data.OrigenActualizacionPeriodoId )
                    };
                    await DataBase.InsertOut("spPeriodos_Insertar", CommandType.StoredProcedure, _params, Configuration[ConnectionStrings]);
                    insert.Result = true;
                }
                else
                {
                    insert.Result = false;
                }

            }
            catch (Exception ex)
            {
                insert.Result = false;
                insert.ErrorMessage = ex.Message;
                throw new CustomException("Ocurrió un error al Guardar Periodo", ex);
            }
            return insert;
        }

        public async Task<ConfiguracionClinicasDto> GetCongfiguracionClinicas(string carrera)
        {
            ConfiguracionClinicasDto periodo = new ConfiguracionClinicasDto();
            IList<Parameter> _params = new List<Parameter>
            {
                DataBase.CreateParameter("@Carrera", DbType.String, 50, ParameterDirection.Input, true, null, DataRowVersion.Default, carrera )
            };
            using (IDataReader reader = await DataBase.GetReader("spCatCarrerasClinicaPeriodos_ObtenerPorCarrera", CommandType.StoredProcedure, _params, Configuration[ConnectionStrings]))
            {
                while (reader.Read())
                {
                    var _periodo = new ConfiguracionClinicasDto
                    {
                        CantidadPeriodos = ComprobarNulos.CheckIntNull(reader["CreditosRequeridosPromedio"]),
                        CantidadTrimestres = ComprobarNulos.CheckIntNull(reader["CantidadTrimestres"]),
                        Carrera = ComprobarNulos.CheckStringNull(reader["Carrera"]),
                    };
                    periodo = _periodo;
                }
            }
            return periodo;
        }


        public async Task<List<ConfiguracionClinicasDto>> GetClinicas()
        {
            List<ConfiguracionClinicasDto> periodo = new List<ConfiguracionClinicasDto>();
            using (IDataReader reader = await DataBase.GetReader("spCatCarrerasClinicaPeriodos_ObtenerTodos", CommandType.StoredProcedure, Configuration[ConnectionStrings]))
            {
                while (reader.Read())
                {
                    var _periodo = new ConfiguracionClinicasDto
                    {
                        CantidadPeriodos = ComprobarNulos.CheckIntNull(reader["CreditosRequeridosPromedio"]),
                        CantidadTrimestres = ComprobarNulos.CheckIntNull(reader["CantidadTrimestres"]),
                        Carrera = ComprobarNulos.CheckStringNull(reader["Carrera"]),
                    };
                    periodo.Add(_periodo);
                }
            }
            return periodo;
        }

        public async Task<List<PeriodoGraduacionEntity>> GetPeriodosGraduacion()
        {
            var periodos = new List<PeriodoGraduacionEntity>();
            using (IDataReader reader = await DataBase.GetReader("spCatPriodosGraduacion_ObtenerTodos", CommandType.StoredProcedure, Configuration[ConnectionStrings]))
            {
                while (reader.Read())
                {
                    var entity = new PeriodoGraduacionEntity();
                    entity.Id = ComprobarNulos.CheckNull<Int32>(reader["ID"]);
                    entity.PeriodoGraduacion = ComprobarNulos.CheckNull<Int32>(reader["PERIODO_GRADUACION"]);
                    entity.DescPeriodoGraduacion = ComprobarNulos.CheckNull<string>(reader["DESC_PERIODO_GRADUACION"]);
                    periodos.Add(entity);
                }
            }
            return periodos;
        }
        public async Task<BaseOutDto> EnviarCorreo(UsuarioDto correo)
        {
            BaseOutDto result = new BaseOutDto();
            try
            {
                string cuerpo = BodyPeriodoGraduacion(correo.Nombre, correo.PeriodoGraduacion).ToString();

                return await _emailData.EnviarCorreo(correo.Correo, "Confirmacion de Periodo graduación", cuerpo, "", "", (int)TipoCorreo.SolicitudCambiosHab);

            }
            catch (Exception ex)
            {
                result.Result = false;
                result.ErrorMessage = ex.Message;
                return result;
                throw new CustomException("Error al enviar Correo", ex);
            }
        }
        private StringBuilder BodyPeriodoGraduacion(string nombre, string periodo)
        {
            StringBuilder textoCorreo;
            try
            {
                StringBuilder template = HtmlTemplate.GetTemplateConfirmacionPeriodoGraduacion();
                template.Replace("HomePage", Configuration["ConfiguracionLinks:Home"]);//Link a donde va el boton de Ir a Graduacion
                template.Replace("#Usuario#", nombre);
                template.Replace("#Periodo#", periodo);

                textoCorreo = template.Replace("#Usuario#", nombre);
                textoCorreo = template.Replace("#Periodo#", periodo);


                return textoCorreo;
            }
            catch (Exception ex)
            {
                throw new CustomException("Ocurrió un error en el método BodyPeriodoGraduacion", ex);
            }
        }
    }
}
