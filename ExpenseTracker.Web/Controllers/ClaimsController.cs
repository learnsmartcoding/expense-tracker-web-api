using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;
using System.Security.Claims;

namespace ExpenseTracker.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ClaimsController(IHttpContextAccessor httpContextAccessor) : ControllerBase
    {
        [HttpGet()]
        [RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes:Read")]
        public ActionResult<Dictionary<string, string>> Get()
        {
            var identity = httpContextAccessor.HttpContext.User.Identity as ClaimsIdentity;
            if (identity == null)
            {
                return Unauthorized(); // or appropriate error response
            }

            var claimsDictionary = identity.Claims
                .GroupBy(c => c.Type)
                .ToDictionary(g => g.Key, g => g.FirstOrDefault()?.Value);

            return Ok(claimsDictionary);
        }

        [HttpGet("public-endpoint")]
        [AllowAnonymous]
        public IActionResult PublicEndpoint()
        {
            return Ok(new { Message = "This endpoint is accessible without authorization." });
        }
    }

}
