using System;

namespace api.Models
{
    public class User
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Role { get; set; }
        public required string Email { get; set; }
        public bool IsApproved { get; set; }
        public required string Password { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;
        public DateTime? LastLogin { get; set; }
        public int FailedLoginAttempts { get; set; }
    }
}
