using AutoMapper;
using PsicoAgenda.Application.Dtos.SesionNota;
using PsicoAgenda.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace PsicoAgenda.Application.Mappers
{
    public class SesionNotaProfile: Profile
    {
        public SesionNotaProfile() {
            CreateMap<SesionNota, SesionNotaRespuesta>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.SesionId, opt => opt.MapFrom(src => src.SesionId))
                .ForMember(dest => dest.Contenido, opt => opt.MapFrom(src => src.Contenido));

            CreateMap<SesionNotaCreacion, SesionNota>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.SesionId, opt => opt.MapFrom(src => src.SesionId))
                .ForMember(dest => dest.Contenido, opt => opt.MapFrom(src => src.Contenido))
                .ForMember(dest => dest.Sesion, opt => opt.Ignore());

            CreateMap<SesionNotaActualizacion, SesionNota>()
                .ForMember(dest => dest.Contenido, opt => opt.MapFrom(src => src.Contenido));
        }
    }
}
