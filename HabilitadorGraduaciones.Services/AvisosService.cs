using HabilitadorGraduaciones.Core.CustomException;
using HabilitadorGraduaciones.Core.DTO;
using HabilitadorGraduaciones.Core.DTO.Base;
using HabilitadorGraduaciones.Core.Entities;
using HabilitadorGraduaciones.Data.Interfaces;
using HabilitadorGraduaciones.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HabilitadorGraduaciones.Services
{
    public class AvisosService : IAvisosService
    {
        private readonly IAvisosRepository _avisosData;

        public AvisosService(IAvisosRepository avisosData)
        {
            _avisosData = avisosData;
        }

        public async Task<AvisosDto> Get3AvisosService(AvisosEntity entity)
        {
            return  await _avisosData.Get3Avisos(entity);
        }
        public async Task<BaseOutDto> SetAvisosService(AvisoGuardar entity)
        {
            return await _avisosData.SetAviso(entity);

        }
        public async Task<AvisosDto> GetAvisosService(AvisosEntity entity)
        {
            return await _avisosData.GetAvisos(entity);
        }
        /// <summary>Obtener el catálogo correspondiente.</summary>
        /// <param name="opcion">Opción para identificar el catálogo necesario.</param>
        /// <returns>Catálogo para llenado de dropdown.</returns>
        public async Task<List<CatalogoDto>> ObtenerCatalogo(int opcion)
        {
            List<CatalogoDto> lstCatalogos = new List<CatalogoDto>();
            try
            {
                lstCatalogos = await _avisosData.GetCatalogo(opcion);
            }
            catch (Exception ex)
            {
                CatalogoDto catalogo = new CatalogoDto();
                catalogo.Result = false;
                catalogo.ErrorMessage = ex.Message;
            }
            return lstCatalogos;

        }
        public async Task<List<CatalogoDto>> ObtenerCatalogoMatricula(FiltrosMatriculaDto filtros)
        {
            List<CatalogoDto> lstCatalogos = new List<CatalogoDto>();
            try
            {
                lstCatalogos = await _avisosData.GetCatalogoMatricula(filtros);
            }
            catch (Exception ex)
            {
                CatalogoDto catalogo = new CatalogoDto();
                catalogo.Result = false;
                catalogo.ErrorMessage = ex.Message;
            }
            return lstCatalogos;
        }
    }
}