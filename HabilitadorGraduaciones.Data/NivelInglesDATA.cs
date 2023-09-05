using HabilitadorGraduaciones.Core.CustomException;
using HabilitadorGraduaciones.Core.DTO;
using HabilitadorGraduaciones.Core.DTO.Base;
using HabilitadorGraduaciones.Core.Entities;
using HabilitadorGraduaciones.Data.Interfaces;
using HabilitadorGraduaciones.Data.Utils;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace HabilitadorGraduaciones.Data
{
    public class NivelInglesData : INivelInglesRepository
    {
        private readonly IConfiguration _configuration =
   new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

        public async Task<NivelInglesDto> GetAlumnoNivelIngles(NivelInglesEntity entity)
        {

            NivelInglesDto nivelInglesdto = new NivelInglesDto();


            IList<Parameter> list = new List<Parameter>
            {
                DataBase.CreateParameter("@MATRICULA", DbType.String, 9, ParameterDirection.Input, false, null, DataRowVersion.Default, entity.Matricula)
            };
            try
            {
                using (IDataReader reader = await DataBase.GetReader("spNivelIngles_Obtener", CommandType.StoredProcedure, list, _configuration.GetConnectionString("DefaultConnection")))
                {
                    while (reader.Read())
                    {
                        nivelInglesdto.Matricula = ComprobarNulos.CheckNull<string>(reader["MATRICULA"]);
                        nivelInglesdto.RequisitoNvl = ComprobarNulos.CheckNull<string>(reader["CLAVE_NIVEL_ACAD_ALUMNO"]);
                        nivelInglesdto.NivelIdiomaAlumno = ComprobarNulos.CheckNull<string>(reader["NIVEL_IDIOMA_REQ_GRAD"]);
                        nivelInglesdto.FechaUltimaModificacion = ComprobarNulos.CheckNull<DateTime>(reader["FECHA_ULTIMA_MODIFICACION"]);
                        nivelInglesdto.NivelCumple = ComprobarNulos.CheckNull<Boolean>(reader["IND_CUMPLE_REQ_GRAD"]);
                        nivelInglesdto.Result = true;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new CustomException("Ocurrió un error en el método GetAlumnoNivelIngles()", ex);
            }

            return nivelInglesdto;

        }

        public async Task<ProgramaDto> GetProgramas(ProgramaDto entity)
        {
            ProgramaDto result = new ProgramaDto();

            List<Programa> lstPrograma = new List<Programa>();

            try
            {
                using (IDataReader reader = await DataBase.GetReader("spProgramaAcademico_Obtener", CommandType.StoredProcedure, _configuration.GetConnectionString("DefaultConnection")))
                {
                    while (reader.Read())
                    {
                        Programa programa = new Programa();
                        programa.NombrePrograma = ComprobarNulos.CheckNull<string>(reader["PROGRAMAS"]);
                        programa.NivelIngles = ComprobarNulos.CheckNull<string>(reader["ID_NIVEL_INGLES"]);
                        lstPrograma.Add(programa);
                    }
                }
                result.Result = true;
                result.Programa = lstPrograma;
            }
            catch (Exception ex)
            {
                throw new CustomException("Ocurrió un error en el método GetProgramas()", ex);
            }
            return result;
        }

        public async Task<BaseOutDto> ModificarNivelIngles(List<ConfiguracionNivelInglesEntity> guardarNiveles)
        {
            BaseOutDto update = new BaseOutDto();
            try
            {
                foreach (var guardarNivel in guardarNiveles)
                {
                    IList<Parameter> list = new List<Parameter>
            {
                DataBase.CreateParameter("@ID_NIVEL_INGLES", DbType.AnsiString, 9, ParameterDirection.Input, false, null, DataRowVersion.Default, guardarNivel.IdNivelIngles ),
                DataBase.CreateParameter("@CLAVE_PROGRAMA_ACADEMICO", DbType.AnsiString, 12, ParameterDirection.Input, false, null, DataRowVersion.Default, guardarNivel.ClaveProgramaAcademico ),
                DataBase.CreateParameter("@ID_USUARIO", DbType.AnsiString, 10, ParameterDirection.Input, false, null, DataRowVersion.Default, guardarNivel.IdUsuario),
                };
                    await DataBase.InsertOut("spRequisitoInglesGuardado_Insertar", CommandType.StoredProcedure, list, _configuration.GetConnectionString("DefaultConnection"));
                }
                update.Result = true;
            }
            catch (Exception ex)
            {
                throw new CustomException("Ocurrió un error en el método ModificarNivelIngles", ex);
            }
            return update;
        }
    }
}