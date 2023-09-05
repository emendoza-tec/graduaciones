using HabilitadorGraduaciones.Core.DTO;
using HabilitadorGraduaciones.Core.DTO.Base;
using HabilitadorGraduaciones.Core.Entities;
using HabilitadorGraduaciones.Data.Interfaces;
using HabilitadorGraduaciones.Services.Interfaces;

namespace HabilitadorGraduaciones.Services
{
    public class RolesService : IRolesService
    {
        private readonly IRolesRepository _rolesData;

        public RolesService(IRolesRepository rolesData)
        {
            _rolesData = rolesData;
        }

        public async Task<List<RolesEntity>> ObtenRoles()
        {
            return await _rolesData.ObtenRoles();
        }

        public async Task<RolesEntity> ObtenerRolesPorId(int idRol)
        {
            return await _rolesData.ObtenerRolesPorId(idRol);
        }

        public async Task<BaseOutDto> GuardaRol(RolesEntity rol)
        {
            return await _rolesData.GuardaRol(rol);
        }

        public async Task<BaseOutDto> ModificaRol(RolesEntity rol)
        {
            return await _rolesData.ModificaRol(rol);
        }

        public async Task<BaseOutDto> CambiaEstatusRol(RolesEntity rol)
        {
            return await _rolesData.CambiaEstatusRol(rol);
        }

        public async Task<BaseOutDto> EliminaRol(RolesEntity rol)
        {
            return await _rolesData.EliminaRol(rol);
        }

        public async Task<List<SeccionesPermisosDto>> ObtenerSecciones()
        {
            return await _rolesData.ObtenerSecciones();
        }

        public async Task<List<RolesDto>> ObtenerDescripcionRoles()
        {
            return await _rolesData.ObtenerDescripcionRoles();
        }
    }
}
