using System;
using System.Collections.Generic;

namespace ExpenseTracker.Core.Entities;

public partial class ExpenseType
{
    public int ExpenseTypeId { get; set; }

    public string ExpenseTypeName { get; set; } = null!;

    public virtual ICollection<ExpenseItem> ExpenseItems { get; set; } = new List<ExpenseItem>();

    public virtual ICollection<Expense> Expenses { get; set; } = new List<Expense>();
}
