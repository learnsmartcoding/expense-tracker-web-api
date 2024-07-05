using System;
using System.Collections.Generic;

namespace ExpenseTracker.Core.Entities;

public partial class UserIncome
{
    public int UserIncomeId { get; set; }

    public int? UserId { get; set; }

    public decimal Amount { get; set; }

    public string IncomeDescription { get; set; } = null!;

    public DateTime IncomeDate { get; set; }

    public bool? RepeatEveryMonth { get; set; }

    public virtual UserProfile? User { get; set; }
}
