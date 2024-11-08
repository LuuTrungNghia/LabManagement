using api.Dtos.User;
using api.Interfaces;
using api.Models;
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
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly SignInManager<ApplicationUser> _signinManager;

        public UserController(UserManager<ApplicationUser> userManager, ITokenService tokenService, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _signinManager = signInManager;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterUserDto registerDto)
        {
            var user = new ApplicationUser
            {
                UserName = registerDto.Username,
                Email = registerDto.Email,
                FullName = registerDto.FullName,
                Avatar = registerDto.Avatar,
                DateOfBirth = registerDto.DateOfBirth,
                Gender = registerDto.Gender
            };

            var result = await _userManager.CreateAsync(user, registerDto.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "user");
                var roles = await _userManager.GetRolesAsync(user);
                var token = _tokenService.CreateToken(user, roles);

                return Ok(new
                {
                    Username = user.UserName,
                    Email = user.Email,
                    Fullname = user.FullName,
                    Avatar = user.Avatar,
                    DateOfBirth = user.DateOfBirth,
                    Gender = user.Gender,
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
            var roles = await _userManager.GetRolesAsync(user);
            var token = _tokenService.CreateToken(user, roles);

            return Ok(new
            {
                Username = user.UserName,
                Email = user.Email,
                Token = token
            });
        }

        [HttpGet("get/{username}")]
        public async Task<IActionResult> GetUser(string username)
        {
            var user = await _userManager.FindByNameAsync(username) as ApplicationUser;
            if (user == null) return NotFound("User not found.");

            return Ok(new
            {
                Username = user.UserName,
                Email = user.Email,
                FullName = user.FullName,
                Avatar = user.Avatar,
                DateOfBirth = user.DateOfBirth,
                Gender = user.Gender,
                Roles = await _userManager.GetRolesAsync(user)
            });
        }


        [HttpGet("get-all")]
        //[Authorize(Roles = "admin")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userManager.Users.ToListAsync();
            if (users == null || users.Count == 0) return NotFound("No users found.");

            var userDtos = users.Select(user => new
            {
                Username = user.UserName,
                Email = user.Email,
                FullName = user.FullName,
                Avatar = user.Avatar,
                DateOfBirth = user.DateOfBirth,
                Gender = user.Gender,
                Roles = _userManager.GetRolesAsync(user).Result
            }).ToList();

            return Ok(userDtos);
        }

        [HttpPut("update/{username}")]
        //[Authorize(Roles = "admin,active")]
        public async Task<IActionResult> UpdateUser(string username, UpdateUserDto updateUserDto)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null) return NotFound("User not found.");

            // Cập nhật email nếu cần
            if (!string.IsNullOrEmpty(updateUserDto.Email) && user.Email != updateUserDto.Email)
            {
                user.Email = updateUserDto.Email;
            }

            if (!string.IsNullOrEmpty(updateUserDto.Fullname))
            {
                user.FullName = updateUserDto.Fullname;
            }

            if (!string.IsNullOrEmpty(updateUserDto.Avatar))
            {
                user.Avatar = updateUserDto.Avatar;
            }

            if (updateUserDto.DateOfBirth.HasValue)
            {
                user.DateOfBirth = updateUserDto.DateOfBirth.Value;
            }

            if (!string.IsNullOrEmpty(updateUserDto.Gender))
            {
                user.Gender = updateUserDto.Gender;
            }

            // Xử lý thay đổi mật khẩu nếu có
            if (!string.IsNullOrEmpty(updateUserDto.NewPassword))
            {
                if (updateUserDto.NewPassword != updateUserDto.ConfirmNewPassword)
                    return BadRequest("New passwords do not match.");

                var passwordCheck = await _userManager.CheckPasswordAsync(user, updateUserDto.CurrentPassword);
                if (!passwordCheck) return BadRequest("Current password is incorrect.");

                var passwordChangeResult = await _userManager.ChangePasswordAsync(user, updateUserDto.CurrentPassword, updateUserDto.NewPassword);
                if (!passwordChangeResult.Succeeded) return BadRequest(passwordChangeResult.Errors);
            }

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded) return BadRequest(result.Errors);

            return Ok("User updated successfully.");
        }

        [HttpDelete("delete/{username}")]
        //[Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteUser(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null) return NotFound("User not found.");

            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded) return Ok("User deleted successfully.");
            return BadRequest(result.Errors);
        }

        [HttpPut("approve/{username}")]
        //[Authorize(Roles = "admin")]
        public async Task<IActionResult> ApproveUser(string username, [FromQuery] string role)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null) return NotFound("User not found.");

            if (role != "student" && role != "lecturer")
                return BadRequest("Invalid role. Please specify 'student' or 'lecturer'.");

            await _userManager.AddToRoleAsync(user, role);
            return Ok($"User {username} approved as {role}.");
        }
    }
}
