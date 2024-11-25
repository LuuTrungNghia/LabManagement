namespace api.Dtos.Device
{
    public class DeviceDto
    {
        public int Id { get; set; }
        public string DeviceName { get; set; } = string.Empty;
        public int Total { get; set; }
        public string CategoryName { get; set; }
    }
}
