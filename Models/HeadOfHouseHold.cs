using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Project.Models;

namespace Project.Models;

public partial class HeadOfHouseHold
{
    public int HeadOfHouseHoldId { get; set; }

    public int UserId { get; set; }

    public int? HouseholdId { get; set; }

    public DateTime RegisteredDate { get; set; }

    public virtual Household? Household { get; set; }

    public virtual ICollection<Household> Households { get; set; } = new List<Household>();

    public virtual User User { get; set; } = null!;
}
