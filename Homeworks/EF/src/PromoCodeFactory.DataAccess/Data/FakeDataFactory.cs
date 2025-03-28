using System;
using System.Collections.Generic;
using System.Linq;
using PromoCodeFactory.Core.Domain.Administration;
using PromoCodeFactory.Core.Domain.PromoCodeManagement;

namespace PromoCodeFactory.DataAccess.Data
{
    public static class FakeDataFactory
    {
        public static IEnumerable<Employee> Employees => new List<Employee>()
        {
            new Employee()
            {
                Id = Guid.Parse("451533d5-d8d5-4a11-9c7b-eb9f14e1a32f"),
                Email = "owner@somemail.ru",
                FirstName = "Иван",
                LastName = "Сергеев",
                Role = Roles.FirstOrDefault(x => x.Name == "Admin"),
                AppliedPromocodesCount = 5
            },
            new Employee()
            {
                Id = Guid.Parse("f766e2bf-340a-46ea-bff3-f1700b435895"),
                Email = "andreev@somemail.ru",
                FirstName = "Петр",
                LastName = "Андреев",
                Role = Roles.FirstOrDefault(x => x.Name == "PartnerManager"),
                AppliedPromocodesCount = 10
            },
        };

        public static IEnumerable<Role> Roles => new List<Role>()
        {
            new Role()
            {
                Id = Guid.NewGuid(),
                Name = "Admin",
                Description = "Администратор",
            },
            new Role()
            {
                Id = Guid.NewGuid(),
                Name = "PartnerManager",
                Description = "Партнерский менеджер"
            }
        };

        public static IEnumerable<Preference> Preferences => new List<Preference>()
        {
            new Preference()
            {
                Id = Guid.Parse("ef7f299f-92d7-459f-896e-078ed53ef99c"),
                Name = "Театр",
            },
            new Preference()
            {
                Id = Guid.Parse("c4bda62e-fc74-4256-a956-4760b3858cbd"),
                Name = "Семья",
            },
            new Preference()
            {
                Id = Guid.Parse("76324c47-68d2-472d-abb8-33cfa8cc0c84"),
                Name = "Дети",
            }
        };

        public static IEnumerable<Customer> Customers
        {
            get
            {
                var customers = new List<Customer>()
                {
                    new Customer()
                    {
                        Id = Guid.Parse("117f299f-92d7-459f-896e-078ed53ef99c"),
                        Email = "ivan_sergeev@mail.ru",
                        FirstName = "Иван",
                        LastName = "Петров",
                        //TODO: Добавить предзаполненный список предпочтений
                    },
                    new Customer()
                    {
                        Id = Guid.Parse("127f299f-92d7-459f-896e-078ed53ef99c"),
                        Email = "ivan_ivanov@mail.ru",
                        FirstName = "Иван",
                        LastName = "Иванов",
                        //TODO: Добавить предзаполненный список предпочтений
                    }
                };

                return customers;
            }
        }
        public static IEnumerable<CustomerPreference> CustomerPreferences
        {
            get
            {
                var customerPreference = new List<CustomerPreference>()
                {
                    new CustomerPreference()
                    {
                        Id = Guid.NewGuid(),
                        CustomerId =  Customers.FirstOrDefault(x => x.LastName == "Петров").Id,
                        PreferenceId =  Preferences.FirstOrDefault(x => x.Name == "Дети").Id
                    },
                    new CustomerPreference()
                    {
                        Id = Guid.NewGuid(),
                        CustomerId =  Customers.FirstOrDefault(x => x.LastName == "Иванов").Id,
                        PreferenceId =  Preferences.FirstOrDefault(x => x.Name == "Театр").Id,
                    },
                };
                return customerPreference;
            }
        }
    }
}