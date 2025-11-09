
using AutoMapper;
using PsicoAgenda.Application.Dtos.Citas;
using PsicoAgenda.Domain.Models;

namespace PsicoAgenda.Application.Mappers
{
    public class CitaProfile: Profile
    {
        public CitaProfile()
        {
            CreateMap<Cita, CitaRespuesta>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.FechaInicio, opt => opt.MapFrom(src => src.FechaInicio))
                .ForMember(dest => dest.FechaFin, opt => opt.MapFrom(src => src.FechaFin))
                .ForMember(dest => dest.Modo, opt => opt.MapFrom(src => src.Modo))
                .ForMember(dest => dest.Estado, opt => opt.MapFrom(src => src.Estado))
                .ForMember(dest => dest.UbicacionLink, opt => opt.MapFrom(src => src.UbicacionLink))
                .ForMember(dest => dest.Notas, opt => opt.MapFrom(src => src.Notas));

            CreateMap<CitaCreacion, Cita>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.PacienteId, opt => opt.MapFrom(src => src.PacienteId))
                .ForMember(dest => dest.FechaInicio, opt => opt.MapFrom(src => src.FechaInicio))
                .ForMember(dest => dest.FechaFin, opt => opt.MapFrom(src => src.FechaFin))
                .ForMember(dest => dest.Modo, opt => opt.MapFrom(src => src.Modo))
                .ForMember(dest => dest.Estado, opt => opt.MapFrom(src => src.Estado))
                .ForMember(dest => dest.UbicacionLink, opt => opt.MapFrom(src => src.UbicacionLink))
                .ForMember(dest => dest.Notas, opt => opt.MapFrom(src => src.Notas));

            CreateMap<CitaActualizacion, Cita>()
                .ForMember(dest => dest.PacienteId, opt => opt.MapFrom(src => src.PacienteId))
                .ForMember(dest => dest.FechaInicio, opt => opt.MapFrom(src => src.FechaInicio))
                .ForMember(dest => dest.FechaFin, opt => opt.MapFrom(src => src.FechaFin))
                .ForMember(dest => dest.Modo, opt => opt.MapFrom(src => src.Modo))
                .ForMember(dest => dest.Estado, opt => opt.MapFrom(src => src.Estado))
                .ForMember(dest => dest.UbicacionLink, opt => opt.MapFrom(src => src.UbicacionLink))
                .ForMember(dest => dest.Notas, opt => opt.MapFrom(src => src.Notas));
        }
    }
}
