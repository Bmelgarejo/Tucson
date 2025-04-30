using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tucson.Domain.Entities;
using Tucson.Domain.ValueObject;

namespace Tucson.Infrastructure.Data
{
    public static class CustomerData
    {
        public static readonly List<Customer> Customers = new List<Customer>
        {
            new Customer(1, "Brisa Melgarejo", MembershipCategoryEnum.Classic),
            new Customer(2, "Emiliano Barbarroja", MembershipCategoryEnum.Gold),
            new Customer(3, "Juan Carlos Rodriguez", MembershipCategoryEnum.Platinum),
            new Customer(4, "Melanie Nuñez", MembershipCategoryEnum.Diamond)
        };
    }
}
