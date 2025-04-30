namespace Tucson.Domain.Entities
{
    public class Reservation
    {
        public int Id { get; private set; }
        public Customer Customer { get; private set; }
        public int TableId { get; private set; }
        public DateTime ReservationDate { get; private set; }

        public Reservation(int id, Customer customer, int tableId, DateTime reservationDate)
        {
            if (!customer.CanReserve(reservationDate))
                throw new InvalidOperationException("Customer cannot reserve for the selected date.");

            Id = id;
            Customer = customer ?? throw new ArgumentNullException(nameof(customer));
            TableId = tableId;
            ReservationDate = reservationDate;
        }
    }
}


