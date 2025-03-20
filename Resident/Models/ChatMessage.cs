using System;
using System.Collections.Generic;

namespace Resident.Models;

public partial class ChatMessage
{
    public int MessageId { get; set; }

    public int? FromUserId { get; set; }

    public int? ToUserId { get; set; }

    public string? Content { get; set; }

    public DateTime? SentDate { get; set; }

    public bool? IsRead { get; set; }

    public virtual User? FromUser { get; set; }

    public virtual User? ToUser { get; set; }
}
