﻿using ExpenseTracker.Core.Models;
using ExpenseTracker.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserProfileModel>> GetUserById(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpGet("family/{familyId}")]
        public async Task<ActionResult<IEnumerable<UserProfileModel>>> GetUsersByFamilyId(int familyId)
        {
            var users = await _userService.GetUsersByFamilyIdAsync(familyId);
            return Ok(users);
        }

        [HttpPost]
        public async Task<ActionResult> AddUser(UserProfileModel userModel)
        {
            await _userService.AddUserAsync(userModel);
            return CreatedAtAction(nameof(GetUserById), new { id = userModel.UserId }, userModel);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateUser(int id, UserProfileModel userModel)
        {
            if (id != userModel.UserId)
            {
                return BadRequest();
            }

            await _userService.UpdateUserAsync(userModel);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteUser(int id)
        {
            await _userService.DeleteUserAsync(id);
            return NoContent();
        }
    }

}
