using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tucson.Domain.Entities;

namespace Tucson.Application.Service.Interface
{
    public interface IReservationService
    {
        Task<Reservation> CreateReservationAsync(Customer customer, DateTime reservationDate, int requestedSeatCount);
        Task CancelReservationAsync(int reservationId);
    }
}
