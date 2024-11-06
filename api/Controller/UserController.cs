using System.Threading.Tasks;
using api.Dtos.User;
using api.Interface;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/v{v}/User")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepo;

        public UserController(IUserRepository userRepo)
        {
            _userRepo = userRepo;
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAll()
        {
            var users = await _userRepo.GetAllAsync();
            return Ok(users);
        }

        [HttpGet("get/{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await _userRepo.GetByIdAsync(id);
            if (user == null) return NotFound();
            return Ok(user);
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] CreateUserRequestDto userDto)
        {
            var user = await _userRepo.CreateAsync(userDto);
            return CreatedAtAction(nameof(GetById), new { v = 1, id = user.Id }, user);
        }

        [HttpPut("update/{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateUserRequestDto userDto)
        {
            var updatedUser = await _userRepo.UpdateAsync(id, userDto);
            if (updatedUser == null) return NotFound();
            return Ok(updatedUser);
        }

        [HttpDelete("delete/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _userRepo.DeleteAsync(id);
            if (!success) return NotFound();
            return NoContent();
        }

        [HttpPut("approve/{id:int}")]
        public async Task<IActionResult> ApproveUser(int id)
        {
            var user = await _userRepo.ApproveUserAsync(id);
            if (user == null) return NotFound();
            return Ok(user);
        }

        [HttpPut("reset-password/{id:int}")]
        public async Task<IActionResult> ResetPassword(int id, [FromBody] string newPassword)
        {
            var user = await _userRepo.ResetPasswordAsync(id, newPassword);
            if (user == null) return NotFound();
            return Ok(user);
        }

        [HttpPost("import")]
        public async Task<IActionResult> ImportUsers([FromBody] IEnumerable<CreateUserRequestDto> users)
        {
            var importedUsers = await _userRepo.ImportUsersAsync(users);
            return Ok(importedUsers);
        }
    }
}
