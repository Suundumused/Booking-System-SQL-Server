using Microsoft.EntityFrameworkCore;

using System.Linq.Expressions;

using ZConnector.Data;
using ZConnector.Repositories.Interfaces;


namespace ZConnector.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        protected readonly AppDbContext _context;
        protected readonly DbSet<T> _dbSet;


        public BaseRepository(AppDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task AddAsync(T entity) => await _dbSet.AddAsync(entity);
        public void Update(T entity) => _dbSet.Update(entity);

        public async Task DeleteAsync(int id)
        {
            T? entity = await _dbSet.FindAsync(id);

            if (entity != null)
            {
                _dbSet.Remove(entity);
            }
        }

        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate) => await _dbSet.Where(predicate).ToListAsync();
        public async Task<IEnumerable<T>> GetAllAsync() => await _dbSet.ToListAsync();
        public async Task<T?> GetByIdAsync(int id) => await _dbSet.FindAsync(id);
        public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
    }
}
