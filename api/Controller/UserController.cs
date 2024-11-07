using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using api.Dtos.User;
using api.Interfaces;
using api.Models;

namespace api.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepo;
        private readonly ITokenService _tokenService;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserRepository userRepo, ITokenService tokenService, ILogger<UserController> logger)
        {
            _userRepo = userRepo;
            _tokenService = tokenService;
            _logger = logger;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            if (string.IsNullOrWhiteSpace(loginDto.Username) || string.IsNullOrWhiteSpace(loginDto.Password))
                return BadRequest("Username and password are required");

            _logger.LogInformation($"Login attempt for user: {loginDto.Username}");

            var user = await _userRepo.AuthenticateUserAsync(loginDto.Username, loginDto.Password);
            if (user == null) return Unauthorized("Invalid credentials");

            if (!user.IsApproved)
            {
                return Unauthorized("User is not approved.");
            }

            var token = _tokenService.CreateToken(user);

            var userDto = new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                Role = user.Role,
                Email = user.Email,
                IsApproved = user.IsApproved
            };

            return Ok(new LoginResponseDto 
            { 
                User = userDto, 
                Token = token 
            });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            if (string.IsNullOrEmpty(registerDto.Role))
            {
                return BadRequest("Role is required.");
            }

            var existingUserByUsername = await _userRepo.GetByUsernameOrEmailAsync(registerDto.Username);
            if (existingUserByUsername != null)
            {
                return BadRequest("Username or email already in use.");
            }

            var existingUserByEmail = await _userRepo.GetByUsernameOrEmailAsync(registerDto.Email);
            if (existingUserByEmail != null)
            {
                return BadRequest("Username or email already in use.");
            }

            var createdUserDto = await _userRepo.CreateAsync(registerDto);

            var userForToken = new User
            {
                Id = createdUserDto.Id,
                Name = createdUserDto.Name,
                Role = createdUserDto.Role,
                Email = createdUserDto.Email,
                IsApproved = createdUserDto.IsApproved,
                Password = registerDto.Password
            };

            var token = _tokenService.CreateToken(userForToken);

            return CreatedAtAction(nameof(GetById), new { id = createdUserDto.Id }, new 
            {
                User = createdUserDto,
                Token = token
            });
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
            if (user == null) return NotFound("User not found.");
            return Ok(user);
        }

        [HttpPut("approve/{id:int}")]
        public async Task<IActionResult> ApproveUser(int id)
        {
            var updatedUser = await _userRepo.ApproveUserAsync(id);
            if (updatedUser == null) return NotFound("User not found.");
            return Ok(updatedUser);
        }

        [HttpPost("reset-password/{id:int}")]
        public async Task<IActionResult> ResetPassword(int id, [FromBody] ResetPasswordDto resetPasswordDto)
        {
            if (string.IsNullOrEmpty(resetPasswordDto.NewPassword))
                return BadRequest("New password is required.");
            
            var user = await _userRepo.ResetPasswordAsync(id, resetPasswordDto.NewPassword);
            if (user == null) return NotFound("User not found.");
            return Ok(user);
        }
    }
}
