using System;
using System.Collections.Generic;

namespace Project.Models;

public partial class Household
{
    public int HouseholdId { get; set; }

    public int HeadId { get; set; }

    public int AddressId { get; set; }

    public DateOnly? CreatedDate { get; set; }

    public virtual Address Address { get; set; } = null!;

    public virtual HeadOfHouseHold Head { get; set; } = null!;

    public virtual ICollection<HeadOfHouseHold> HeadOfHouseHolds { get; set; } = new List<HeadOfHouseHold>();

    public virtual ICollection<HouseholdMember> HouseholdMembers { get; set; } = new List<HouseholdMember>();

    public virtual ICollection<HouseholdTransfer> HouseholdTransfers { get; set; } = new List<HouseholdTransfer>();
}
