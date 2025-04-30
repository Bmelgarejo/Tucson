using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tucson.Domain.Entities
{
    public class WaitingList
    {
        public Customer Customer { get; set; }
        public DateTime ReservationDate { get; set; }
        public int SeatCount { get; set; }

        public WaitingList(Customer customer, DateTime reservationDate, int seatCount)
        {
            Customer = customer;
            ReservationDate = reservationDate;
            SeatCount = seatCount;
        }
    }
}
