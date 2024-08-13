using ExpenseTracker.Core.Models;
using ExpenseTracker.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;

namespace ExpenseTracker.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CreditCardController : ControllerBase
    {
        private readonly ICreditCardService _creditCardService;

        public CreditCardController(ICreditCardService creditCardService)
        {
            _creditCardService = creditCardService;
        }

        [HttpGet("{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<CreditCardModel>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes:Read")]
        public async Task<ActionResult<IEnumerable<CreditCardModel>>> GetCreditCardsByUserId(int userId)
        {
            var creditCards = await _creditCardService.GetCreditCardsByUserIdAsync(userId);
            return Ok(creditCards);
        }

        [HttpGet("{userId}/{creditCardId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CreditCardModel))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes:Read")]
        public async Task<ActionResult<CreditCardModel>> GetCreditCardById(int userId, int creditCardId)
        {
            var creditCard = await _creditCardService.GetCreditCardByIdAsync(creditCardId);
            if (creditCard == null)
            {
                return NotFound();
            }
            return Ok(creditCard);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(CreditCardModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes:Write")]
        public async Task<ActionResult<CreditCardModel>> AddCreditCard(CreditCardModel creditCard)
        {
            await _creditCardService.AddCreditCardAsync(creditCard);
            return CreatedAtAction(nameof(GetCreditCardById), new { userId = creditCard.UserId, creditCardId = creditCard.CreditCardId }, creditCard);
        }

        [HttpPut("{creditCardId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes:Write")]
        public async Task<IActionResult> UpdateCreditCard(int creditCardId, CreditCardModel creditCard)
        {
            if (creditCardId != creditCard.CreditCardId)
            {
                return BadRequest();
            }

            await _creditCardService.UpdateCreditCardAsync(creditCard);
            return NoContent();
        }

        [HttpDelete("{creditCardId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes:Write")]
        public async Task<IActionResult> DeleteCreditCard(int creditCardId)
        {
            await _creditCardService.DeleteCreditCardAsync(creditCardId);
            return NoContent();
        }
    }


}
