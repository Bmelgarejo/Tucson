namespace Tucson.Domain.Entities
{
    public class Table
    {
        public int Id { get; private set; }
        public int SeatCount { get; private set; }
        public bool IsAvailable { get; private set; }

        public Table(int id, int seatCount)
        {
            Id = id;
            SeatCount = seatCount;
            IsAvailable = true;
        }

        public void Reserve()
        {
            IsAvailable = false;
        }

        public void Release()
        {
            IsAvailable = true;
        }

    }
}
