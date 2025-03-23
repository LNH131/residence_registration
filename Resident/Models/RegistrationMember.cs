using System;
using System.Collections.Generic;

namespace Resident.Models;

public partial class RegistrationMember
{
    public int RegistrationMemberId { get; set; }

    public int RegistrationId { get; set; }

    public string FullName { get; set; } = null!;

    public string Relationship { get; set; } = null!;

    public string? IdentityCard { get; set; }

    public DateOnly? Birthday { get; set; }

    public string? Sex { get; set; }

    public virtual Registration Registration { get; set; } = null!;
}
