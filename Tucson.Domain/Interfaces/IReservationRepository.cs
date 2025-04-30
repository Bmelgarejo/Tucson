using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tucson.Domain.Entities;

namespace Tucson.Domain.Interfaces
{
    public interface IReservationRepository
    {
        List<Reservation> GetAllReservations();
        List<Reservation> GetReservationsByDate(DateTime date);
        void AddReservation(Reservation reservation);
        void RemoveReservation(Reservation reservation);

        List<Table> GetAllTables();
        Table GetTableById(int tableId);

        Customer GetCustomerById(int customerId);

        List<WaitingList> GetWaitingList();
        void AddToWaitingList(Customer customer, DateTime reservationDate, int seatCount);

        void RemoveFromWaitingList(Customer customer, DateTime reservationDate);

        bool IsTableReservedForDate(int tableId, DateTime reservationDate);
    }
}
