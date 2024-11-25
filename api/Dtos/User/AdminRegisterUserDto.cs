using System.ComponentModel.DataAnnotations;

public class AdminRegisterUserDto
{
    [Required]
    public string Username { get; set; }
    public string FullName { get; set; } 
    public string Avatar { get; set; } 
    public DateTime DateOfBirth { get; set; }
    public string Gender { get; set; }
        
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    
    [Required]
    public string Password { get; set; }
    public string Role { get; set; }
}
