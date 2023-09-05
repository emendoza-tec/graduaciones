using HabilitadorGraduaciones.Core.DTO;
using HabilitadorGraduaciones.Core.DTO.Base;
using HabilitadorGraduaciones.Core.Entities;

namespace HabilitadorGraduaciones.Services.Interfaces
{
    public interface ISolicitudDeCambioDeDatosService
    {
        public Task<List<EstatusSolicitudDatosPersonalesEntity>> GetEstatusSolicitudes();
        public Task<List<SolicitudDeCambioDeDatosEntity>> GetPendientes(int idUsuario);
        public Task<List<SolicitudDeCambioDeDatosEntity>> Get(int idEstatusSolicitud, int idUsuario);
        public Task<List<DetalleSolicitudDeCambioDeDatosEntity>> GetDetalle(int idSolicitud);
        public Task<BaseOutDto> GuardaSolicitudes(List<SolicitudDeCambioDeDatosDto> solicitudes);
        public Task<BaseOutDto> ModificaSolicitud(ModificarEstatusSolicitudDto solicitud);
        public Task<TotalSolicitudesDto> GetConteoPendientes(int idUsuario);
        public Task<CorreoSolicitudDatosPersonalesDto> GetCorreo(int idCorreo);
    }
}
