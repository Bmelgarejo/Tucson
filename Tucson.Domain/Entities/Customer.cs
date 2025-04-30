using Tucson.Domain.ValueObject;

public class Customer
{
    public int Id { get; private set; }
    public string Name { get; private set; }
    public MembershipCategoryEnum MembershipCategory { get; private set; }

    public Customer(int id, string name, MembershipCategoryEnum membershipCategory)
    {
        Id = id;
        Name = name;
        MembershipCategory = membershipCategory;
    }

    public bool CanReserve(DateTime reservationDate)
    {
        var maxAdvanceDays = GetMaxAdvanceDays();
        var daysInAdvance = (reservationDate.Date - DateTime.Today).TotalDays;
        return daysInAdvance <= maxAdvanceDays;
    }

    public int GetMaxAdvanceDays()
    {
        return MembershipCategory switch
        {
            MembershipCategoryEnum.Classic => 2,
            MembershipCategoryEnum.Gold => 3,
            MembershipCategoryEnum.Platinum => 4,
            MembershipCategoryEnum.Diamond => 365,
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}
