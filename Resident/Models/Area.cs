using System;
using System.Collections.Generic;

namespace Resident.Models;

public partial class Area
{
    public int AreaId { get; set; }

    public string? AreaName { get; set; }

    public int PoliceId { get; set; }

    public virtual User Police { get; set; } = null!;

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
