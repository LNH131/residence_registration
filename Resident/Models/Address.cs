using System;
using System.Collections.Generic;

namespace Resident.Models;

public partial class Address
{
    public int AddressId { get; set; }

    public string Street { get; set; } = null!;

    public string City { get; set; } = null!;

    public string State { get; set; } = null!;

    public string ZipCode { get; set; } = null!;

    public string Country { get; set; } = null!;

    public string? Ward { get; set; }

    public string? District { get; set; }

    public virtual ICollection<HouseholdTransfer> HouseholdTransferFromAddresses { get; set; } = new List<HouseholdTransfer>();

    public virtual ICollection<HouseholdTransfer> HouseholdTransferToAddresses { get; set; } = new List<HouseholdTransfer>();

    public virtual ICollection<Household> Households { get; set; } = new List<Household>();

    public virtual ICollection<Registration> Registrations { get; set; } = new List<Registration>();

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
