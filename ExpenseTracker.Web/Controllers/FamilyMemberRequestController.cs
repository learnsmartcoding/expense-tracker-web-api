using ExpenseTracker.Core.Models;
using ExpenseTracker.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;

namespace ExpenseTracker.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class FamilyMemberRequestController : ControllerBase
    {
        private readonly IFamilyMemberRequestService _familyMemberRequestService;

        public FamilyMemberRequestController(IFamilyMemberRequestService familyMemberRequestService)
        {
            _familyMemberRequestService = familyMemberRequestService;
        }

        [HttpGet("{userId}")]
        [RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes:Read")]
        public async Task<ActionResult<IEnumerable<FamilyMemberRequestModel>>> GetFamilyMemberRequestsByUserId(int userId)
        {
            var familyMemberRequests = await _familyMemberRequestService.GetFamilyMemberRequestsByUserIdAsync(userId);
            return Ok(familyMemberRequests);
        }

        [HttpGet("{userId}/{familyMemberRequestId}")]
        [RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes:Read")]
        public async Task<ActionResult<FamilyMemberRequestModel>> GetFamilyMemberRequestById(int userId, int familyMemberRequestId)
        {
            var familyMemberRequest = await _familyMemberRequestService.GetFamilyMemberRequestByIdAsync(familyMemberRequestId);
            if (familyMemberRequest == null)
            {
                return NotFound();
            }
            return Ok(familyMemberRequest);
        }

        [HttpPost]
        [RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes:Write")]
        public async Task<ActionResult<FamilyMemberRequestModel>> AddFamilyMemberRequest(FamilyMemberRequestModel familyMemberRequest)
        {
            await _familyMemberRequestService.AddFamilyMemberRequestAsync(familyMemberRequest);
            return CreatedAtAction(nameof(GetFamilyMemberRequestById), new { userId = familyMemberRequest.RequestedUserId, familyMemberRequestId = familyMemberRequest.FamilyMemberRequestId }, familyMemberRequest);
        }

        [HttpPut("{familyMemberRequestId}")]
        [RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes:Write")]
        public async Task<IActionResult> UpdateFamilyMemberRequest(int familyMemberRequestId, FamilyMemberRequestModel familyMemberRequest)
        {
            if (familyMemberRequestId != familyMemberRequest.FamilyMemberRequestId)
            {
                return BadRequest();
            }

            await _familyMemberRequestService.UpdateFamilyMemberRequestAsync(familyMemberRequest);
            return NoContent();
        }

        [HttpDelete("{familyMemberRequestId}")]
        [RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes:Write")]
        public async Task<IActionResult> DeleteFamilyMemberRequest(int familyMemberRequestId)
        {
            await _familyMemberRequestService.DeleteFamilyMemberRequestAsync(familyMemberRequestId);
            return NoContent();
        }
    }

}
