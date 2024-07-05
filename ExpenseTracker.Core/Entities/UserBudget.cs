using System;
using System.Collections.Generic;

namespace ExpenseTracker.Core.Entities;

public partial class UserBudget
{
    public int UserBudgetId { get; set; }

    public int? UserId { get; set; }

    public decimal Amount { get; set; }

    public string ItemName { get; set; } = null!;

    public string ItemDescription { get; set; } = null!;

    public DateTime BudgetDate { get; set; }

    public bool? RepeatEveryMonth { get; set; }

    public virtual UserProfile? User { get; set; }
}
