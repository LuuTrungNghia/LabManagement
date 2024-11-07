using System.Linq;
using System.Threading.Tasks;
using api.Dtos.User;
using api.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly SignInManager<IdentityUser> _signinManager;

        public UserController(UserManager<IdentityUser> userManager, ITokenService tokenService, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _signinManager = signInManager;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterUserDto registerDto)
        {
            var user = new IdentityUser
            {
                UserName = registerDto.Username,
                Email = registerDto.Email
            };

            var result = await _userManager.CreateAsync(user, registerDto.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "user");
                var token = _tokenService.CreateToken(user);

                return Ok(new
                {
                    Username = user.UserName,
                    Email = user.Email,
                    Token = token
                });
            }

            return BadRequest(result.Errors);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginUserDto loginDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == loginDto.Username.ToLower());
            if (user == null) return Unauthorized("Invalid username!");

            var result = await _signinManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
            if (!result.Succeeded) return Unauthorized("Invalid password.");

            var token = _tokenService.CreateToken(user);

            return Ok(new
            {
                Username = user.UserName,
                Email = user.Email,
                Token = token
            });
        }

        [HttpGet("get/{username}")]
        // [Authorize(Roles = "admin,active")]
        public async Task<IActionResult> GetUser(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null) return NotFound("User not found.");

            return Ok(new
            {
                Username = user.UserName,
                Email = user.Email,
                Roles = await _userManager.GetRolesAsync(user)
            });
        }

        [HttpGet("get-all")]
        // [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userManager.Users.ToListAsync();
            if (users == null || users.Count == 0) return NotFound("No users found.");

            var userDtos = users.Select(user => new
            {
                Username = user.UserName,
                Email = user.Email,
                Roles = _userManager.GetRolesAsync(user).Result
            }).ToList();

            return Ok(userDtos);
        }

        [HttpPut("update/{username}")]
        // [Authorize(Roles = "admin,active")]
        public async Task<IActionResult> UpdateUser(string username, UpdateUserDto updateUserDto)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null) return NotFound("User not found.");

            // Cập nhật email nếu có thay đổi
            if (!string.IsNullOrEmpty(updateUserDto.Email) && user.Email != updateUserDto.Email)
            {
                user.Email = updateUserDto.Email;
            }

            // Kiểm tra và thay đổi mật khẩu nếu có
            if (!string.IsNullOrEmpty(updateUserDto.NewPassword))
            {
                // Kiểm tra mật khẩu mới có khớp với xác nhận không
                if (updateUserDto.NewPassword != updateUserDto.ConfirmNewPassword)
                {
                    return BadRequest("New passwords do not match.");
                }

                // Kiểm tra mật khẩu hiện tại có chính xác không
                var passwordCheck = await _userManager.CheckPasswordAsync(user, updateUserDto.CurrentPassword);
                if (!passwordCheck)
                {
                    return BadRequest("Current password is incorrect.");
                }

                // Thực hiện thay đổi mật khẩu
                var passwordChangeResult = await _userManager.ChangePasswordAsync(user, updateUserDto.CurrentPassword, updateUserDto.NewPassword);
                if (!passwordChangeResult.Succeeded)
                {
                    return BadRequest(passwordChangeResult.Errors);
                }
            }

            // Cập nhật thông tin người dùng
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return Ok("User updated successfully.");
        }


        [HttpDelete("delete/{username}")]
        // [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteUser(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null) return NotFound("User not found.");

            var result = await _userManager.DeleteAsync(user);

            if (result.Succeeded)
                return Ok("User deleted successfully.");
            return BadRequest(result.Errors);
        }

        [HttpPut("approve/{username}")]
        //[Authorize(Roles = "admin")]
        public async Task<IActionResult> ApproveUser(string username, [FromQuery] string role)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null) return NotFound("User not found.");

            if (role != "student" && role != "lecturer")
            {
                return BadRequest("Invalid role. Please specify 'student' or 'lecturer'.");
            }

            await _userManager.AddToRoleAsync(user, role);
            return Ok($"User {username} approved as {role}.");
        }
    }
}
