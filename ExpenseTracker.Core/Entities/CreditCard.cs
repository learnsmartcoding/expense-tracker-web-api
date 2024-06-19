using System;
using System.Collections.Generic;

namespace ExpenseTracker.Core.Entities;

public partial class CreditCard
{
    public int CreditCardId { get; set; }

    public string CardLastFourDigit { get; set; } = null!;

    public string CreditCardName { get; set; } = null!;

    public int? UserId { get; set; }

    public virtual ICollection<Expense> Expenses { get; set; } = new List<Expense>();

    public virtual UserProfile? User { get; set; }
}
