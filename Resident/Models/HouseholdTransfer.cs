using System;
using System.Collections.Generic;

namespace Resident.Models;

public partial class HouseholdTransfer
{
    public int TransferId { get; set; }

    public int HouseholdId { get; set; }

    public int FromAreaId { get; set; }

    public int ToAreaId { get; set; }

    public DateOnly RequestDate { get; set; }

    public string Status { get; set; } = null!;

    public int? ApprovedBy { get; set; }

    public string? Comments { get; set; }

    public virtual User? ApprovedByNavigation { get; set; }

    public virtual Area FromArea { get; set; } = null!;

    public virtual Household Household { get; set; } = null!;

    public virtual Area ToArea { get; set; } = null!;
}
