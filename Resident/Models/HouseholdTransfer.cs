using System;
using System.Collections.Generic;

namespace Resident.Models;

public partial class HouseholdTransfer
{
    public int TransferId { get; set; }

    public int HouseholdId { get; set; }

    public int FromAddressId { get; set; }

    public int ToAddressId { get; set; }

    public DateOnly RequestDate { get; set; }

    public string Status { get; set; } = null!;

    public int? ApprovedBy { get; set; }

    public string? Comments { get; set; }

    public virtual User? ApprovedByNavigation { get; set; }

    public virtual Address FromAddress { get; set; } = null!;

    public virtual Household Household { get; set; } = null!;

    public virtual Address ToAddress { get; set; } = null!;
}
