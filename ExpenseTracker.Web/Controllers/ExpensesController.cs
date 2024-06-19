using ExpenseTracker.Core.Models;
using ExpenseTracker.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExpensesController : ControllerBase
    {
        private readonly IExpenseService _expenseService;

        public ExpensesController(IExpenseService expenseService)
        {
            _expenseService = expenseService;
        }

        [HttpGet("{id}")]
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
        public async Task<ActionResult<IEnumerable<ExpenseModel>>> GetExpensesByUserId(int userId)
        {
            var expenses = await _expenseService.GetExpensesByUserIdAsync(userId);
            return Ok(expenses);
        }

        [HttpGet("family/{familyId}")]
        public async Task<ActionResult<IEnumerable<ExpenseModel>>> GetExpensesByFamilyId(int familyId)
        {
            var expenses = await _expenseService.GetExpensesByFamilyIdAsync(familyId);
            return Ok(expenses);
        }

        [HttpPost]
        public async Task<ActionResult> AddExpense(ExpenseModel expenseModel)
        {
            await _expenseService.AddExpenseAsync(expenseModel);
            return CreatedAtAction(nameof(GetExpenseById), new { id = expenseModel.ExpenseId }, expenseModel);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateExpense(int id, ExpenseModel expenseModel)
        {
            if (id != expenseModel.ExpenseId)
            {
                return BadRequest();
            }

            await _expenseService.UpdateExpenseAsync(expenseModel);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteExpense(int id)
        {
            await _expenseService.DeleteExpenseAsync(id);
            return NoContent();
        }

        [HttpGet("types")]
        public async Task<ActionResult<IEnumerable<ExpenseTypeModel>>> GetAllExpenseTypes()
        {
            var expenseTypes = await _expenseService.GetAllExpenseTypesAsync();
            return Ok(expenseTypes);
        }

        [HttpGet("categories")]
        public async Task<ActionResult<IEnumerable<ExpenseCategoryModel>>> GetAllExpenseCategories()
        {
            var expenseCategories = await _expenseService.GetAllExpenseCategoriesAsync();
            return Ok(expenseCategories);
        }

        [HttpGet("creditcards")]
        public async Task<ActionResult<IEnumerable<CreditCardModel>>> GetAllCreditCards()
        {
            var creditCards = await _expenseService.GetAllCreditCardsAsync();
            return Ok(creditCards);
        }
    }

}
