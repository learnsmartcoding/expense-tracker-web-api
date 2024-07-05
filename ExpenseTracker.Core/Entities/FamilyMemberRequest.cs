using System;
using System.Collections.Generic;

namespace ExpenseTracker.Core.Entities;

public partial class FamilyMemberRequest
{
    public int FamilyMemberRequestId { get; set; }

    public int? RequestedUserId { get; set; }

    public string UserMessage { get; set; } = null!;

    public string FamilyEmailIds { get; set; } = null!;

    public bool? IsEmailSent { get; set; }

    public bool? IsProcessed { get; set; }

    public virtual UserProfile? RequestedUser { get; set; }
}
