using api.Models;

public class DeviceReturnDto
{
    public int DeviceId { get; set; }
    public int DeviceItemId { get; set; }
    public string Description { get; set; }
    public DeviceItemStatus Status { get; set; }
}
