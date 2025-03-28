using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PromoCodeFactory.DataAccess;
using PromoCodeFactory.DataAccess.Data;
using System;
using System.Linq;

namespace PromoCodeFactory.WebHost
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            using (var scope = host.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

                db.Database.EnsureDeletedAsync();
                db.Database.EnsureCreatedAsync();
                Seed(scope.ServiceProvider);
            }
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => 
                { 
                    webBuilder.UseStartup<Startup>();
                    webBuilder.ConfigureAppConfiguration((hostingContext, config) =>
                    {
                    });
                });

        //Предзаполнение БД из FakeDataFactory
        public static void Seed(IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var _db = scope.ServiceProvider.GetService<DatabaseContext>();
                foreach (var role in FakeDataFactory.Roles)
                    _db.T_Roles.AddAsync(role);
                _db.SaveChanges();
            }
            using (var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var _db = scope.ServiceProvider.GetService<DatabaseContext>();
                foreach (var preference in FakeDataFactory.Preferences)
                    _db.T_Preferences.Add(preference);
                _db.SaveChanges();
            }
            using (var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var _db = scope.ServiceProvider.GetService<DatabaseContext>();
                foreach (var customer in FakeDataFactory.Customers)
                    _db.T_Customers.Add(customer);
                _db.SaveChanges();

            }
            using (var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var _db = scope.ServiceProvider.GetService<DatabaseContext>();
                foreach (var employee in FakeDataFactory.Employees)
                    _db.T_Employees.Add(employee);
                _db.SaveChanges();

            }
            using (var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var _db = scope.ServiceProvider.GetService<DatabaseContext>();
                var cust = _db.T_Customers.ToList();
                var pref = _db.T_Preferences.ToList();
                foreach (var customerPreference in FakeDataFactory.CustomerPreferences)
                {
                    _db.T_Customer_Preferences.Add(customerPreference);
                }              
                _db.SaveChanges();
            }
        }
    }
}