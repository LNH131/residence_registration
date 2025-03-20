using System;
using System.Collections.Generic;

namespace Resident.Models;

public partial class HeadOfHouseHold
{
    public int HeadOfHouseHoldId { get; set; }

    public int UserId { get; set; }

    public int? HouseholdId { get; set; }

    public DateTime RegisteredDate { get; set; }

    public virtual Household? Household { get; set; }

    public virtual ICollection<Household> Households { get; set; } = new List<Household>();

    public virtual User User { get; set; } = null!;
}
