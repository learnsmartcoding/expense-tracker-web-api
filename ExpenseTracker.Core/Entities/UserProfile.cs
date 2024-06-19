using System;
using System.Collections.Generic;

namespace ExpenseTracker.Core.Entities;

public partial class UserProfile
{
    public int UserId { get; set; }

    public string DisplayName { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string AdObjId { get; set; } = null!;

    public int? FamilyId { get; set; }

    public virtual ICollection<CreditCard> CreditCards { get; set; } = new List<CreditCard>();

    public virtual ICollection<Expense> Expenses { get; set; } = new List<Expense>();

    public virtual Family? Family { get; set; }
}
