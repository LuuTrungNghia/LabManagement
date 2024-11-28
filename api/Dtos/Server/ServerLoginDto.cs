using System.ComponentModel.DataAnnotations;

namespace api.Dtos.Server
{
    public class ServerLoginDto
    {
        [Required]
        public string Username { get; set; } // Tên người dùng từ hệ thống Identity

        [Required]
        public string ServerUser { get; set; } // Tên người dùng server (fixed value: "serveruser")

        [Required]
        public string PassUser { get; set; } // Mật khẩu server (fixed value: "passuser")
    }
}
