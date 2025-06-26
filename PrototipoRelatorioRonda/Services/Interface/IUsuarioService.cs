using PrototipoRelatorioRonda.Models;
using PrototipoRelatorioRonda.Models.DTO;

namespace PrototipoRelatorioRonda.Services.Interface;

public interface IUsuarioService
{
    Task<IEnumerable<Usuario>> GetAllAsync();
    Task<Usuario?> GetByIdAsync(int id);
    Task<Usuario> CreateAsync(UsuarioDTO usuarioDto);
    Task UpdateAsync(int id, UsuarioDTO usuarioDto);
    Task DeleteAsync(int id);
    Task DesativarAsync(int id);
    Task<IEnumerable<Usuario>> GetAllWithEmpresaAsync();
    Task<Usuario?> GetByIdWithEmpresaAsync(int id);
} 
