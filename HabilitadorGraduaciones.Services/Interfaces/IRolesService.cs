using HabilitadorGraduaciones.Core.DTO;
using HabilitadorGraduaciones.Core.DTO.Base;
using HabilitadorGraduaciones.Core.Entities;

namespace HabilitadorGraduaciones.Services.Interfaces
{
    public interface IRolesService
    {
        public Task<List<RolesEntity>> ObtenRoles();
        public Task<RolesEntity> ObtenerRolesPorId(int idRol);
        public Task<BaseOutDto> GuardaRol(RolesEntity rol);
        public Task<BaseOutDto> ModificaRol(RolesEntity rol);
        public Task<BaseOutDto> CambiaEstatusRol(RolesEntity rol);
        public Task<BaseOutDto> EliminaRol(RolesEntity rol);
        public Task<List<SeccionesPermisosDto>> ObtenerSecciones();
        public Task<List<RolesDto>> ObtenerDescripcionRoles();
    }
}
