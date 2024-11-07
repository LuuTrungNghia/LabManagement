using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace api.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }
        // public DbSet<Device> Devices { get; set; }
        //public DbSet<DeviceBorrowingRequest> DeviceBorrowingRequests { get; set; }
        // public DbSet<Lab> Labs { get; set; }
        // public DbSet<LabBorrowingRequest> LabBorrowingRequests { get; set; }
        // public DbSet<RoomBookingRequest> RoomBookingRequests { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }   
}
