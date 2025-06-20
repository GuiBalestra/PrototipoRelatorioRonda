using AutoMapper;
using PrototipoRelatorioRonda.Models;
using PrototipoRelatorioRonda.Models.DTO;

namespace PrototipoRelatorioRonda.Profiles;

public class RelatorioRondaProfile : Profile
{
    public RelatorioRondaProfile()
    {
        CreateMap<RelatorioRondaDTO, RelatorioRonda>()
            .ForMember(dest => dest.EmpresaId, opt => opt.MapFrom(src => src.EmpresaId))
            .ForMember(dest => dest.VigilanteId, opt => opt.MapFrom(src => src.VigilanteId))
            .ForMember(dest => dest.Data, opt => opt.MapFrom(src => src.Data))
            .ForMember(dest => dest.KmSaida, opt => opt.MapFrom(src => src.KmSaida))
            .ForMember(dest => dest.KmChegada, opt => opt.MapFrom(src => src.KmChegada))
            .ForMember(dest => dest.TestemunhaSaida, opt => opt.MapFrom(src => src.TestemunhaSaida))
            .ForMember(dest => dest.TestemunhaChegada, opt => opt.MapFrom(src => src.TestemunhaChegada))
            .ForMember(dest => dest.Empresa, opt => opt.Ignore())
            .ForMember(dest => dest.Vigilante, opt => opt.Ignore())
            .ForMember(dest => dest.Voltas, opt => opt.Ignore())
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Ativo, opt => opt.Ignore())
            .ForMember(dest => dest.CriadoEm, opt => opt.Ignore());

        CreateMap<RelatorioRonda, RelatorioRondaDTO>()
            .ForMember(dest => dest.EmpresaId, opt => opt.MapFrom(src => src.EmpresaId))
            .ForMember(dest => dest.VigilanteId, opt => opt.MapFrom(src => src.VigilanteId))
            .ForMember(dest => dest.Data, opt => opt.MapFrom(src => src.Data))
            .ForMember(dest => dest.KmSaida, opt => opt.MapFrom(src => src.KmSaida))
            .ForMember(dest => dest.KmChegada, opt => opt.MapFrom(src => src.KmChegada))
            .ForMember(dest => dest.TestemunhaSaida, opt => opt.MapFrom(src => src.TestemunhaSaida))
            .ForMember(dest => dest.TestemunhaChegada, opt => opt.MapFrom(src => src.TestemunhaChegada));
    }
} 
