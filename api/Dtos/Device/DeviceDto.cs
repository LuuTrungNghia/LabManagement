namespace api.Dtos.Device
{
    public class DeviceDto
    {
        public int Id { get; set; }
        public string DeviceName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public string DeviceStatus { get; set; } = string.Empty;
    }
}
