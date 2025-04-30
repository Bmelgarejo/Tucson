using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tucson.Domain.Interfaces;
using Tucson.Domain.ValueObject;

namespace Tucson.Application.Strategies.Contexto
{
    public class AdvanceDaysValidationContext
    {
        private readonly IAdvanceDaysValidationStrategy _strategy;

        public AdvanceDaysValidationContext(MembershipCategoryEnum membershipCategory)
        {
            _strategy = membershipCategory switch
            {
                MembershipCategoryEnum.Classic => new ClassicAdvanceDaysValidationStrategy(),
                MembershipCategoryEnum.Gold => new GoldAdvanceDaysValidationStrategy(),
                MembershipCategoryEnum.Platinum => new PlatinumAdvanceDaysValidationStrategy(),
                MembershipCategoryEnum.Diamond => new DiamondAdvanceDaysValidationStrategy(),
                _ => throw new ArgumentException("Invalid membership category")
            };
        }

        public bool CanReserve(DateTime reservationDate, DateTime currentDate)
        {
            return _strategy.CanReserve(reservationDate, currentDate);
        }
    }
}
