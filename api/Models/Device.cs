using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models
{
    public class Device
    {
        public int Id { get; set; }
        public string DeviceName { get; set; }
        public string Quantity { get; set; }
        public string DeviceStatus { get; set; }
    }
}