using ExpenseTracker.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly ExpenseTrackerDbContext _context;

        public UserRepository(ExpenseTrackerDbContext context)
        {
            _context = context;
        }

        // UserProfile related methods
        public async Task<UserProfile> GetUserByIdAsync(int userId)
        {
            return await _context.UserProfiles.FindAsync(userId);
        }

        public async Task<IEnumerable<UserProfile>> GetUsersByFamilyIdAsync(int familyId)
        {
            return await _context.UserProfiles.Where(u => u.FamilyId == familyId).ToListAsync();
        }

        public async Task AddUserAsync(UserProfile user)
        {
            _context.UserProfiles.Add(user);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateUserAsync(UserProfile user)
        {
            _context.UserProfiles.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteUserAsync(int userId)
        {
            var user = await _context.UserProfiles.FindAsync(userId);
            if (user != null)
            {
                _context.UserProfiles.Remove(user);
                await _context.SaveChangesAsync();
            }
        }

        // Family related methods
        public async Task<Family> GetFamilyByIdAsync(int familyId)
        {
            return await _context.Families.FindAsync(familyId);
        }

        public async Task<IEnumerable<Family>> GetAllFamiliesAsync()
        {
            return await _context.Families.ToListAsync();
        }

        public async Task AddFamilyAsync(Family family)
        {
            _context.Families.Add(family);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateFamilyAsync(Family family)
        {
            _context.Families.Update(family);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteFamilyAsync(int familyId)
        {
            var family = await _context.Families.FindAsync(familyId);
            if (family != null)
            {
                _context.Families.Remove(family);
                await _context.SaveChangesAsync();
            }
        }
    }

}
