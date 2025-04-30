using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tucson.Domain.Interfaces;

namespace Tucson.Application.Strategies
{
    public class ClassicAdvanceDaysValidationStrategy : IAdvanceDaysValidationStrategy
    {
        public bool CanReserve(DateTime reservationDate, DateTime currentDate)
        {
            return (reservationDate - currentDate).TotalHours <= 48;
        }
    }

    public class GoldAdvanceDaysValidationStrategy : IAdvanceDaysValidationStrategy
    {
        public bool CanReserve(DateTime reservationDate, DateTime currentDate)
        {
            return (reservationDate - currentDate).TotalHours <= 72;
        }
    }

    public class PlatinumAdvanceDaysValidationStrategy : IAdvanceDaysValidationStrategy
    {
        public bool CanReserve(DateTime reservationDate, DateTime currentDate)
        {
            return (reservationDate - currentDate).TotalHours <= 96;
        }
    }

    public class DiamondAdvanceDaysValidationStrategy : IAdvanceDaysValidationStrategy
    {
        public bool CanReserve(DateTime reservationDate, DateTime currentDate)
        {
            return true;
        }
    }
}
