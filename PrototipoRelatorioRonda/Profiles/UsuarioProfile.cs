using AutoMapper;
using PrototipoRelatorioRonda.Models;
using PrototipoRelatorioRonda.Models.DTO;
using System.Security.Cryptography;
using System.Text;

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
            .ForMember(dest => dest.HashSenha, opt => opt.MapFrom(src => GerarHashSenha(src.Senha)))
            .ForMember(dest => dest.Empresa, opt => opt.Ignore())
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Ativo, opt => opt.Ignore())
            .ForMember(dest => dest.CriadoEm, opt => opt.Ignore());

        CreateMap<Usuario, UsuarioDTO>()
            .ForMember(dest => dest.Nome, opt => opt.MapFrom(src => src.Nome))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.EmpresaId, opt => opt.MapFrom(src => src.EmpresaId))
            .ForMember(dest => dest.Funcao, opt => opt.MapFrom(src => src.Funcao))
            .ForMember(dest => dest.Senha, opt => opt.Ignore());
    }

    private static string GerarHashSenha(string senha)
    {
        using var sha256 = SHA256.Create();
        var bytes = Encoding.UTF8.GetBytes(senha);
        var hash = sha256.ComputeHash(bytes);
        return Convert.ToBase64String(hash);
    }
}
