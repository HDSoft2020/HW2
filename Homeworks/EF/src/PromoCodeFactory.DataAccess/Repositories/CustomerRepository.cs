using PromoCodeFactory.Core.Abstractions.Repositories;
using PromoCodeFactory.Core.Domain.PromoCodeManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Threading;

namespace PromoCodeFactory.DataAccess.Repositories
{
    public class CustomerRepository : EfRepository<Customer, Guid>, ICustomerRepository
    {

        public CustomerRepository(DatabaseContext _db) : base(_db)
        {

        }
        //
        //Get
        //
        public Task<Customer> GetCustomer(Guid id)
        {
            return Task.FromResult<Customer>(Get(id));
        }
        public async Task<Customer> GetCustomerAsync(Guid id)
        {
            return await GetAsync(id, CancellationToken.None);
        }

        //
        //GetList
        //
        public Task<List<Customer>> GetCustomerList()
        {
            return Task.FromResult(GetAll(true).ToList());
        }

        public async Task<List<Customer>> GetCustomertListAsync()
        {
            return await GetAllAsync(CancellationToken.None, true);
        }

        //
        //Add
        //
        public Task AddCustomer(Customer customer)
        {
            Add(customer);
            return Task.CompletedTask;
        }

        public async Task AddCustomerAsync(Customer customer)
        {
            await AddAsync(customer);
        }

        //
        //Update
        //
        public Task UpateCustomer(Customer customer)
        {
            Update(customer);
            return Task.CompletedTask;
        }

        public async Task UpateCustomerAsync(Customer customer)
        {
            await UpdateAsync(customer);
        }


        //
        //Delete
        //
        public Task DeleteCustomer(Guid id)
        {
            Delete(id);
            return Task.CompletedTask;
        }

        public async Task DeleteCustomerAsync(Guid id)
        {
            await DeleteAsync(id);
        }






        
    }
}
