using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PromoCodeFactory.Core.Domain;

namespace PromoCodeFactory.Core.Abstractions.Repositories
{
    public interface IRepository<T, TPrimaryKey>
        where T : IEntity<TPrimaryKey>
    {
        //Task<IEnumerable<T>> GetAllAsync();
        IQueryable<T> GetAll(bool noTracking = false);
        Task<List<T>> GetAllAsync(CancellationToken cancellationToken, bool asNoTracking = false);
        T Get(TPrimaryKey id);
        Task<T> GetAsync(TPrimaryKey id, CancellationToken cancellationToken);
        bool Delete(TPrimaryKey id);
        bool DeleteRange(ICollection<T> entities);
        void Update(T entity);
        T Add(T entity);
        Task<T> AddAsync(T entity);
        void AddRange(List<T> entities);
        Task AddRangeAsync(ICollection<T> entities);
        void SaveChanges();
        Task SaveChangesAsync(CancellationToken cancellationToken = default);
        //Task<T> GetByIdAsync(Guid id);
    }
}