using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PromoCodeFactory.Core.Domain.Administration;
using PromoCodeFactory.Core.Domain.PromoCodeManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PromoCodeFactory.DataAccess
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {

        }
        public DbSet<Employee> T_Employees { get; set; }
        public DbSet<Role> T_Roles { get; set; }
        public DbSet<Customer> T_Customers { get; set; }
        public DbSet<Preference> T_Preferences { get; set; }
        public DbSet<PromoCode> T_PromoCodes { get; set; }
        public DbSet<CustomerPreference> T_Customer_Preferences { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<PromoCode>()
                 .HasOne(u => u.Preference);
            modelBuilder.Entity<PromoCode>()
                .HasOne(u => u.PartnerManager);

            modelBuilder.Entity<Employee>()
               .HasOne(u => u.Role);

/*            modelBuilder.Entity<Customer>()
                .Property(c => c.Id)
                .HasConversion(
                    v => v.ToString(),          //Convert to string when writing to DB
                    v => Guid.Parse(v));        //Convert back to Guid when reading*/

            modelBuilder.Entity<CustomerPreference>()
               .HasOne<Preference>(sc => sc.Preference)
               .WithMany(s => s.CustomerPreferences)
               .HasForeignKey(sc => sc.PreferenceId)
               .HasPrincipalKey(c => c.Id); ;

            modelBuilder.Entity<PromoCode>()
               .HasOne<Customer>(s => s.Customer)
               .WithMany(g => g.PromoCodes)
               .HasForeignKey(s => s.CustomerId)
               .HasPrincipalKey(c => c.Id); ;

            modelBuilder.Entity<Customer>()
                .HasMany<PromoCode>(g => g.PromoCodes);

            modelBuilder.Entity<Employee>().Property(c => c.FirstName).HasMaxLength(100).IsRequired();
            modelBuilder.Entity<Employee>().Property(c => c.LastName).HasMaxLength(100).IsRequired();
            modelBuilder.Entity<Employee>().Property(c => c.Email).HasMaxLength(150).IsRequired();

            modelBuilder.Entity<Role>().Property(c => c.Name).HasMaxLength(100).IsRequired();
            modelBuilder.Entity<Role>().Property(c => c.Description).HasMaxLength(100);

            modelBuilder.Entity<Preference>().Property(c => c.Name).HasMaxLength(50);

            modelBuilder.Entity<Customer>().Property(c => c.FirstName).HasMaxLength(100).IsRequired();
            modelBuilder.Entity<Customer>().Property(c => c.LastName).HasMaxLength(100).IsRequired();
            modelBuilder.Entity<Customer>().Property(c => c.Email).HasMaxLength(150).IsRequired();


        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.LogTo(Console.WriteLine, LogLevel.Information);
/*            optionsBuilder
                   .UseLazyLoadingProxies() // lazy loading
                                            //.UseNpgsql(connectionString));
                   .UseSqlite(connectionString));
            //.UseSqlServer(connectionString));*/
        }
    }
}
