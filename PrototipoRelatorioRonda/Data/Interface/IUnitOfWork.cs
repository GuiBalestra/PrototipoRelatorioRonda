using System;
using System.Threading;
using System.Threading.Tasks;

namespace PrototipoRelatorioRonda.Data.Interface
{
    public interface IUnitOfWork : IDisposable
    {
        IEmpresaRepository Empresas { get; }
        IUsuarioRepository Usuarios { get; }
        IRelatorioRondaRepository Relatorios { get; }
        IVoltaRondaRepository Voltas { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
    }
}
