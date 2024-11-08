using System.ComponentModel.DataAnnotations;

namespace api.Dtos.User
{
    public class RegisterUserDto
    {
        [Required]
        public string Username { get; set; }
        public string FullName { get; set; } 
        public string Avatar { get; set; }  // Thêm URL hoặc đường dẫn đến ảnh
        public DateTime DateOfBirth { get; set; }  // Ngày Sinh
        public string Gender { get; set; }  // Male, Female, Other
        
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        
        [Required]
        public string Password { get; set; }
    }
}
