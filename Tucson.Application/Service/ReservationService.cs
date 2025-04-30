using Tucson.Application.Service.Interface;
using Tucson.Application.Strategies.Contexto;
using Tucson.Application.Strategies.Tucson.Application.Strategies;
using Tucson.Domain.Entities;
using Tucson.Domain.Interfaces;
using Tucson.Domain.ValueObject;

namespace Tucson.Application.Service
{
    public class ReservationService : IReservationService
    {
        private readonly IReservationRepository _reservationRepository;

        public ReservationService(IReservationRepository reservationRepository)
        {
            _reservationRepository = reservationRepository;
        }
        public async Task<Reservation> CreateReservationAsync(Customer customer, DateTime reservationDate, int requestedSeatCount)
        {
            ValidateReservationDate(customer, reservationDate);

            var availableTables = GetAvailableTablesForDate(reservationDate, requestedSeatCount);

            if (!availableTables.Any())
            {
                AddToWaitingList(customer, reservationDate, requestedSeatCount);
                throw new InvalidOperationException("No available table. Customer added to the waiting list.");
            }

            var assignedTable = AssignTableToCustomer(customer, availableTables, reservationDate);

            if (assignedTable == null)
            {
                AddToWaitingList(customer, reservationDate, requestedSeatCount);
                throw new InvalidOperationException("No available table. Customer added to the waiting list.");
            }

            return CreateAndSaveReservation(customer, assignedTable, reservationDate);
        }

        private void ValidateReservationDate(Customer customer, DateTime reservationDate)
        {
            var validationContext = new AdvanceDaysValidationContext(customer.MembershipCategory);
            if (!validationContext.CanReserve(reservationDate, DateTime.Today))
            {
                throw new InvalidOperationException("Customer cannot make a reservation for the selected date.");
            }
        }

        private List<Table> GetAvailableTablesForDate(DateTime reservationDate, int requestedSeatCount)
        {
            // Obtengo todas las mesas y filtro por su disponibilidad
            var tables = _reservationRepository.GetAllTables()
                .Where(t => !_reservationRepository.IsTableReservedForDate(t.Id, reservationDate))
                .ToList();

            // Filtro las mesas que tienen capacidad suficiente para el nuumero de asientos que se piden
            return tables
                .Where(t => t.SeatCount >= requestedSeatCount)
                .ToList();
        }

        private void AddToWaitingList(Customer customer, DateTime reservationDate, int requestedSeatCount)
        {
            _reservationRepository.AddToWaitingList(customer, reservationDate, requestedSeatCount);
        }

        private Table AssignTableToCustomer(Customer customer, List<Table> availableTables, DateTime reservationDate)
        {
            var assignmentContext = new TableAssignmentContext(customer.MembershipCategory);
            return assignmentContext.AssignTable(availableTables, _reservationRepository.GetAllReservations(), reservationDate);
        }

        private Reservation CreateAndSaveReservation(Customer customer, Table assignedTable, DateTime reservationDate)
        {
            var reservation = new Reservation(
                _reservationRepository.GetAllReservations().Count + 1,
                customer,
                assignedTable.Id,
                reservationDate
            );

            _reservationRepository.AddReservation(reservation);
            return reservation;
        }

        public async Task CancelReservationAsync(int reservationId)
        {
            // Buscar la reserva a cancelar
            var reservation = _reservationRepository.GetAllReservations().FirstOrDefault(r => r.Id == reservationId);

            if (reservation == null)
            {
                throw new InvalidOperationException("Reservation not found.");
            }

            // Liberar la mesa asociada a la reserva
            var table = _reservationRepository.GetTableById(reservation.TableId);
            table.Release();

            // Eliminar la reserva del repositorio
            _reservationRepository.RemoveReservation(reservation);

            // Buscar el primer cliente en la lista de espera que coincida con la fecha y la cantidad de comensales
            var waitingCustomer = _reservationRepository.GetWaitingList()
                .Where(w => w.ReservationDate == reservation.ReservationDate && w.SeatCount <= table.SeatCount)
                .OrderByDescending(w => w.Customer.MembershipCategory)
                .ThenBy(w => w.ReservationDate)
                .FirstOrDefault();

            if (waitingCustomer != default)
            {
                try
                {
                    // Crear una nueva reserva para el cliente encontrado
                    await CreateReservationAsync(waitingCustomer.Customer, waitingCustomer.ReservationDate, waitingCustomer.SeatCount);

                    // Eliminar al cliente de la lista de espera
                    _reservationRepository.RemoveFromWaitingList(waitingCustomer.Customer, waitingCustomer.ReservationDate);
                }
                catch (InvalidOperationException)
                {
                }
            }
        }


    }
}
