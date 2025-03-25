using PrototipoRelatorioRonda.Data.Interface;
using PrototipoRelatorioRonda.Models.Interface;
using System.Linq.Expressions;

namespace PrototipoRelatorioRonda.Data.Repositories;

public class BaseRepository<T> : IBaseRepository<T> where T : class, IAtivavel
{
    private readonly RelatorioRondaContext _context;

    public BaseRepository(RelatorioRondaContext context)
    {
        _context = context;
    }

    public T Add(T entity)
    {
        _context.Set<T>().Add(entity);
        _context.SaveChanges();
        return entity;
    }

    public void Delete(T entity)
    {
        _context.Set<T>().Remove(entity);
        _context.SaveChanges();
    }

    public void Desativar(T entity)
    {
        entity.Ativo = false;
        _context.Set<T>().Update(entity);
        _context.SaveChanges();
    }

    public IEnumerable<T> GetAll()
    {
        return _context.Set<T>().ToList();
    }

    public T? GetBy(Expression<Func<T, bool>> predicado)
    {
        return _context.Set<T>().FirstOrDefault(predicado);
    }

    public T? GetById(int id)
    {
        return _context.Set<T>().Find(id);
    }

    public void Update(T entity)
    {
        _context.Set<T>().Update(entity);
        _context.SaveChanges();
    }
}
