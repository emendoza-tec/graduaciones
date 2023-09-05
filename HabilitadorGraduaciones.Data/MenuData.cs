using HabilitadorGraduaciones.Core.Entities;
using HabilitadorGraduaciones.Data.Utils;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace HabilitadorGraduaciones.Data
{
    public class MenuData
    {
        public const string ConnectionStrings = "ConnectionStrings:DefaultConnection";
        public IConfiguration Configuration { get; }
        public MenuData(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public async Task<List<MenuEntity>> GetMenuPadre()
        {
            var ListaEntity = new List<MenuEntity>();
            using (IDataReader reader = await DataBase.GetReader("spMenus_ObtenerMenu", CommandType.StoredProcedure, Configuration[ConnectionStrings]))
            {
                while (reader.Read())
                {
                    var entity = new MenuEntity();
                    entity.Id = ComprobarNulos.CheckNull<int>(reader["ID"]);
                    entity.Nombre = ComprobarNulos.CheckNull<string>(reader["NOMBRE"]);
                    entity.Path = ComprobarNulos.CheckNull<string>(reader["PATH"]);
                    entity.Icono = ComprobarNulos.CheckNull<string>(reader["ICONO"]);
                    entity.MenuHijo = await GetHijos(entity.Id);
                    entity.Result = true;
                    ListaEntity.Add(entity);
                }
            }
            return ListaEntity;
        }

        public async Task<List<MenuHijoEntity>> GetHijos(int id)
        {
            var hijos = new List<MenuHijoEntity>();
            IList<Parameter> list = new List<Parameter>
             {
                 DataBase.CreateParameter("@MenuPadre", DbType.Int32, 4, ParameterDirection.Input, false, null, DataRowVersion.Default, id)
              };
            using (IDataReader reader = await DataBase.GetReader("spSubMenus_ObtenerMenu", CommandType.StoredProcedure, list, Configuration[ConnectionStrings]))
            {
                while (reader.Read())
                {
                    var menuHijo = new MenuHijoEntity();
                    menuHijo.Id = ComprobarNulos.CheckNull<int>(reader["ID"]);
                    menuHijo.Nombre = ComprobarNulos.CheckNull<string>(reader["NOMBRE"]);
                    menuHijo.Path = ComprobarNulos.CheckNull<string>(reader["PATH"]);
                    menuHijo.Icono = ComprobarNulos.CheckNull<string>(reader["ICONO"]);
                    hijos.Add(menuHijo);
                }
            }
            return hijos;
        }
    }
}