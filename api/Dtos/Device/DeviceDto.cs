using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dtos.Device
{
    public class DeviceDto
    {
        public int Id { get; set; }
        public string DeviceName { get; set; }
        public string DeviceType { get; set; }
        public int Quantity { get; set; }
        public string DeviceStatus { get; set; }
        public bool IsAvailable { get; set; }
    }
}