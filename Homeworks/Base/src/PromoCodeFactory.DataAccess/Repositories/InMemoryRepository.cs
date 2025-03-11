using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PromoCodeFactory.Core.Abstractions.Repositories;
using PromoCodeFactory.Core.Domain;
using PromoCodeFactory.Core.Domain.Administration;
using PromoCodeFactory.DataAccess.Data;
namespace PromoCodeFactory.DataAccess.Repositories
{
    public class InMemoryRepository<T>: IRepository<T> where T: BaseEntity
    {
        protected IEnumerable<T> Data { get; set; }

        public InMemoryRepository(IEnumerable<T> data)
        {
            Data = data;
        }
        public Task<IEnumerable<T>> GetAllAsync()
        {
            return Task.FromResult(Data);
        }

        public Task<T> GetByIdAsync(Guid id)
        {
            return Task.FromResult(Data.FirstOrDefault(x => x.Id == id));
        }

        public Task Add(T item)
        {
            Data=Data.Append(item);
            return Task.CompletedTask;
        }
        public Task Update(T item)
        {
            var find = Data.FirstOrDefault(x => x.Id == item.Id);
            if (find != null)
            {
                var updated = Data.ToList();
                int index = updated.IndexOf(find);
                if (index != -1)
                {
                    updated[index] = item;
                    Data=updated;
                }
            }
            return Task.CompletedTask;
        }
        public Task Delete(Guid id)
        {
            var find = Data.FirstOrDefault(x => x.Id == id);
            if (find != null)
            {
                var updated = Data.ToList();
                updated.Remove(find);
                Data=updated;
            }
            return Task.CompletedTask;
        }
    }
}