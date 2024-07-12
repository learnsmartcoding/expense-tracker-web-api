using ExpenseTracker.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Core.Models
{
    public class UserProfileModel
    {
        public int UserId { get; set; }

        public string DisplayName { get; set; } = null!;

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string AdObjId { get; set; } = null!;

        public int? FamilyId { get; set; }
    }

    public partial class UserIncomeModel
    {
        public int UserIncomeId { get; set; }

        public int? UserId { get; set; }

        public decimal Amount { get; set; }

        public string IncomeDescription { get; set; } = null!;

        public DateTime IncomeDate { get; set; }

        public bool? RepeatEveryMonth { get; set; }
       
    }

    public partial class UserBudgetModel
    {
        public int UserBudgetId { get; set; }

        public int? UserId { get; set; }

        public decimal Amount { get; set; }

        public string ItemName { get; set; } = null!;

        public string ItemDescription { get; set; } = null!;

        public DateTime BudgetDate { get; set; }

        public bool? RepeatEveryMonth { get; set; }
    }
}
