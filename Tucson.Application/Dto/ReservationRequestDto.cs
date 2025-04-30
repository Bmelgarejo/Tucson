using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tucson.Application.Dto
{
    public class ReservationRequestDto
    {
        public int CustomerId { get; set; }
        public DateTime ReservationDate { get; set; }
        public int SeatCount { get; set; } 
    }

}
