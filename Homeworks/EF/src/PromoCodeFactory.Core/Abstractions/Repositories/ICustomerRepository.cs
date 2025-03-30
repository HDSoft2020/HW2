using PromoCodeFactory.Core.Domain.PromoCodeManagement;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PromoCodeFactory.Core.Abstractions.Repositories
{
    public interface ICustomerRepository : IRepository<Customer, Guid>
    {
        Task<Customer> GetCustomerAsync(Guid id);
        Task<List<Customer>> GetCustomerListAsync();
        Task AddCustomerAsync(Customer customer);
        Task UpateCustomerAsync(Customer customer);
        Task DeleteCustomerAsync(Guid id);

    }
}
