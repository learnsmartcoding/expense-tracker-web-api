using System;
using System.Collections.Generic;

namespace ExpenseTracker.Core.Entities;

public partial class Family
{
    public int FamilyId { get; set; }

    public string FamilyName { get; set; } = null!;

    public virtual ICollection<UserProfile> UserProfiles { get; set; } = new List<UserProfile>();
}
