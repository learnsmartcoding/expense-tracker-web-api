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
    public class UserIncomeController : ControllerBase
    {
        private readonly IUserIncomeService _userIncomeService;

        public UserIncomeController(IUserIncomeService userIncomeService)
        {
            _userIncomeService = userIncomeService;
        }

        [HttpGet("{userId}")]
        [RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes:Read")]
        public async Task<ActionResult<IEnumerable<UserIncomeModel>>> GetUserIncomesByUserId(int userId, int month = 0, int year = 0)
        {
            var userIncomes = await _userIncomeService.GetUserIncomesByUserIdAsync(userId,month, year);
            return Ok(userIncomes);
        }

        [HttpGet("{userId}/{userIncomeId}")]
        [RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes:Read")]
        public async Task<ActionResult<UserIncomeModel>> GetUserIncomeById(int userId, int userIncomeId)
        {
            var userIncome = await _userIncomeService.GetUserIncomeByIdAsync(userIncomeId);
            if (userIncome == null)
            {
                return NotFound();
            }
            return Ok(userIncome);
        }

        [HttpPost]
        [RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes:Write")]
        public async Task<ActionResult<UserIncomeModel>> AddUserIncome(UserIncomeModel userIncome)
        {
            await _userIncomeService.AddUserIncomeAsync(userIncome);
            return CreatedAtAction(nameof(GetUserIncomeById), new { userId = userIncome.UserId, userIncomeId = userIncome.UserIncomeId }, userIncome);
        }

        [HttpPut("{userIncomeId}")]
        [RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes:Write")]
        public async Task<IActionResult> UpdateUserIncome(int userIncomeId, UserIncomeModel userIncome)
        {
            if (userIncomeId != userIncome.UserIncomeId)
            {
                return BadRequest();
            }

            await _userIncomeService.UpdateUserIncomeAsync(userIncome);
            return NoContent();
        }

        [HttpDelete("{userIncomeId}")]
        [RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes:Write")]
        public async Task<IActionResult> DeleteUserIncome(int userIncomeId)
        {
            await _userIncomeService.DeleteUserIncomeAsync(userIncomeId);
            return NoContent();
        }
    }
}
