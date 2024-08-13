using ExpenseTracker.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Data
{
    public interface IUserIncomeRepository
    {
        Task<IEnumerable<UserIncome>> GetUserIncomesByUserIdAsync(int userId, int month = 0, int year = 0);
        Task<UserIncome> GetUserIncomeByIdAsync(int userIncomeId);
        Task AddUserIncomeAsync(UserIncome userIncome);
        Task UpdateUserIncomeAsync(UserIncome userIncome);
        Task DeleteUserIncomeAsync(int userIncomeId);
    }

    public class UserIncomeRepository : IUserIncomeRepository
    {
        private readonly ExpenseTrackerDbContext _context;

        public UserIncomeRepository(ExpenseTrackerDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<UserIncome>> GetUserIncomesByUserIdAsync(int userId, int month = 0, int year = 0)
        {
            month = month == 0 ? DateTime.UtcNow.Month : month;
            year = year == 0 ? DateTime.UtcNow.Year : year;

            return await _context.UserIncomes
                .Where(ui => ui.UserId == userId
                && ui.IncomeDate.Month == month && ui.IncomeDate.Year == year)
                .ToListAsync();
        }

        public async Task<UserIncome> GetUserIncomeByIdAsync(int userIncomeId)
        {
            return await _context.UserIncomes.FindAsync(userIncomeId);
        }

        public async Task AddUserIncomeAsync(UserIncome userIncome)
        {
            _context.UserIncomes.Add(userIncome);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateUserIncomeAsync(UserIncome userIncome)
        {
            _context.Entry(userIncome).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteUserIncomeAsync(int userIncomeId)
        {
            var userIncome = await _context.UserIncomes.FindAsync(userIncomeId);
            if (userIncome != null)
            {
                _context.UserIncomes.Remove(userIncome);
                await _context.SaveChangesAsync();
            }
        }
    }


}
