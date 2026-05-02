namespace PrototipoRelatorioRonda.Application.Interfaces;

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
