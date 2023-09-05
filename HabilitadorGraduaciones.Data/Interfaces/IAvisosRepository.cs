using HabilitadorGraduaciones.Core.DTO;
using HabilitadorGraduaciones.Core.DTO.Base;
using HabilitadorGraduaciones.Core.Entities;
using System.Text;

namespace HabilitadorGraduaciones.Data.Interfaces
{
    public interface IAvisosRepository
    {
        public Task<BaseOutDto> SetAviso(AvisoGuardar entity);
        public void SendEmails(int idAviso, AvisoGuardar entity);
        public Task<bool> SetCorreoEnviado(StringBuilder listId);
        public Task<List<CorreosEnviadosEntity>> GetCorreoLote(int idAviso);
        public Task<AvisosDto> Get3Avisos(AvisosEntity entity);
        public Task<AvisosDto> GetAvisos(AvisosEntity entity);
        public Task<List<CatalogoDto>> GetCatalogo(int opcion);
        public Task<List<CatalogoDto>> GetCatalogoMatricula(FiltrosMatriculaDto filtros);
    }
}
