using ExpenseTracker.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Data
{
    public class ExpenseRepository : IExpenseRepository
    {
        private readonly ExpenseTrackerDbContext _context;

        public ExpenseRepository(ExpenseTrackerDbContext context)
        {
            _context = context;
        }

        // Expense related methods
        public async Task<Expense> GetExpenseByIdAsync(int expenseId)
        {
            return await _context.Expenses.FindAsync(expenseId);
        }

        public async Task<IEnumerable<Expense>> GetExpensesByUserIdAsync(int userId)
        {
            return await _context.Expenses.Where(e => e.UserId == userId).ToListAsync();
        }

        public async Task<IEnumerable<Expense>> GetExpensesByFamilyIdAsync(int familyId)
        {
            var userIds = await _context.UserProfiles.Where(u => u.FamilyId == familyId).Select(u => u.UserId).ToListAsync();
            return await _context.Expenses.Where(e => userIds.Contains(e.UserId??0)).ToListAsync();
        }

        public async Task AddExpenseAsync(Expense expense)
        {
            _context.Expenses.Add(expense);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateExpenseAsync(Expense expense)
        {
            _context.Expenses.Update(expense);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteExpenseAsync(int expenseId)
        {
            var expense = await _context.Expenses.FindAsync(expenseId);
            if (expense != null)
            {
                _context.Expenses.Remove(expense);
                await _context.SaveChangesAsync();
            }
        }

        // ExpenseType related methods
        public async Task<IEnumerable<ExpenseType>> GetAllExpenseTypesAsync()
        {
            return await _context.ExpenseTypes.ToListAsync();
        }

        // ExpenseCategory related methods
        public async Task<IEnumerable<ExpenseCategory>> GetAllExpenseCategoriesAsync()
        {
            return await _context.ExpenseCategories.ToListAsync();
        }

        // CreditCard related methods
        public async Task<IEnumerable<CreditCard>> GetAllCreditCardsAsync()
        {
            return await _context.CreditCards.ToListAsync();
        }
    }

}
