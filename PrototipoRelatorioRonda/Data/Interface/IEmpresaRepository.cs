using PrototipoRelatorioRonda.Models;

namespace PrototipoRelatorioRonda.Data.Interface;

public interface IEmpresaRepository : IBaseRepository<Empresa>
{
    Task<bool> NomeExisteAsync(string nome, int? idExcluir = null);
}
