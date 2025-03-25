using PrototipoRelatorioRonda.Data.Interface;
using PrototipoRelatorioRonda.Models;

namespace PrototipoRelatorioRonda.Data.Repositories;

public class UsuarioRepository : BaseRepository<Usuario>, IUsuarioRepository
{
    public UsuarioRepository(RelatorioRondaContext context) : base(context) { }
}
