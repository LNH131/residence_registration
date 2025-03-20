using System;
using System.Collections.Generic;

namespace Resident.Models;

public partial class Document
{
    public int DocumentId { get; set; }

    public int RegistrationId { get; set; }

    public int DocumentTypeId { get; set; }

    public string FilePath { get; set; } = null!;

    public DateOnly? UploadDate { get; set; }

    public virtual DocumentType DocumentType { get; set; } = null!;

    public virtual Registration Registration { get; set; } = null!;
}
