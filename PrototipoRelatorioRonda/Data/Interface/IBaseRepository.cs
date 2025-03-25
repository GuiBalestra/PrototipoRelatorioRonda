using System.Linq.Expressions;

namespace PrototipoRelatorioRonda.Data.Interface
{
    public interface IBaseRepository<T> where T : class
    {
        IEnumerable<T> GetAll();
        T? GetById(int id);
        T Add(T entity);
        void Update(T entity);
        void Delete(T entity);
        void Desativar(T entity);
        T? GetBy(Expression<Func<T, bool>> predicado);
    }
}
