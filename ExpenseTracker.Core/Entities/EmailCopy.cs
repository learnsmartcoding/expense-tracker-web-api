using System;
using System.Collections.Generic;

namespace ExpenseTracker.Core.Entities;

public partial class EmailCopy
{
    public int EmailCopyId { get; set; }

    public string EmailFrom { get; set; } = null!;

    public string EmailTo { get; set; } = null!;

    public string EmailSubject { get; set; } = null!;

    public string EmailMessage { get; set; } = null!;

    public DateTime SentDate { get; set; }
}
