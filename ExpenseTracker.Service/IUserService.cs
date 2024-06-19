using ExpenseTracker.Core.Entities;
using ExpenseTracker.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Service
{
    public interface IUserService
    {
        Task<UserProfileModel> GetUserByIdAsync(int userId);
        Task<IEnumerable<UserProfileModel>> GetUsersByFamilyIdAsync(int familyId);
        Task AddUserAsync(UserProfileModel user);
        Task UpdateUserAsync(UserProfileModel user);
        Task DeleteUserAsync(int userId);

        Task<FamilyMemberModel> GetFamilyByIdAsync(int familyId);
        Task<IEnumerable<FamilyModel>> GetAllFamiliesAsync();
        Task AddFamilyAsync(FamilyModel family);
        Task UpdateFamilyAsync(FamilyModel family);
        Task DeleteFamilyAsync(int familyId);
    }

}
