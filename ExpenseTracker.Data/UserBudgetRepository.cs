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
        Task<IEnumerable<UserBudget>> GetUserBudgetsByUserIdAsync(int userId, int month = 0, int year = 0);
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

        public async Task<IEnumerable<UserBudget>> GetUserBudgetsByUserIdAsync(int userId, int month = 0, int year = 0)
        {
            month = month == 0 ? DateTime.UtcNow.Month : month;
            year = year == 0 ? DateTime.UtcNow.Year : year;

            return await _context.UserBudgets
                .Where(ub => ub.UserId == userId
                && ub.BudgetDate.Month == month && ub.BudgetDate.Year == year)
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
