using ExpenseTracker.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Core.Models
{
    public class ExpenseModel
    {
        public int ExpenseId { get; set; }

        public int? UserId { get; set; }

        public decimal ExpenseAmount { get; set; }

        public int? ExpenseCategoryId { get; set; }

        public int? ExpenseTypeId { get; set; }

        public int? CreditCardId { get; set; }

        public string ExpenseDescription { get; set; } = null!;

        public DateTime ExpenseDate { get; set; }
    }
    public partial class ExpenseTypeModel
    {
        public int ExpenseTypeId { get; set; }

        public string ExpenseTypeName { get; set; } = null!;
    }

    public partial class ExpenseCategoryModel
    {
        public int ExpenseCategoryId { get; set; }

        public string ExpenseCategoryName { get; set; } = null!;
        
    }
    public partial class CreditCardModel
    {
        public int CreditCardId { get; set; }

        public string CardLastFourDigit { get; set; } = null!;

        public string CreditCardName { get; set; } = null!;
    }
}
