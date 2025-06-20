using AutoMapper;
using PrototipoRelatorioRonda.Models;
using PrototipoRelatorioRonda.Models.DTO;

namespace PrototipoRelatorioRonda.Profiles;

public class VoltaRondaProfile : Profile
{
    public VoltaRondaProfile()
    {
        CreateMap<VoltaRondaDTO, VoltaRonda>()
            .ForMember(dest => dest.RelatorioRondaId, opt => opt.MapFrom(src => src.RelatorioRondaId))
            .ForMember(dest => dest.NumeroVolta, opt => opt.MapFrom(src => src.NumeroVolta))
            .ForMember(dest => dest.HoraSaida, opt => opt.MapFrom(src => src.HoraSaida))
            .ForMember(dest => dest.HoraChegada, opt => opt.MapFrom(src => src.HoraChegada))
            .ForMember(dest => dest.HoraDescanso, opt => opt.MapFrom(src => src.HoraDescanso))
            .ForMember(dest => dest.Observacoes, opt => opt.MapFrom(src => src.Observacoes))
            .ForMember(dest => dest.RelatorioRonda, opt => opt.Ignore())
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Ativo, opt => opt.Ignore())
            .ForMember(dest => dest.CriadoEm, opt => opt.Ignore());

        CreateMap<VoltaRonda, VoltaRondaDTO>()
            .ForMember(dest => dest.RelatorioRondaId, opt => opt.MapFrom(src => src.RelatorioRondaId))
            .ForMember(dest => dest.NumeroVolta, opt => opt.MapFrom(src => src.NumeroVolta))
            .ForMember(dest => dest.HoraSaida, opt => opt.MapFrom(src => src.HoraSaida))
            .ForMember(dest => dest.HoraChegada, opt => opt.MapFrom(src => src.HoraChegada))
            .ForMember(dest => dest.HoraDescanso, opt => opt.MapFrom(src => src.HoraDescanso))
            .ForMember(dest => dest.Observacoes, opt => opt.MapFrom(src => src.Observacoes));
    }
} 
