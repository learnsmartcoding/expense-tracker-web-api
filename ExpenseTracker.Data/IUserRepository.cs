using ExpenseTracker.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Data
{
    public interface IUserRepository
    {
        Task<UserProfile> GetUserByIdAsync(int userId);
        Task<IEnumerable<UserProfile>> GetUsersByFamilyIdAsync(int familyId);
        Task AddUserAsync(UserProfile user);
        Task UpdateUserAsync(UserProfile user);
        Task DeleteUserAsync(int userId);

        Task<Family> GetFamilyByIdAsync(int familyId);
        Task<IEnumerable<Family>> GetAllFamiliesAsync();
        Task AddFamilyAsync(Family family);
        Task UpdateFamilyAsync(Family family);
        Task DeleteFamilyAsync(int familyId);
    }

}
