using api.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace api.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) {}

        public DbSet<Device> Devices { get; set; }
        public DbSet<DeviceBorrowingRequest> DeviceBorrowingRequests { get; set; }
        public DbSet<DeviceItem> DeviceItems { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Lab> Labs { get; set; }
        public DbSet<LabBorrowingRequest> LabBorrowingRequests { get; set; }
        public DbSet<DeviceBorrowingDetail> DeviceBorrowingDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Relationship between DeviceBorrowingRequest and ApplicationUser (1-n)
            builder.Entity<DeviceBorrowingRequest>()
                .HasOne(dbr => dbr.User)
                .WithMany(u => u.DeviceBorrowingRequests)
                .HasForeignKey(dbr => dbr.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            // Relationship between DeviceBorrowingRequest and DeviceBorrowingDetail (1-n)
            builder.Entity<DeviceBorrowingRequest>()
                .HasMany(dbr => dbr.DeviceBorrowingDetails)
                .WithOne(dbd => dbd.DeviceBorrowingRequest)
                .HasForeignKey(dbd => dbd.DeviceBorrowingRequestId)
                .OnDelete(DeleteBehavior.NoAction);

            // Relationship between Device and DeviceBorrowingDetail (1-n)
            builder.Entity<Device>()
                .HasMany(d => d.DeviceBorrowingDetails)
                .WithOne(dbd => dbd.Device)
                .HasForeignKey(dbd => dbd.DeviceId)
                .OnDelete(DeleteBehavior.NoAction);

            // Relationship between DeviceItem and Device (1-n)
            builder.Entity<Device>()
                .HasMany(d => d.DeviceItems)
                .WithOne(di => di.Device)
                .HasForeignKey(di => di.DeviceId)
                .OnDelete(DeleteBehavior.Cascade);

            // Relationship between DeviceBorrowingDetail and DeviceItem (n-1)
            builder.Entity<DeviceBorrowingDetail>()
                .HasOne(dbd => dbd.DeviceItem)
                .WithMany()
                .HasForeignKey(dbd => dbd.DeviceItemId)
                .OnDelete(DeleteBehavior.Cascade);

            // Relationship between DeviceBorrowingRequest and LabBorrowingRequest (1-n)
            builder.Entity<DeviceBorrowingRequest>()
                .HasOne(dbr => dbr.LabBorrowingRequest)
                .WithMany(lbr => lbr.DeviceBorrowingRequests)
                .HasForeignKey(dbr => dbr.LabBorrowingRequestId)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
