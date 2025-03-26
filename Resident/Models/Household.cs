using System;
using System.Collections.Generic;

namespace Resident.Models;

public partial class Household
{
    public int HouseholdId { get; set; }

    public int AddressId { get; set; }

    public DateOnly? CreatedDate { get; set; }

    public virtual Address Address { get; set; } = null!;

    public virtual HeadOfHouseHold? HeadOfHouseHold { get; set; }

    public virtual ICollection<HouseholdMember> HouseholdMembers { get; set; } = new List<HouseholdMember>();

    public virtual ICollection<HouseholdSeparation> HouseholdSeparationNewHouseholds { get; set; } = new List<HouseholdSeparation>();

    public virtual ICollection<HouseholdSeparation> HouseholdSeparationOriginalHouseholds { get; set; } = new List<HouseholdSeparation>();

    public virtual ICollection<HouseholdTransfer> HouseholdTransfers { get; set; } = new List<HouseholdTransfer>();
}
