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
    public class UserBudgetController : ControllerBase
    {
        private readonly IUserBudgetService _userBudgetService;

        public UserBudgetController(IUserBudgetService userBudgetService)
        {
            _userBudgetService = userBudgetService;
        }

        [HttpGet("{userId}")]
        [RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes:Read")]
        public async Task<ActionResult<IEnumerable<UserBudgetModel>>> GetUserBudgetsByUserId(int userId, int month = 0, int year = 0)
        {
            var userBudgets = await _userBudgetService.GetUserBudgetsByUserIdAsync(userId,month, year);
            return Ok(userBudgets);
        }

        [HttpGet("{userId}/{userBudgetId}")]
        [RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes:Read")]
        public async Task<ActionResult<UserBudgetModel>> GetUserBudgetById(int userId, int userBudgetId)
        {
            var userBudget = await _userBudgetService.GetUserBudgetByIdAsync(userBudgetId);
            if (userBudget == null)
            {
                return NotFound();
            }
            return Ok(userBudget);
        }

        [HttpPost]
        [RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes:Write")]
        public async Task<ActionResult<UserBudgetModel>> AddUserBudget(UserBudgetModel userBudget)
        {
            await _userBudgetService.AddUserBudgetAsync(userBudget);
            return CreatedAtAction(nameof(GetUserBudgetById), new { userId = userBudget.UserId, userBudgetId = userBudget.UserBudgetId }, userBudget);
        }

        [HttpPut("{userBudgetId}")]
        [RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes:Write")]
        public async Task<IActionResult> UpdateUserBudget(int userBudgetId, UserBudgetModel userBudget)
        {
            if (userBudgetId != userBudget.UserBudgetId)
            {
                return BadRequest();
            }

            await _userBudgetService.UpdateUserBudgetAsync(userBudget);
            return NoContent();
        }

        [HttpDelete("{userBudgetId}")]
        [RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes:Write")]
        public async Task<IActionResult> DeleteUserBudget(int userBudgetId)
        {
            await _userBudgetService.DeleteUserBudgetAsync(userBudgetId);
            return NoContent();
        }
    }
}
