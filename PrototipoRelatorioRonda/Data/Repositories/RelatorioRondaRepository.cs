using PrototipoRelatorioRonda.Data.Interface;
using PrototipoRelatorioRonda.Models;

namespace PrototipoRelatorioRonda.Data.Repositories;

public class RelatorioRondaRepository : BaseRepository<RelatorioRonda>, IRelatorioRondaRepository
{
    public RelatorioRondaRepository(RelatorioRondaContext context) : base(context) { }
}
