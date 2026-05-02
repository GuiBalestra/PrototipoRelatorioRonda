using Microsoft.EntityFrameworkCore.Storage;
using PrototipoRelatorioRonda.Application.Interfaces;
using PrototipoRelatorioRonda.Domain.Interfaces;

namespace PrototipoRelatorioRonda.Infrastructure.Data;

public class UnitOfWork : IUnitOfWork
{
    private readonly RelatorioRondaContext _context;
    private IDbContextTransaction? _transaction;

    public IEmpresaRepository Empresas { get; }
    public IUsuarioRepository Usuarios { get; }
    public IRelatorioRondaRepository Relatorios { get; }
    public IVoltaRondaRepository Voltas { get; }

    public UnitOfWork(
        RelatorioRondaContext context,
        IEmpresaRepository empresas,
        IUsuarioRepository usuarios,
        IRelatorioRondaRepository relatorios,
        IVoltaRondaRepository voltas)
    {
        _context = context;
        Empresas = empresas;
        Usuarios = usuarios;
        Relatorios = relatorios;
        Voltas = voltas;
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task BeginTransactionAsync()
    {
        _transaction = await _context.Database.BeginTransactionAsync();
    }

    public async Task CommitTransactionAsync()
    {
        if (_transaction != null)
        {
            await _transaction.CommitAsync();
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public async Task RollbackTransactionAsync()
    {
        if (_transaction != null)
        {
            await _transaction.RollbackAsync();
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public void Dispose()
    {
        _transaction?.Dispose();
        _context.Dispose();
    }
}
