using System;
using Microsoft.EntityFrameworkCore;
using api.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace api.Data
{
    public class ApplicationDBContext : IdentityDbContext
    {
        public ApplicationDBContext(DbContextOptions dbContextOptions) : base(dbContextOptions) {}

        public DbSet<Device> Devices { get; set; }
        public DbSet<Lab> Labs { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
