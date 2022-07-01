using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace ParsFile.Application.Contrcats.Persistense.Repositories
{
    public interface IRepository<T> where T : class
    {
        T Find(object id);
        Task<T> FindAsync(object id);

        IEnumerable<U> GetAll<U>
            (
                Expression<Func<T, bool>> filter = null,
                Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
                Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
                Expression<Func<T, U>> select = null,
                bool isTracking = false
            );

        T FirstOrDefault
            (
                Expression<Func<T, bool>> filter = null,
                Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
                Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
                bool isTracking = false
            );
        Task<T> FirstOrDefaultAsync
            (
                Expression<Func<T, bool>> filter = null,
                Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
                bool isTracking = false
            );

        void Remove(T obj);
        void RemoveById(object id);

        void Add(T obj);
        Task AddAsync(T obj);

        bool Save();
        Task<bool> SaveAsync();
    }
}
