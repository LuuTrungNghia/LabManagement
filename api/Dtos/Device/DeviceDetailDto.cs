namespace api.Dtos.Device
{
    public class DeviceDetailDto
    {
        public int Id { get; set; }
        public string DeviceName { get; set; }
        public int Total { get; set; }
        public string CategoryName { get; set; }
        public List<DeviceItemDto> DeviceItems { get; set; }
    }
}