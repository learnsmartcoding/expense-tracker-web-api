using ExpenseTracker.Core.Entities;
using Microsoft.EntityFrameworkCore;

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

            return await _context.Expenses
                 .Include(i => i.ExpenseItems)
                 .Where(w => w.ExpenseId == expenseId)
                 .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Expense>> GetExpensesByUserIdAsync(int userId, int month = 0, int year = 0)
        {
            month = month == 0 ? DateTime.UtcNow.Month : month;
            year = year == 0 ? DateTime.UtcNow.Year : year;

            return await _context.Expenses
                .Include(i => i.ExpenseItems)
                .Where(e => e.UserId == userId && e.ExpenseDate.Month == month && 
                e.ExpenseDate.Year == year)
                .ToListAsync();
        }

        public async Task<IEnumerable<Expense>> GetExpensesByFamilyIdAsync(int familyId, int month = 0, int year = 0)
        {
            month = month == 0 ? DateTime.UtcNow.Month : month;
            year = year == 0 ? DateTime.UtcNow.Year : year;

            var userIds = await _context.UserProfiles
                .Where(u => u.FamilyId == familyId)
                .Select(u => u.UserId)
                .ToListAsync();

            return await _context.Expenses
                .Include(i => i.ExpenseItems)
                .Where(e => userIds.Contains(e.UserId ?? 0) && e.ExpenseDate.Month == month 
                && e.ExpenseDate.Year == year)
                .ToListAsync();
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
            //var expense = await GetExpenseByIdAsync(expenseId);//this will delete its child entry as well
            if (expense != null)
            {
                //if you dont use this, then the child items expenseid will be set null based on foreign key concepts
                /*
                 if (expense.ExpenseItems != null && expense.ExpenseItems.Any())
                     _context.ExpenseItems.RemoveRange(expense.ExpenseItems);
                */
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
