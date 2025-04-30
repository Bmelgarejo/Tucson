using Tucson.Domain.Entities;
using Tucson.Domain.Interfaces;
using Tucson.Domain.ValueObject;

namespace Tucson.Application.Strategies
{
    namespace Tucson.Application.Strategies
    {
        public class TableAssignmentContext
        {
            private readonly ITableAssignmentStrategy _strategy;

            public TableAssignmentContext(MembershipCategoryEnum membershipCategory)
            {
                _strategy = membershipCategory switch
                {
                    MembershipCategoryEnum.Diamond => new DiamondTableAssignmentStrategy(),
                    _ => new NonDiamondTableAssignmentStrategy()
                };
            }

            public Table AssignTable(List<Table> availableTables, List<Reservation> reservations, DateTime reservationDate)
            {
                return _strategy.AssignTable(availableTables, reservations, reservationDate);
            }
        }
    }

}
