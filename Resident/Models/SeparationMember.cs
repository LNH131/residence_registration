using System;
using System.Collections.Generic;

namespace Resident.Models;

public partial class SeparationMember
{
    public int SeparationId { get; set; }

    public int UserId { get; set; }

    public string NewRelationship { get; set; } = null!;

    public bool IsNewHeadOfHousehold { get; set; }

    public virtual HouseholdSeparation Separation { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
