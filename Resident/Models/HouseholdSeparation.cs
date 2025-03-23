using System;
using System.Collections.Generic;

namespace Resident.Models;

public partial class HouseholdSeparation
{
    public int SeparationId { get; set; }

    public int OriginalHouseholdId { get; set; }

    public int? NewHouseholdId { get; set; }

    public DateTime RequestDate { get; set; }

    public string Status { get; set; } = null!;

    public int? ApprovedBy { get; set; }

    public DateTime? ApprovalDate { get; set; }

    public string? Comments { get; set; }

    public virtual User? ApprovedByNavigation { get; set; }

    public virtual Household? NewHousehold { get; set; }

    public virtual Household OriginalHousehold { get; set; } = null!;

    public virtual ICollection<SeparationMember> SeparationMembers { get; set; } = new List<SeparationMember>();
}
