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
        public DbSet<GroupStudent> GroupStudents { get; set; }

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
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<DeviceBorrowingDetail>()
                .HasOne(dbd => dbd.DeviceBorrowingRequest)
                .WithMany(dbr => dbr.DeviceBorrowingDetails)
                .HasForeignKey(dbd => dbd.DeviceBorrowingRequestId)
                .IsRequired();

            // Relationship between Device and DeviceBorrowingDetail (1-n)
            builder.Entity<Device>()
                .HasMany(d => d.DeviceBorrowingDetails)
                .WithOne(dbd => dbd.Device)
                .HasForeignKey(dbd => dbd.DeviceId)
                .OnDelete(DeleteBehavior.Restrict);

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

            // Relationship between LabBorrowingRequest and DeviceBorrowingDetail (1-n)
            builder.Entity<LabBorrowingRequest>()
                .HasMany(lbr => lbr.DeviceBorrowingDetails)
                .WithOne(dbr => dbr.LabBorrowingRequest)
                .HasForeignKey(dbr => dbr.LabBorrowingRequestId)
                .OnDelete(DeleteBehavior.SetNull); // Ensure the foreign key is correctly defined

            // Relationship between DeviceBorrowingRequest and GroupStudents (1-n)
            builder.Entity<DeviceBorrowingRequest>()
                .HasMany(d => d.GroupStudents)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);

            // Relationship between LabBorrowingRequest and ApplicationUser (1-n)
            builder.Entity<LabBorrowingRequest>()
                .HasOne(dbr => dbr.User)
                .WithMany(u => u.LabBorrowingRequests)
                .HasForeignKey(dbr => dbr.UserId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
