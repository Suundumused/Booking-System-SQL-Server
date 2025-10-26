﻿using System.Linq.Expressions;


namespace ZConnector.Repositories.Interfaces
{
    public interface IBaseRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T?> GetByIdAsync(int id);

        Task AddAsync(T entity);
        void Update(T entity);
        Task DeleteAsync(int id);

        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);

        Task SaveChangesAsync();
        Task ReloadChangesAsync(T entity);
    }
}
