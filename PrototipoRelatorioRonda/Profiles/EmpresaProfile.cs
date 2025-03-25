using AutoMapper;
using PrototipoRelatorioRonda.Models;
using PrototipoRelatorioRonda.Models.DTO;

namespace PrototipoRelatorioRonda.Profiles;

public class EmpresaProfile : Profile
{
    public EmpresaProfile()
    {
        CreateMap<EmpresaDTO, Empresa>();
        CreateMap<Empresa, EmpresaDTO>();
    }
}
