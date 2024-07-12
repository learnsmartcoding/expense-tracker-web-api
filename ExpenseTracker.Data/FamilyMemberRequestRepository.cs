using ExpenseTracker.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Data
{
    public interface IFamilyMemberRequestRepository
    {
        Task<IEnumerable<FamilyMemberRequest>> GetFamilyMemberRequestsByUserIdAsync(int userId);
        Task<FamilyMemberRequest> GetFamilyMemberRequestByIdAsync(int familyMemberRequestId);
        Task AddFamilyMemberRequestAsync(FamilyMemberRequest familyMemberRequest);
        Task UpdateFamilyMemberRequestAsync(FamilyMemberRequest familyMemberRequest);
        Task DeleteFamilyMemberRequestAsync(int familyMemberRequestId);
    }

    public class FamilyMemberRequestRepository : IFamilyMemberRequestRepository
    {
        private readonly ExpenseTrackerDbContext _context;

        public FamilyMemberRequestRepository(ExpenseTrackerDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<FamilyMemberRequest>> GetFamilyMemberRequestsByUserIdAsync(int userId)
        {
            return await _context.FamilyMemberRequests
                .Where(fmr => fmr.RequestedUserId == userId)
                .ToListAsync();
        }

        public async Task<FamilyMemberRequest> GetFamilyMemberRequestByIdAsync(int familyMemberRequestId)
        {
            return await _context.FamilyMemberRequests.FindAsync(familyMemberRequestId);
        }

        public async Task AddFamilyMemberRequestAsync(FamilyMemberRequest familyMemberRequest)
        {
            _context.FamilyMemberRequests.Add(familyMemberRequest);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateFamilyMemberRequestAsync(FamilyMemberRequest familyMemberRequest)
        {
            _context.Entry(familyMemberRequest).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteFamilyMemberRequestAsync(int familyMemberRequestId)
        {
            var familyMemberRequest = await _context.FamilyMemberRequests.FindAsync(familyMemberRequestId);
            if (familyMemberRequest != null)
            {
                _context.FamilyMemberRequests.Remove(familyMemberRequest);
                await _context.SaveChangesAsync();
            }
        }
    }


}
