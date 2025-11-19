using AutoMapper;
using PsicoAgenda.Application.Dtos.Sesiones;
using PsicoAgenda.Domain.Models;

namespace PsicoAgenda.Application.Mappers;

public class SesionProfile : Profile
{
    public SesionProfile()
    {
        CreateMap<Sesion, SesionRespuesta>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.PacienteId, opt => opt.MapFrom(src => src.PacienteId))
            .ForMember(dest => dest.CitaId, opt => opt.MapFrom(src => src.CitaId))
            .ForMember(dest => dest.SoapSubj, opt => opt.MapFrom(src => src.SoapSubj))
            .ForMember(dest => dest.Observaciones, opt => opt.MapFrom(src => src.Observaciones))
            .ForMember(dest => dest.Analasis, opt => opt.MapFrom(src => src.Analasis))
            .ForMember(dest => dest.PlanAccion, opt => opt.MapFrom(src => src.PlanAccion))
            .ForMember(dest => dest.ArchivosJson, opt => opt.MapFrom(src => src.ArchivosJson))
            // Map session notes to a list of strings (contenido)
            .ForMember(dest => dest.Notas, opt => opt.MapFrom(src => src.Notas.Select(n => n.Contenido).ToList()))
            .ForMember(dest => dest.Paciente, opt => opt.MapFrom(src => src.Paciente))
            .ForMember(dest => dest.Cita, opt => opt.MapFrom(src => src.Cita));

        CreateMap<SesionCreacion, Sesion>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.PacienteId, opt => opt.MapFrom(src => src.PacienteId))
            .ForMember(dest => dest.CitaId, opt => opt.MapFrom(src => src.CitaId))
            .ForMember(dest => dest.SoapSubj, opt => opt.MapFrom(src => src.SoapSubj))
            .ForMember(dest => dest.Observaciones, opt => opt.MapFrom(src => src.Observaciones))
            .ForMember(dest => dest.Analasis, opt => opt.MapFrom(src => src.Analasis))
            .ForMember(dest => dest.PlanAccion, opt => opt.MapFrom(src => src.PlanAccion))
            .ForMember(dest => dest.ArchivosJson, opt => opt.MapFrom(src => src.ArchivosJson))
            // Notas are managed via SesionNota service; ignore automatic mapping
            .ForMember(dest => dest.Notas, opt => opt.Ignore());

        CreateMap<SesionActualizacion, Sesion>()
            .ForMember(dest => dest.PacienteId, opt => opt.MapFrom(src => src.PacienteId))
            .ForMember(dest => dest.SoapSubj, opt => opt.MapFrom(src => src.SoapSubj))
            .ForMember(dest => dest.Observaciones, opt => opt.MapFrom(src => src.Observaciones))
            .ForMember(dest => dest.Analasis, opt => opt.MapFrom(src => src.Analasis))
            .ForMember(dest => dest.PlanAccion, opt => opt.MapFrom(src => src.PlanAccion))
            // Notas en la actualización son cadenas; la gestión de notas se hace con SesionNotaService
            .ForMember(dest => dest.Notas, opt => opt.Ignore());
    }
}

