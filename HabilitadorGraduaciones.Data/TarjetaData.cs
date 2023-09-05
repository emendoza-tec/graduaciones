using HabilitadorGraduaciones.Core.DTO;
using HabilitadorGraduaciones.Core.Entities;
using HabilitadorGraduaciones.Data.Interfaces;
using HabilitadorGraduaciones.Data.Utils;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace HabilitadorGraduaciones.Data
{
    public class TarjetaData : ITarjetaRepository
    {
        private readonly string _connectionString;

        public TarjetaData(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<TarjetaDto> Get(TarjetaEntity entity)
        {
            TarjetaDto result = new TarjetaDto();
            result.Result = false;

            IList<Parameter> list = new List<Parameter>
            {
                DataBase.CreateParameter("@Tarjeta", DbType.Int32, 10, ParameterDirection.Input, false, null, DataRowVersion.Default, entity.IdTarjeta),
                DataBase.CreateParameter("@Idioma", DbType.String, 3, ParameterDirection.Input, false, null, DataRowVersion.Default, entity.Idioma)
            };

            using (IDataReader reader = await DataBase.GetReader("spTarjeta_ObtenerDetalleMultilenguaje", CommandType.StoredProcedure, list, _connectionString))
            {
                while (reader.Read())
                {
                    result.Tarjeta = ComprobarNulos.CheckNull<string>(reader["TARJETA"]);
                    result.Nota = ComprobarNulos.CheckNull<string>(reader["NOTA"]);
                    result.Contacto = ComprobarNulos.CheckNull<string>(reader["CONTACTO"]);
                    result.Correo = ComprobarNulos.CheckNull<string>(reader["CORREO"]);
                    result.Link = ComprobarNulos.CheckNull<string>(reader["LINK"]);

                    if (result.Tarjeta.Equals("Expediente"))
                        result.Documentos = await GetDocumentos(entity.Idioma);

                    result.Result = true;
                }
            }
            return result;
        }

        public async Task<List<DocumentosDto>> GetDocumentos(string idioma)
        {
            var listaDocumentos = new List<DocumentosDto>();
            IList<Parameter> list = new List<Parameter>
            {
                DataBase.CreateParameter("@Idioma", DbType.String, 3, ParameterDirection.Input, false, null, DataRowVersion.Default,idioma)
            };
            using (IDataReader reader = await DataBase.GetReader("spExpedientesDocumentos_ObtenerDocumentosMultilenguaje", CommandType.StoredProcedure, list, _connectionString))
            {
                while (reader.Read())
                {
                    var documento = new DocumentosDto();
                    documento.Descripcion = ComprobarNulos.CheckNull<string>(reader["DESCRIPCION"]);
                    documento.Mexicano = ComprobarNulos.CheckBooleanNull(reader["Mexicano"]);
                    documento.Extranjero = ComprobarNulos.CheckBooleanNull(reader["Extranjero"]);
                    documento.Orden = ComprobarNulos.CheckIntNull(reader["Orden"]);
                    listaDocumentos.Add(documento);
                }
            }
            return listaDocumentos;
        }
    }
}