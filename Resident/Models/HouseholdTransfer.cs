using Resident.Enums;

namespace Resident.Models;

public partial class HouseholdTransfer
{
    public int TransferId { get; set; }

    public int HouseholdId { get; set; }

    public int FromAreaId { get; set; }

    public int ToAreaId { get; set; }

    public DateOnly RequestDate { get; set; }

    public Status Status { get; set; } 

    public int? ApprovedBy { get; set; }

    public string? Comments { get; set; }

    public virtual User? ApprovedByNavigation { get; set; }

    public virtual Area FromArea { get; set; } = null!;

    public virtual Household Household { get; set; } = null!;

    public virtual Area ToArea { get; set; } = null!;
}
