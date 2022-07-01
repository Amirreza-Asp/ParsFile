using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using ParsFile.Application.Contrcats.Persistense.Repositories;
using ParsFile.Infrastructure.Persistence.Data;
using System.Linq.Expressions;

namespace ParsFile.Infrastructure.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {

        protected readonly ApplicationDbContext _db;
        private readonly DbSet<T> _dbSet;

        public Repository(ApplicationDbContext db)
        {
            _db = db;
            _dbSet = _db.Set<T>();
        }

        public virtual void Add(T obj)
        {
            _dbSet.Add(obj);
        }
        public virtual async Task AddAsync(T obj)
        {
            await _dbSet.AddAsync(obj);
        }

        public T Find(object id)
        {
            return _dbSet.Find(id);
        }
        public async Task<T> FindAsync(object id)
        {
            return await _dbSet.FindAsync(id);
        }

        public T FirstOrDefault(
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            bool isTracking = false)
        {
            IQueryable<T> query = _dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }
            if (orderBy != null)
            {
                query = orderBy(query);
            }
            if (include != null)
            {
                query = include(query);
            }
            if (!isTracking)
            {
                query = query.AsNoTracking();
            }
            return query.FirstOrDefault();
        }

        public async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
            bool isTracking = false)
        {
            IQueryable<T> query = _dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }
            if (include != null)
            {
                query = include(query);
            }
            if (!isTracking)
            {
                query = query.AsNoTracking();
            }
            return await query.FirstOrDefaultAsync();
        }

        public IEnumerable<U> GetAll<U>(
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            Expression<Func<T, U>> select = null,
            bool isTracking = false)
        {
            IQueryable<T> query = _dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }
            if (include != null)
            {
                query = include(query);
            }
            if (orderBy != null)
            {
                query = orderBy(query);
            }
            if (!isTracking)
            {
                query = query.AsNoTracking();
            }

            if (select != null)
            {
                return query.Select(select);
            }
            return (IQueryable<U>)query;
        }

        public virtual void Remove(T obj)
        {
            _dbSet.Remove(obj);
        }
        public virtual void RemoveById(object id)
        {
            var entity = Find(id);
            Remove(entity);
        }

        public bool Save()
        {
            try
            {
                _db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public async Task<bool> SaveAsync()
        {
            try
            {
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
