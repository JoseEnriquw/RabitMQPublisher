
using System.Linq.Expressions;

namespace Core.Common.Interfaces
{
    public interface IRepositoryEF
    {
        void Insert<T>(T entity) where T : class;
        void Update<T>(T entity) where T : class;
        void Delete<T>(T entity) where T : class;

        Task SaveChangesAsync();

        Task<List<T>> GetAllAsync<T>() where T : class;

        Task<T> FindAsync<T>(Expression<Func<T, bool>> func) where T : class;
    }
}
