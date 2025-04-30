using System;
using System.Collections.Generic;
using System.Linq;
using Tucson.Domain.Entities;
using Tucson.Domain.Interfaces;
using Tucson.Infrastructure.Data;

namespace Tucson.Infrastructure.Repositories
{
    public class ReservationRepository : IReservationRepository
    {
        private readonly List<Reservation> _reservations = new();
        private readonly List<WaitingList> _waitingList = new();
        private readonly List<Table> _tables;

        public ReservationRepository()
        {
            _tables = TableData.Instance.Tables; 
        }

        public List<Reservation> GetAllReservations()
        {
            return _reservations;
        }

        public List<Reservation> GetReservationsByDate(DateTime date)
        {
            return _reservations.Where(r => r.ReservationDate.Date == date.Date).ToList();
        }

        public void AddReservation(Reservation reservation)
        {
            var table = GetTableById(reservation.TableId);
            if (table == null)
                throw new InvalidOperationException("Table not found.");

            if (!table.IsAvailable)
                throw new InvalidOperationException("Table is already reserved.");

            table.Reserve(); 
            _reservations.Add(reservation);
        }


        public void RemoveReservation(Reservation reservation)
        {
            var table = GetTableById(reservation.TableId);
            if (table == null)
                throw new InvalidOperationException("Table not found.");

            table.Release(); 
            _reservations.Remove(reservation);
        }

        public List<Table> GetAllTables()
        {
            return _tables.ToList();
        }

        public Table GetTableById(int tableId)
        {
            return _tables.FirstOrDefault(t => t.Id == tableId);
        }

        public Customer GetCustomerById(int customerId)
        {
            return CustomerData.Customers.FirstOrDefault(c => c.Id == customerId);
        }

        public List<WaitingList> GetWaitingList()
        {
            return _waitingList;
        }

        public void AddToWaitingList(Customer customer, DateTime reservationDate, int seatCount)
        {
            var waitingListEntry = new WaitingList(customer, reservationDate, seatCount);
            _waitingList.Add(waitingListEntry);
        }

        public void RemoveFromWaitingList(Customer customer, DateTime reservationDate)
        {
            var entryToRemove = _waitingList.FirstOrDefault(w => w.Customer.Id == customer.Id && w.ReservationDate == reservationDate);
            if (entryToRemove != null)
            {
                _waitingList.Remove(entryToRemove);
            }
        }

        public bool IsTableReservedForDate(int tableId, DateTime reservationDate)
        {
            return _reservations.Any(r => r.TableId == tableId && r.ReservationDate.Date == reservationDate.Date);
        }
    }
}
