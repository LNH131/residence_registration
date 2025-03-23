using System;
using System.Collections.Generic;

namespace Resident.Models;

public partial class Registration
{
    public int RegistrationId { get; set; }

    public int UserId { get; set; }

    public int AddressId { get; set; }

    public string RegistrationType { get; set; } = null!;

    public DateOnly StartDate { get; set; }

    public DateOnly? EndDate { get; set; }

    public string Status { get; set; } = null!;

    public int? ApprovedBy { get; set; }

    public string? Comments { get; set; }

    public virtual Address Address { get; set; } = null!;

    public virtual User? ApprovedByNavigation { get; set; }

    public virtual ICollection<RegistrationApproval> RegistrationApprovals { get; set; } = new List<RegistrationApproval>();

    public virtual ICollection<RegistrationMember> RegistrationMembers { get; set; } = new List<RegistrationMember>();

    public virtual User User { get; set; } = null!;
}
