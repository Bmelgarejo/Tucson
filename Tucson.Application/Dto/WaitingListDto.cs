namespace Tucson.Application.Dto
{
    public class WaitingListDto
    {
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public DateTime ReservationDate { get; set; }
        public int SeatCount { get; set; }
    }
}
