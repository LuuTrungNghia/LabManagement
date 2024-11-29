using System.ComponentModel.DataAnnotations;
using api.Models;

public class LabBorrowingRequest
{    
    public int Id { get; set; }
    public string Username { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public LabBorrowingStatus Status { get; set; }
    [MaxLength(500)]
    public string Description { get; set; }
    public int LabId { get; set; } = 1;
    public Lab BorrowedLab { get; set; }
    [Required]
    public string UserId { get; set; }
    public ApplicationUser User { get; set; }
    public ICollection<DeviceBorrowingDetail> DeviceBorrowingDetails { get; set; }
}

public enum LabBorrowingStatus
{
    Pending,
    Approved,
    Rejected
}
