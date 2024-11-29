using Microsoft.AspNetCore.Mvc;
using api.Models;
using api.Dtos.Server;
using Microsoft.AspNetCore.Authorization;
using api.Data;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ServerController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ServerController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> CreateServerUser([FromBody] CreateUserDto createServerUserDto)
        {
            var existingServerUser = await _context.ServerUsers
                .FirstOrDefaultAsync(u => u.Username == createServerUserDto.Username);
            if (existingServerUser != null)
            {
                return BadRequest("Username đã tồn tại.");
            }

            var serverUser = new ServerUser
            {
                Username = createServerUserDto.Username,
                UserServer = createServerUserDto.UserServer,
                PassServer = createServerUserDto.PassServer,
                IsApproved = false  // Default to not approved
            };

            _context.ServerUsers.Add(serverUser);
            await _context.SaveChangesAsync();

            // Return the UserId, Username, and UserServer
            return Ok(new
            {
                UserId = serverUser.Id,
                Username = serverUser.Username,
                UserServer = serverUser.UserServer,
                IsApproved = serverUser.IsApproved
            });
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetAllServerUsers()
        {
            var serverUsers = await _context.ServerUsers.ToListAsync();
            var result = serverUsers.Select(user => new ServerUserDto
            {
                Id = user.Id,
                Username = user.Username,
                UserServer = user.UserServer,
                IsApproved = user.IsApproved
            }).ToList();

            return Ok(result);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetServerUserById(int id)
        {
            var serverUser = await _context.ServerUsers.FindAsync(id);
            if (serverUser == null)
            {
                return NotFound("Server user không tìm thấy!");
            }

            var serverUserDetails = new ServerUserDto
            {
                Id = serverUser.Id,
                Username = serverUser.Username,
                UserServer = serverUser.UserServer,
                PassServer = serverUser.PassServer,
                IsApproved = serverUser.IsApproved
            };

            return Ok(serverUserDetails);
        }

        [HttpPut("approve/{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> ApproveServerUser(int id)
        {
            var serverUser = await _context.ServerUsers.FindAsync(id);
            if (serverUser == null) return NotFound("Server user không tìm thấy!");

            if (serverUser.IsApproved) return BadRequest("Server user đã được phê duyệt!");

            serverUser.IsApproved = true;
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Server user đã được phê duyệt thành công!" });
        }

        [HttpPut("reject/{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> RejectServerUser(int id)
        {
            var serverUser = await _context.ServerUsers.FindAsync(id);
            if (serverUser == null) return NotFound("Server user không tìm thấy!");

            if (!serverUser.IsApproved) return BadRequest("Server user đã bị từ chối!");

            serverUser.IsApproved = false;
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Server user đã bị từ chối thành công!" });
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteServerUser(int id)
        {
            var serverUser = await _context.ServerUsers.FindAsync(id);
            if (serverUser == null) return NotFound("Server user không tìm thấy!");

            _context.ServerUsers.Remove(serverUser);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Server user đã bị xóa thành công!" });
        }

        [HttpGet("history")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetHistory([FromQuery] string username)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                return BadRequest("Username cannot be empty.");
            }

            var serverUsers = await _context.ServerUsers
                .Where(u => u.Username == username) // Filter by username
                .OrderByDescending(u => u.Id)       // Sort by Id in descending order
                .Select(user => new HistoryDto      // Use DTO
                {
                    UserId = user.Id,
                    Username = user.Username,
                    UserServer = user.UserServer,
                    IsApproved = user.IsApproved
                })
                .ToListAsync();

            if (serverUsers.Count == 0)
            {
                return NotFound("No history found for the provided username.");
            }

            return Ok(serverUsers);
        }
    }
}
