using System;
using System.Collections.Generic;

namespace ExpenseTracker.Core.Entities;

public partial class ExpenseCategory
{
    public int ExpenseCategoryId { get; set; }

    public string ExpenseCategoryName { get; set; } = null!;

    public virtual ICollection<Expense> Expenses { get; set; } = new List<Expense>();
}
