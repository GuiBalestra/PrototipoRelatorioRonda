using PrototipoRelatorioRonda.Domain.Interfaces;
﻿using PrototipoRelatorioRonda.Domain.Entities;

namespace PrototipoRelatorioRonda.Application.Interfaces;

public interface IEmpresaRepository : IBaseRepository<Empresa>
{
    Task<bool> NomeExisteAsync(string nome, int? idExcluir = null);
}
