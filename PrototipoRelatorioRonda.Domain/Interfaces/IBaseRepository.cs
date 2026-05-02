using PrototipoRelatorioRonda.Domain.Interfaces;
using System.Linq.Expressions;

namespace PrototipoRelatorioRonda.Domain.Interfaces;

public interface IBaseRepository<T> where T : class, IAtivavel
{
    Task<T> AddAsync(T entity);
    Task DeleteAsync(T entity);
    Task DesativarAsync(T entity);
    Task<IEnumerable<T>> GetAllAsync();
    Task<T?> GetByAsync(Expression<Func<T, bool>> predicado);
    Task<T?> GetByIdAsync(int id);
    Task UpdateAsync(T entity);
}
