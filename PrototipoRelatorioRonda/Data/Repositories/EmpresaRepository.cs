using PrototipoRelatorioRonda.Data.Interface;
using PrototipoRelatorioRonda.Models;

namespace PrototipoRelatorioRonda.Data.Repositories;

public class EmpresaRepository : BaseRepository<Empresa>, IEmpresaRepository
{
    public EmpresaRepository(RelatorioRondaContext context) : base(context) { }
}
