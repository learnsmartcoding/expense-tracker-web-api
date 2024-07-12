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
    //[Authorize]
    public class ExpensesController : ControllerBase
    {
        private readonly IExpenseService _expenseService;

        public ExpensesController(IExpenseService expenseService)
        {
            _expenseService = expenseService;
        }

        [HttpGet("{id}")]
        //[RequiredScope(RequiredScopesConfigurationKey = "AzureAdB2C:Scopes:Read")]
        public async Task<ActionResult<ExpenseModel>> GetExpenseById(int id)
        {
            var expense = await _expenseService.GetExpenseByIdAsync(id);
            if (expense == null)
            {
                return NotFound();
            }
            return Ok(expense);
        }

        [HttpGet("user/{userId}")]
        //[RequiredScope(RequiredScopesConfigurationKey = "AzureAdB2C:Scopes:Read")]
        public async Task<ActionResult<IEnumerable<ExpenseModel>>> GetExpensesByUserId(int userId)
        {
            var expenses = await _expenseService.GetExpensesByUserIdAsync(userId);
            return Ok(expenses);
        }

        [HttpGet("family/{familyId}")]
        //[RequiredScope(RequiredScopesConfigurationKey = "AzureAdB2C:Scopes:Read")]
        public async Task<ActionResult<IEnumerable<ExpenseModel>>> GetExpensesByFamilyId(int familyId)
        {
            var expenses = await _expenseService.GetExpensesByFamilyIdAsync(familyId);
            return Ok(expenses);
        }

        [HttpPost]
        //[RequiredScope(RequiredScopesConfigurationKey = "AzureAdB2C:Scopes:Write")]
        public async Task<ActionResult> AddExpense(ExpenseModel expenseModel)
        {
            expenseModel.ExpenseItemsModel = expenseModel.ExpenseItemsModel ?? new List<ExpenseItemModel>();
            
            await _expenseService.AddExpenseAsync(expenseModel);
            return CreatedAtAction(nameof(GetExpenseById), new { id = expenseModel.ExpenseId }, expenseModel);
        }

        [HttpPut("{id}")]
        //[RequiredScope(RequiredScopesConfigurationKey = "AzureAdB2C:Scopes:Write")]
        public async Task<ActionResult> UpdateExpense(int id, ExpenseModel expenseModel)
        {
            if (id != expenseModel.ExpenseId)
            {
                return BadRequest();
            }

            expenseModel.ExpenseItemsModel = expenseModel.ExpenseItemsModel ?? new List<ExpenseItemModel>();

            await _expenseService.UpdateExpenseAsync(expenseModel);
            return NoContent();
        }

        [HttpDelete("{id}")]
        //[RequiredScope(RequiredScopesConfigurationKey = "AzureAdB2C:Scopes:Write")]
        public async Task<ActionResult> DeleteExpense(int id)
        {
            await _expenseService.DeleteExpenseAsync(id);
            return NoContent();
        }

        [HttpGet("types")]
        //[RequiredScope(RequiredScopesConfigurationKey = "AzureAdB2C:Scopes:Read")]
        public async Task<ActionResult<IEnumerable<ExpenseTypeModel>>> GetAllExpenseTypes()
        {
            var expenseTypes = await _expenseService.GetAllExpenseTypesAsync();
            return Ok(expenseTypes);
        }

        [HttpGet("categories")]
        //[RequiredScope(RequiredScopesConfigurationKey = "AzureAdB2C:Scopes:Read")]
        public async Task<ActionResult<IEnumerable<ExpenseCategoryModel>>> GetAllExpenseCategories()
        {
            var expenseCategories = await _expenseService.GetAllExpenseCategoriesAsync();
            return Ok(expenseCategories);
        }

        [HttpGet("creditcards")]
        //[RequiredScope(RequiredScopesConfigurationKey = "AzureAdB2C:Scopes:Read")]
        public async Task<ActionResult<IEnumerable<CreditCardModel>>> GetAllCreditCards()
        {
            var creditCards = await _expenseService.GetAllCreditCardsAsync();
            return Ok(creditCards);
        }
    }

}
