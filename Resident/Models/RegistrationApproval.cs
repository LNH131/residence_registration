using System;
using System.Collections.Generic;
using Resident.Enums;

namespace Resident.Models;

public partial class RegistrationApproval
{
    public int ApprovalId { get; set; }

    public int RegistrationId { get; set; }

    public int ApproverId { get; set; }

    public string ApprovalStep { get; set; } = null!;

    public Status Status { get; set; }

    public string? Comments { get; set; }

    public DateTime? ApprovalDate { get; set; }

    public virtual User Approver { get; set; } = null!;

    public virtual Registration Registration { get; set; } = null!;
}
