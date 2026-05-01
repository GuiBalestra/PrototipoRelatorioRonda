using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using PrototipoRelatorioRonda.Data.Repositories;
using PrototipoRelatorioRonda.Data.Interface;

namespace PrototipoRelatorioRonda.Data
{
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
                await _context.SaveChangesAsync();
                await _transaction.CommitAsync();
                _transaction.Dispose();
                _transaction = null;
            }
        }

        public async Task RollbackTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();
                _transaction.Dispose();
                _transaction = null;
            }
        }

        public void Dispose()
        {
            _transaction?.Dispose();
            _context.Dispose();
        }
    }
}
