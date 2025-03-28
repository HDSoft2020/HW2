using PromoCodeFactory.Core.Domain.PromoCodeManagement;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PromoCodeFactory.Core.Abstractions.Repositories
{
    public interface ICustomerRepository : IRepository<Customer, Guid>
    {
        Task<Customer> GetCustomer(Guid id);
        Task<Customer> GetCustomerAsync(Guid id);
        Task<List<Customer>> GetCustomerList();
        Task<List<Customer>> GetCustomertListAsync();
        Task AddCustomer(Customer customer);
        Task AddCustomerAsync(Customer customer);
        Task UpateCustomer(Customer customer);
        Task UpateCustomerAsync(Customer customer);
        Task DeleteCustomer(Guid id);
        Task DeleteCustomerAsync(Guid id);

    }
}
