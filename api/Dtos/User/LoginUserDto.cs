using System.ComponentModel.DataAnnotations;

namespace api.Dtos.User
{
    public class LoginUserDto
    {        
        public required string Username { get; set; }
        public required string Password { get; set; }
    }
}
