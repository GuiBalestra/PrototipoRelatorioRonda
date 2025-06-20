using AutoMapper;
using PrototipoRelatorioRonda.Models;
using PrototipoRelatorioRonda.Models.DTO;

namespace PrototipoRelatorioRonda.Profiles;

public class EmpresaProfile : Profile
{
    public EmpresaProfile()
    {
        CreateMap<EmpresaDTO, Empresa>()
            .ForMember(dest => dest.Nome, opt => opt.MapFrom(src => src.Nome))
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Ativo, opt => opt.Ignore())
            .ForMember(dest => dest.CriadoEm, opt => opt.Ignore())
            .ForMember(dest => dest.Usuarios, opt => opt.Ignore());

        CreateMap<Empresa, EmpresaDTO>()
            .ForMember(dest => dest.Nome, opt => opt.MapFrom(src => src.Nome));
    }
}
