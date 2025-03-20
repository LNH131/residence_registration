using System;
using System.Collections.Generic;

namespace Resident.Models;

public partial class DocumentType
{
    public int DocumentTypeId { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<Document> Documents { get; set; } = new List<Document>();
}
