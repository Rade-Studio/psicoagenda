using AutoMapper;
using PsicoAgenda.Application.Dtos.Pacientes;
using PsicoAgenda.Domain.Models;

namespace PsicoAgenda.Application.Mappers;

public class PacienteProfile : Profile
{
    public PacienteProfile()
    {
        CreateMap<Paciente, PacienteRespuesta>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Nombre, opt => opt.MapFrom(src => src.Nombre))
            .ForMember(dest => dest.Apellidos, opt => opt.MapFrom(src => src.Apellido))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.Telefono, opt => opt.MapFrom(src => src.Telefono))
            .ForMember(dest => dest.ContactoEmergencia, opt => opt.MapFrom(src => src.ContactoEmergencia))
            .ForMember(dest => dest.TagsJson, opt => opt.MapFrom(src => src.TagsJson))
            .ForMember(dest => dest.FechaNacimiento, opt => opt.MapFrom(src => src.FechaNacimiento.ToUniversalTime()));
        
        CreateMap<PacienteCreacion, Paciente>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Nombre, opt => opt.MapFrom(src => src.Nombre))
            .ForMember(dest => dest.Apellido, opt => opt.MapFrom(src => src.Apellidos))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.Telefono, opt => opt.MapFrom(src => src.Telefono))
            .ForMember(dest => dest.ContactoEmergencia, opt => opt.MapFrom(src => src.ContactoEmergencia))
            .ForMember(dest => dest.TagsJson, opt => opt.MapFrom(src => src.TagsJson))
            .ForMember(dest => dest.FechaNacimiento, opt => opt.MapFrom(src => src.FechaNacimiento.ToUniversalTime()));

        CreateMap<PacienteActualizacion, Paciente>()
            .ForMember(dest => dest.Nombre, opt => opt.MapFrom(src => src.Nombre))
            .ForMember(dest => dest.Apellido, opt => opt.MapFrom(src => src.Apellidos))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.Telefono, opt => opt.MapFrom(src => src.Telefono))
            .ForMember(dest => dest.ContactoEmergencia, opt => opt.MapFrom(src => src.ContactoEmergencia))
            .ForMember(dest => dest.TagsJson, opt => opt.MapFrom(src => src.TagsJson))
            .ForMember(dest => dest.FechaNacimiento, opt => opt.MapFrom(src => src.FechaNacimiento.ToUniversalTime()));

    }
    
}