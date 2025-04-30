namespace Tucson.Domain.Interfaces
{
    public interface IAdvanceDaysValidationStrategy
    {
        bool CanReserve(DateTime reservationDate, DateTime currentDate);
    }

}
