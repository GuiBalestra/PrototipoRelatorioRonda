using PrototipoRelatorioRonda.Models;

namespace PrototipoRelatorioRonda.Data.Interface;

public interface IUsuarioRepository : IBaseRepository<Usuario>
{
    Task<bool> EmailExisteAsync(string email, int? idExcluir = null);
    Task<bool> NomeExisteAsync(string nome, int? idExcluir = null);
    Task<bool> EmpresaExisteAsync(int empresaId);
    Task<Usuario?> GetByIdWithEmpresaAsync(int id);
    Task<IEnumerable<Usuario>> GetAllWithEmpresaAsync();
}
