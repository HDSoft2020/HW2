using PromoCodeFactory.Core.Abstractions.Repositories;
using PromoCodeFactory.Core.Domain;
using System;
using System.Collections.Generic;

namespace PromoCodeFactory.Core.Domain.PromoCodeManagement
{
    public class Customer: IEntity<Guid>
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string FullName => $"{FirstName} {LastName}";

        public string Email { get; set; }

        //TODO: Списки Preferences и Promocodes 
        public virtual IList<CustomerPreference> Preferences { get; set; }
        public virtual ICollection<PromoCode> PromoCodes { get; set; }
    }
}