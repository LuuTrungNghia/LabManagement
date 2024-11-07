using System;
using Microsoft.AspNetCore.Identity;

namespace api.Models
{
    public class User : IdentityUser<int>
    {
        public string Name { get; set; }
        public string Role { get; set; }
        public bool IsApproved { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;
        public DateTime? LastLogin { get; set; }
        public int FailedLoginAttempts { get; set; }
    }
}
