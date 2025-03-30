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
        private readonly DatabaseContext _context;
        public CustomerRepository(DatabaseContext _db) : base(_db)
        {
            _context = _db;
        }
        //
        //Get
        //
        public async Task<Customer> GetCustomerAsync(Guid id)
        {
            var customer = await _context.T_Customers
               .AsNoTracking()
               .Include(u => u.PromoCodes)
               .Include(u => u.Preferences)
               .FirstOrDefaultAsync(u => u.Id == id);

            return customer;
        }
        //
        //GetList
        //
        public async Task<List<Customer>> GetCustomerListAsync()
        {
            var customers = await _context.T_Customers
               .AsNoTracking()
               .Include(u => u.PromoCodes)
               .Include(u => u.Preferences)
               .ToListAsync();

            return customers;
        }
        //
        //Add
        //
        public async Task AddCustomerAsync(Customer customer)
        {
            await AddAsync(customer);
            await _context.SaveChangesAsync();
        }
        //
        //Update
        //
        public async Task UpateCustomerAsync(Customer customer)
        {
            var find = await _context.T_Customers.FirstOrDefaultAsync(u=>u.Id==customer.Id);
            if (find == null)
            {
                throw new Exception($"Клиент с идентфикатором {customer.Id} не найден");
            }
            find.LastName = customer.LastName;
            find.FirstName = customer.FirstName;
            find.Email = customer.Email;
            await UpdateAsync(find);

            await _context.SaveChangesAsync();
        }
        //
        //Delete
        //
        public async Task DeleteCustomerAsync(Guid id)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var promocodes = await _context.T_PromoCodes.Where(u => u.CustomerId == id).ToListAsync();
                    _context.T_PromoCodes.RemoveRange(promocodes);
                    var preferences = await _context.T_Customer_Preferences.Where(u => u.CustomerId == id).ToListAsync();
                    _context.T_Customer_Preferences.RemoveRange(preferences);
                    var customer = await _context.T_Customers.FirstOrDefaultAsync(u => u.Id == id);
                    _context.T_Customers.Remove(customer);
                    await _context.SaveChangesAsync();
                    transaction.Commit();
                }
                catch (Exception exp)
                {
                    transaction.Rollback();
                }

            }
        }
    }
}