using Microsoft.EntityFrameworkCore;
using PrototipoRelatorioRonda.Data.Interface;
using PrototipoRelatorioRonda.Models.Interface;
using System.Linq.Expressions;

namespace PrototipoRelatorioRonda.Data.Repositories;

public class BaseRepository<T> : IBaseRepository<T> where T : class, IAtivavel
{
    protected readonly RelatorioRondaContext _context;

    public BaseRepository(RelatorioRondaContext context)
    {
        _context = context;
    }

    public async Task<T> AddAsync(T entity)
    {
        _context.Set<T>().Add(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task DeleteAsync(T entity)
    {
        _context.Set<T>().Remove(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DesativarAsync(T entity)
    {
        entity.Ativo = false;
        _context.Set<T>().Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _context.Set<T>().ToListAsync();
    }

    public async Task<T?> GetByAsync(Expression<Func<T, bool>> predicado)
    {
        return await _context.Set<T>().FirstOrDefaultAsync(predicado);
    }

    public async Task<T?> GetByIdAsync(int id)
    {
        return await _context.Set<T>().FindAsync(id);
    }

    public async Task UpdateAsync(T entity)
    {
        _context.Set<T>().Update(entity);
        await _context.SaveChangesAsync();
    }
}
