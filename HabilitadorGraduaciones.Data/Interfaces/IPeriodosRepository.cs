using HabilitadorGraduaciones.Core.DTO;
using HabilitadorGraduaciones.Core.DTO.Base;
using HabilitadorGraduaciones.Core.Entities;

namespace HabilitadorGraduaciones.Data.Interfaces
{
    public interface IPeriodosRepository
    {
        public Task<List<PeriodosEntity>> GetPeriodos(PeriodosEntity data);
        public Task<PeriodosEntity> GetPeriodoAlumno(string matricula);
        public Task<BaseOutDto> GuardarPeriodo(PeriodosDto data);
        public Task<ConfiguracionClinicasDto> GetCongfiguracionClinicas(string carrera);
        public Task<List<ConfiguracionClinicasDto>> GetClinicas();
        public Task<List<PeriodoGraduacionEntity>> GetPeriodosGraduacion();
        public Task<BaseOutDto> EnviarCorreo(UsuarioDto correo);
    }
}
