using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using api.Models;
using api.Dtos.Server;
using Microsoft.AspNetCore.Authorization;  // Thêm namespace cho [Authorize]

namespace api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ServerController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public ServerController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        // **1. Login Server** - Không cần phải có quyền admin cho login
        [HttpPost("login")]
        public async Task<IActionResult> LoginServer([FromBody] ServerLoginDto loginDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Tìm người dùng trong cơ sở dữ liệu theo Username
            var user = await _userManager.FindByNameAsync(loginDto.Username);
            if (user == null || !user.IsApproved)
                return Unauthorized("Invalid credentials or user not approved!");

            // Kiểm tra mật khẩu của người dùng
            var passwordCheck = await _userManager.CheckPasswordAsync(user, loginDto.PassUser);
            if (!passwordCheck)
                return Unauthorized("Invalid password!");

            // Kiểm tra xem người dùng có phải là admin không (nếu cần thiết)
            if (!await _userManager.IsInRoleAsync(user, "admin"))
                return Unauthorized("User does not have server access!");

            return Ok(new
            {
                Username = user.UserName,
                FullName = user.FullName
            });
        }

        // **2. Create User** - Cần quyền admin
        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserDto createUserDto)
        {
            var user = new ApplicationUser
            {
                UserName = createUserDto.Username,
                Email = createUserDto.Email,
                FullName = createUserDto.FullName,
                Gender = createUserDto.Gender,
                DateOfBirth = createUserDto.DateOfBirth,
                IsApproved = false
            };

            var result = await _userManager.CreateAsync(user, createUserDto.Password);
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            return Ok(new { Message = "User created successfully!", UserId = user.Id });
        }

        // **3. Get All Users** - Cần quyền admin
        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = _userManager.Users.ToList();
            var result = new List<UserDetailsDto>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                result.Add(new UserDetailsDto
                {
                    Id = user.Id,
                    Username = user.UserName,
                    Email = user.Email,
                    FullName = user.FullName,
                    IsApproved = user.IsApproved,
                    Roles = roles.ToList()
                });
            }

            return Ok(result);
        }

        // **4. Approve User** - Cần quyền admin
        [HttpPut("approve/{username}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> ApproveUser(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null) return NotFound("User not found!");

            if (user.IsApproved) return BadRequest("User is already approved!");

            user.IsApproved = true;
            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded) return BadRequest(result.Errors);

            return Ok(new { Message = "User approved successfully!" });
        }

        // **5. Reject User** - Cần quyền admin
        [HttpPut("reject/{username}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> RejectUser(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null) return NotFound("User not found!");

            if (!user.IsApproved) return BadRequest("User is already not approved!");

            user.IsApproved = false;
            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded) return BadRequest(result.Errors);

            return Ok(new { Message = "User rejected successfully!" });
        }

        // **6. Delete User** - Cần quyền admin
        [HttpDelete("{username}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteUser(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null) return NotFound("User not found!");

            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded) return BadRequest(result.Errors);

            return Ok(new { Message = "User deleted successfully!" });
        }
    }
}
