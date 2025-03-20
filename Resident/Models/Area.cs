using System;
using System.Collections.Generic;

namespace Resident.Models;

public partial class Area
{
    public int AreaId { get; set; }

    public string AreaName { get; set; } = null!;

    public int PoliceId { get; set; }

    public virtual ICollection<HouseholdTransfer> HouseholdTransferFromAreas { get; set; } = new List<HouseholdTransfer>();

    public virtual ICollection<HouseholdTransfer> HouseholdTransferToAreas { get; set; } = new List<HouseholdTransfer>();

    public virtual User Police { get; set; } = null!;

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
