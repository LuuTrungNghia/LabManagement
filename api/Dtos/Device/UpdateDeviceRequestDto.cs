using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dtos.Device
{
    public class UpdateDeviceRequestDto
    {
        public string DeviceName { get; set; }
        public string Quantity { get; set; }
        public string DeviceStatus { get; set; }
    }
}