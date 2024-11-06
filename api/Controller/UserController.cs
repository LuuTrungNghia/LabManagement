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
    }
}
