using System;
using System.Collections.Generic;

namespace Resident.Models;

public partial class UserContact
{
    public int ContactId { get; set; }

    public int UserId { get; set; }

    public string ContactType { get; set; } = null!;

    public string ContactValue { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
