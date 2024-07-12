using ExpenseTracker.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Data
{
    public interface IUserBudgetRepository
    {
        Task<IEnumerable<UserBudget>> GetUserBudgetsByUserIdAsync(int userId);
        Task<UserBudget> GetUserBudgetByIdAsync(int userBudgetId);
        Task AddUserBudgetAsync(UserBudget userBudget);
        Task UpdateUserBudgetAsync(UserBudget userBudget);
        Task DeleteUserBudgetAsync(int userBudgetId);
    }


    public class UserBudgetRepository : IUserBudgetRepository
    {
        private readonly ExpenseTrackerDbContext _context;

        public UserBudgetRepository(ExpenseTrackerDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<UserBudget>> GetUserBudgetsByUserIdAsync(int userId)
        {
            return await _context.UserBudgets
                .Where(ub => ub.UserId == userId
                && ub.BudgetDate.Month == DateTime.UtcNow.Month && ub.BudgetDate.Year == DateTime.UtcNow.Year)
                .ToListAsync();
        }

        public async Task<UserBudget> GetUserBudgetByIdAsync(int userBudgetId)
        {
            return await _context.UserBudgets.FindAsync(userBudgetId);
        }

        public async Task AddUserBudgetAsync(UserBudget userBudget)
        {
            _context.UserBudgets.Add(userBudget);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateUserBudgetAsync(UserBudget userBudget)
        {
            _context.Entry(userBudget).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteUserBudgetAsync(int userBudgetId)
        {
            var userBudget = await _context.UserBudgets.FindAsync(userBudgetId);
            if (userBudget != null)
            {
                _context.UserBudgets.Remove(userBudget);
                await _context.SaveChangesAsync();
            }
        }
    }
}
