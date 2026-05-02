using PrototipoRelatorioRonda.Domain.Entities;
using PrototipoRelatorioRonda.Application.DTOs;

namespace PrototipoRelatorioRonda.Application.Interfaces;

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
