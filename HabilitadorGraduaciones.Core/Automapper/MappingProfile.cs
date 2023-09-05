using AutoMapper;
using HabilitadorGraduaciones.Core.DTO;
using HabilitadorGraduaciones.Core.DTO.Base;
using HabilitadorGraduaciones.Core.Entities;
using HabilitadorGraduaciones.Core.Entities.Expediente;
using HabilitadorGraduaciones.Core.Token;

namespace HabilitadorGraduaciones.Core.Automapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<SemanasTecEntity, SemanasTecDto>();
            CreateMap<SemanasTecExtEntity, SemanasTecExtDto>();
            CreateMap<DistincionesEntity, DistincionesDto>();
            CreateMap<ExpedienteEntity, ExpedienteOutDto>();
            CreateMap<NivelInglesEntity, NivelInglesDto>();
            CreateMap<TarjetaEntity, TarjetaDto>();
            CreateMap<ServicioSocialEntity, ServicioSocialDto>();
            CreateMap<CatalogoEntity, CatalogoDto>();
            CreateMap<AvisoGuardar, BaseOutDto>();
            CreateMap<AvisosEntity, AvisosDto>().ReverseMap();
            CreateMap<MenuEntity, MenuOutDto>();
            CreateMap<SinRequisitoExamenEntity, SinRequisitoExamenDto>();
            CreateMap<CenevalEntity, CenevalDto>();
            CreateMap<PeriodosEntity, PeriodosDto>();
            CreateMap<CalendarioEntity, CalendarioDto>();
            CreateMap<AccesosNominaEntity, AccesosNominaDto>();
            CreateMap<PrestamoEducativoEntity, PrestamoEducativoDto>();
            CreateMap<ApiToken, Sesion>();
            CreateMap<Sesion, ApiToken>();
            CreateMap<ExamenConocimientosDto, TipoExamenPorCarreraDto>().ReverseMap();
        }
    }
}