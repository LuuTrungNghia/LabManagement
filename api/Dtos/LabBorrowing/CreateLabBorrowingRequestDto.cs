using System.ComponentModel.DataAnnotations;

public class CreateLabBorrowingRequestDto
{
    [Required]
    public int LabId { get; set; }
    
    [Required]
    public string UserId { get; set; }
    
    [Required]
    public DateTime FromDate { get; set; }
    
    [Required]
    public DateTime ToDate { get; set; }
    
    public List<int> DeviceIds { get; set; } = new List<int>();
}
