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
        // public DbSet<Lab> Labs { get; set; }
        // public DbSet<LabBorrowingRequest> LabBorrowingRequests { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Relationship between ApplicationUser and DeviceBorrowingRequest (1-n)
            builder.Entity<DeviceBorrowingRequest>()
                .HasOne(dbr => dbr.User) // One DeviceBorrowingRequest has one User
                .WithMany(u => u.DeviceBorrowingRequests) // One User can have many DeviceBorrowingRequests
                .HasForeignKey(dbr => dbr.UserId) // Foreign key is UserId in DeviceBorrowingRequest
                .OnDelete(DeleteBehavior.NoAction); // Use NoAction instead of Cascade

            // Relationship between DeviceBorrowingRequest and DeviceBorrowingDetail (1-n)
            builder.Entity<DeviceBorrowingRequest>()
                .HasMany(dbr => dbr.DeviceBorrowingDetails) // One DeviceBorrowingRequest can have many DeviceBorrowingDetails
                .WithOne(dbd => dbd.DeviceBorrowingRequest) // Each DeviceBorrowingDetail belongs to one DeviceBorrowingRequest
                .HasForeignKey(dbd => dbd.DeviceBorrowingRequestId) // Foreign key in DeviceBorrowingDetail
                .OnDelete(DeleteBehavior.NoAction); // Use NoAction instead of Cascade

            // Relationship between Device and DeviceBorrowingDetail (1-n)
            builder.Entity<Device>()
                .HasMany(d => d.DeviceBorrowingDetails) // One Device can have many DeviceBorrowingDetails
                .WithOne(dbd => dbd.Device) // Each DeviceBorrowingDetail has one Device
                .HasForeignKey(dbd => dbd.DeviceId) // Foreign key in DeviceBorrowingDetail
                .OnDelete(DeleteBehavior.NoAction); // Use NoAction instead of Cascade

            // Relationship between DeviceItem and Device (1-n)
            builder.Entity<Device>()
                .HasMany(d => d.DeviceItems) // One Device can have many DeviceItems
                .WithOne(di => di.Device) // Each DeviceItem belongs to one Device
                .HasForeignKey(di => di.DeviceId) // Foreign key in DeviceItem
                .OnDelete(DeleteBehavior.Cascade); // Use NoAction instead of Cascade

            // Relationship between DeviceBorrowingDetail and DeviceItem (n-1)
            builder.Entity<DeviceBorrowingDetail>()
                .HasOne(dbd => dbd.DeviceItem) // Each DeviceBorrowingDetail has one DeviceItem
                .WithMany() // A DeviceItem can have many DeviceBorrowingDetails
                .HasForeignKey(dbd => dbd.DeviceItemId) // Foreign key in DeviceBorrowingDetail
                .OnDelete(DeleteBehavior.Cascade); // Use NoAction instead of Cascade

        //      // Quan hệ giữa Lab và Device (1-n)
        //     builder.Entity<Lab>()
        //         .HasMany(l => l.Devices) // Một Lab chứa nhiều thiết bị
        //         .WithOne(d => d.Lab) // Một thiết bị thuộc về một Lab
        //         .HasForeignKey(d => d.LabId)
        //         .OnDelete(DeleteBehavior.NoAction);

        //     // Quan hệ giữa Lab và LabBorrowingRequest (1-n)
        //     builder.Entity<LabBorrowingRequest>()
        //         .HasOne(lbr => lbr.Lab) // Một yêu cầu mượn Lab thuộc về một Lab
        //         .WithMany(l => l.LabBorrowingRequests) // Một Lab có nhiều yêu cầu mượn
        //         .HasForeignKey(lbr => lbr.LabId)
        //         .OnDelete(DeleteBehavior.NoAction);

        //     // Quan hệ giữa ApplicationUser và LabBorrowingRequest (1-n)
        //     builder.Entity<LabBorrowingRequest>()
        //         .HasOne(lbr => lbr.User) // Một yêu cầu mượn thuộc về một người dùng
        //         .WithMany(u => u.LabBorrowingRequests) // Một người dùng có thể có nhiều yêu cầu mượn Lab
        //         .HasForeignKey(lbr => lbr.UserId)
        //         .OnDelete(DeleteBehavior.NoAction);

        //     // Quan hệ giữa LabBorrowingRequest và LabBorrowingDetail (1-n)
        //     builder.Entity<LabBorrowingRequest>()
        //         .HasMany(lbr => lbr.LabBorrowingDetails) // Một yêu cầu mượn Lab có nhiều chi tiết
        //         .WithOne(lbd => lbd.LabBorrowingRequest) // Một chi tiết thuộc về một yêu cầu
        //         .HasForeignKey(lbd => lbd.LabBorrowingRequestId)
        //         .OnDelete(DeleteBehavior.NoAction);

        //     // Quan hệ giữa Device và LabBorrowingDetail (1-n)
        //     builder.Entity<LabBorrowingDetail>()
        //         .HasOne(lbd => lbd.Device) // Một chi tiết mượn liên kết với một thiết bị
        //         .WithMany(d => d.LabBorrowingDetails) // Một thiết bị có thể được mượn nhiều lần
        //         .HasForeignKey(lbd => lbd.DeviceId)
        //         .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
