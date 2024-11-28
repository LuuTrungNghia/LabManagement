using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using api.Models;
using api.Dtos.Server;

namespace api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ServerController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public ServerController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginServer([FromBody] ServerLoginDto loginDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Kiểm tra xem ServerUser và PassUser có đúng giá trị "admin" và "Admin@123"
            if (loginDto.ServerUser != "admin" || loginDto.PassUser != "Admin@123")
                return Unauthorized("Invalid server user credentials!");

            // Tìm người dùng trong Identity bằng Username
            var user = await _userManager.FindByNameAsync(loginDto.Username);
            if (user == null)
                return Unauthorized("Invalid username or password!");

            // Kiểm tra tài khoản đã được phê duyệt chưa
            if (!user.IsApproved)
                return Unauthorized("User account is not approved yet!");

            // Kiểm tra xem tài khoản có quyền "admin"
            if (!await _userManager.IsInRoleAsync(user, "admin"))
                return Unauthorized("User does not have access to server!");

            // Kiểm tra mật khẩu server (PassUser)
            if (loginDto.PassUser != "Admin@123")
                return Unauthorized("Invalid server password!");

            // Nếu mọi thứ đúng, trả về thông tin người dùng sau khi đăng nhập thành công
            var roles = await _userManager.GetRolesAsync(user);
            return Ok(new
            {
                Username = user.UserName,
                FullName = user.FullName,
                Roles = roles
            });
        }
    }
}
