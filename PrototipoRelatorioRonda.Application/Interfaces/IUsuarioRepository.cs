using PrototipoRelatorioRonda.Domain.Interfaces;
﻿using PrototipoRelatorioRonda.Domain.Entities;

namespace PrototipoRelatorioRonda.Application.Interfaces;

public interface IUsuarioRepository : IBaseRepository<Usuario>
{
    Task<bool> EmailExisteAsync(string email, int? idExcluir = null);
    Task<bool> NomeExisteAsync(string nome, int? idExcluir = null);
    Task<bool> EmpresaExisteAsync(int empresaId);
    Task<Usuario?> GetByIdWithEmpresaAsync(int id);
    Task<IEnumerable<Usuario>> GetAllWithEmpresaAsync();
    Task<Usuario?> GetByEmailAsync(string email);
}
