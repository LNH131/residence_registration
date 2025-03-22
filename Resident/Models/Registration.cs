using System;
using System.Collections.Generic;
using Resident.Enums;

namespace Resident.Models;

public partial class Registration
{
    public int RegistrationId { get; set; }

    public int UserId { get; set; }

    public int AddressId { get; set; }

    public RegistrationType RegistrationType { get; set; } 

    public DateOnly StartDate { get; set; }

    public DateOnly? EndDate { get; set; }

    public Status Status { get; set; } 

    public int? ApprovedBy { get; set; }

    public string? Comments { get; set; }

    public virtual Address Address { get; set; } = null!;

    public virtual User? ApprovedByNavigation { get; set; }

    public virtual ICollection<Document> Documents { get; set; } = new List<Document>();

    public virtual ICollection<RegistrationApproval> RegistrationApprovals { get; set; } = new List<RegistrationApproval>();

    public virtual User User { get; set; } = null!;
}
