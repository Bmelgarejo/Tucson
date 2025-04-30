using Tucson.Domain.Entities;
using Tucson.Domain.Interfaces;
using Tucson.Domain.ValueObject;

public class DiamondTableAssignmentStrategy : ITableAssignmentStrategy
{
    public Table AssignTable(List<Table> availableTables, List<Reservation> reservations, DateTime reservationDate)
    {
        return availableTables.FirstOrDefault();
    }
}

public class NonDiamondTableAssignmentStrategy : ITableAssignmentStrategy
{
    public Table AssignTable(List<Table> availableTables, List<Reservation> reservations, DateTime reservationDate)
    {
        var diamondReservations = reservations
            .Where(r => r.Customer.MembershipCategory == MembershipCategoryEnum.Diamond && r.ReservationDate == reservationDate)
            .Select(r => r.TableId)
            .ToList();

        var nonDiamondTables = availableTables.Where(t => !diamondReservations.Contains(t.Id)).ToList();

        return nonDiamondTables.FirstOrDefault();
    }
}
