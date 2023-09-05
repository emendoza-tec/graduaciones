using HabilitadorGraduaciones.Core.DTO;
using HabilitadorGraduaciones.Core.DTO.Base;
using HabilitadorGraduaciones.Core.Entities;
using HabilitadorGraduaciones.Data.Interfaces;
using HabilitadorGraduaciones.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace HabilitadorGraduaciones.Services
{
    public class SolicitudDeCambioDeDatosService : ISolicitudDeCambioDeDatosService
    {
        private readonly string contenedor = "Solicitudes";
        private readonly IArchivoLocalStorageService _archivoStorage;
        private readonly ISolicitudDeCambioDeDatosRepository _solicitudesData;
        public IConfiguration Configuration { get; }

        public SolicitudDeCambioDeDatosService(ISolicitudDeCambioDeDatosRepository solicitudesData, IArchivoLocalStorageService archivoStorage)
        {
            _solicitudesData = solicitudesData;
            _archivoStorage = archivoStorage;
        }

        public async Task<List<EstatusSolicitudDatosPersonalesEntity>> GetEstatusSolicitudes()
        {
            return await _solicitudesData.GetEstatusSolicitudes();
        }

        public async Task<List<SolicitudDeCambioDeDatosEntity>> GetPendientes(int idUsuario)
        {
            return await _solicitudesData.GetPendientes(idUsuario);
        }

        public async Task<List<SolicitudDeCambioDeDatosEntity>> Get(int idEstatusSolicitud, int idUsuario)
        {
            return await _solicitudesData.Get(idEstatusSolicitud, idUsuario);
        }

        public async Task<List<DetalleSolicitudDeCambioDeDatosEntity>> GetDetalle(int idSolicitud)
        {
            return await _solicitudesData.GetDetalle(idSolicitud);
        }

        public async Task<BaseOutDto> GuardaSolicitudes(List<SolicitudDeCambioDeDatosDto> solicitudes)
        {
            foreach (var solicitud in solicitudes)
            {
                if (solicitud.Detalle != null)
                {
                    foreach (var archivo in solicitud.Detalle)
                    {
                        if (archivo.Archivo != null)
                        {
                            byte[] bytes = Convert.FromBase64String(archivo.Archivo);
                            MemoryStream stream = new MemoryStream(bytes);
                            IFormFile file = new FormFile(stream, 0, bytes.Length, archivo.Documento, archivo.Documento);

                            archivo.AzureStorage = await _archivoStorage.SaveFile(contenedor, file);
                        }
                    }
                }
            }
            return await _solicitudesData.GuardaSolicitudes(solicitudes);
        }

        public async Task<BaseOutDto> ModificaSolicitud(ModificarEstatusSolicitudDto solicitud)
        {
            return await _solicitudesData.ModificaSolicitud(solicitud);
        }

        public async Task<TotalSolicitudesDto> GetConteoPendientes(int idUsuario)
        {   
            return await _solicitudesData.GetConteoPendientes(idUsuario);
        }

        public async Task<CorreoSolicitudDatosPersonalesDto> GetCorreo(int idCorreo)
        {
            return await _solicitudesData.GetCorreo(idCorreo);
        }

    }
}
