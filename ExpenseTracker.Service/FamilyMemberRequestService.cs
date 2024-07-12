using AutoMapper;
using ExpenseTracker.Core.Entities;
using ExpenseTracker.Core.Models;
using ExpenseTracker.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Service
{
    public interface IFamilyMemberRequestService
    {
        Task<IEnumerable<FamilyMemberRequestModel>> GetFamilyMemberRequestsByUserIdAsync(int userId);
        Task<FamilyMemberRequestModel> GetFamilyMemberRequestByIdAsync(int familyMemberRequestId);
        Task AddFamilyMemberRequestAsync(FamilyMemberRequestModel familyMemberRequest);
        Task UpdateFamilyMemberRequestAsync(FamilyMemberRequestModel familyMemberRequest);
        Task DeleteFamilyMemberRequestAsync(int familyMemberRequestId);
    }

    public class FamilyMemberRequestService : IFamilyMemberRequestService
    {
        private readonly IFamilyMemberRequestRepository _familyMemberRequestRepository;
        private readonly IMapper _mapper;

        public FamilyMemberRequestService(IFamilyMemberRequestRepository familyMemberRequestRepository, IMapper mapper)
        {
            _familyMemberRequestRepository = familyMemberRequestRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<FamilyMemberRequestModel>> GetFamilyMemberRequestsByUserIdAsync(int userId)
        {
            var familyMemberRequests = await _familyMemberRequestRepository.GetFamilyMemberRequestsByUserIdAsync(userId);
            return _mapper.Map<IEnumerable<FamilyMemberRequestModel>>(familyMemberRequests);
        }

        public async Task<FamilyMemberRequestModel> GetFamilyMemberRequestByIdAsync(int familyMemberRequestId)
        {
            var familyMemberRequest = await _familyMemberRequestRepository.GetFamilyMemberRequestByIdAsync(familyMemberRequestId);
            return _mapper.Map<FamilyMemberRequestModel>(familyMemberRequest);
        }

        public async Task AddFamilyMemberRequestAsync(FamilyMemberRequestModel familyMemberRequest)
        {
            var familyMemberRequestEntity = _mapper.Map<FamilyMemberRequest>(familyMemberRequest);
            await _familyMemberRequestRepository.AddFamilyMemberRequestAsync(familyMemberRequestEntity);
        }

        public async Task UpdateFamilyMemberRequestAsync(FamilyMemberRequestModel familyMemberRequest)
        {
            var familyMemberRequestEntity = _mapper.Map<FamilyMemberRequest>(familyMemberRequest);
            await _familyMemberRequestRepository.UpdateFamilyMemberRequestAsync(familyMemberRequestEntity);
        }

        public async Task DeleteFamilyMemberRequestAsync(int familyMemberRequestId)
        {
            await _familyMemberRequestRepository.DeleteFamilyMemberRequestAsync(familyMemberRequestId);
        }
    }


}
