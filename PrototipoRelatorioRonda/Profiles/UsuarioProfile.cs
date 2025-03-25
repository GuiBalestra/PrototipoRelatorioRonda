using AutoMapper;
using PrototipoRelatorioRonda.Models;
using PrototipoRelatorioRonda.Models.DTO;

namespace PrototipoRelatorioRonda.Profiles;

public class UsuarioProfile : Profile
{
    public UsuarioProfile()
    {

        CreateMap<UsuarioDTO, Usuario>()
            .ForMember(dest => dest.Nome, opt => opt.MapFrom(src => src.Nome))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.EmpresaId, opt => opt.MapFrom(src => src.EmpresaId))
            .ForMember(dest => dest.Funcao, opt => opt.MapFrom(src => src.Funcao))
            .ForMember(dest => dest.Empresa, opt => opt.Ignore())
            .ForMember(dest => dest.HashSenha, opt => opt.Ignore())
            .ForAllMembers(opt => opt.Condition((src, dest, srcMember) =>
                srcMember != null && !string.IsNullOrEmpty(srcMember.ToString())));

        CreateMap<Usuario, UsuarioDTO>()
            .ForMember(dest => dest.Nome, opt => opt.MapFrom(src => src.Nome))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.EmpresaId, opt => opt.MapFrom(src => src.EmpresaId))
            .ForMember(dest => dest.Funcao, opt => opt.MapFrom(src => src.Funcao));
    }
}
