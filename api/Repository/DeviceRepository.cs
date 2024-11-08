using api.Data;
using api.Interfaces;
using api.Models;
using api.Dtos.Device;
using Microsoft.EntityFrameworkCore;

namespace api.Repository
{
    public class DeviceRepository : IDeviceRepository
    {
        private readonly ApplicationDbContext _context;
        public DeviceRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Device>> GetAllAsync() => await _context.Devices.ToListAsync();

        public async Task<Device?> GetByIdAsync(int id) => await _context.Devices.FindAsync(id);

        public async Task<Device> CreateAsync(Device device)
        {
            await _context.Devices.AddAsync(device);
            await _context.SaveChangesAsync();
            return device;
        }

        public async Task<Device?> UpdateAsync(int id, UpdateDeviceRequestDto deviceDto)
        {
            var device = await _context.Devices.FindAsync(id);
            if (device == null) return null;

            device.DeviceName = deviceDto.DeviceName;
            device.Quantity = deviceDto.Quantity;
            device.DeviceStatus = deviceDto.DeviceStatus;
            await _context.SaveChangesAsync();
            return device;
        }

        public async Task<Device?> DeleteAsync(int id)
        {
            var device = await _context.Devices.FindAsync(id);
            if (device == null) return null;

            _context.Devices.Remove(device);
            await _context.SaveChangesAsync();
            return device;
        }

        public async Task ImportDevices(List<Device> devices)
        {
            await _context.Devices.AddRangeAsync(devices);
            await _context.SaveChangesAsync();
        }
    }
}
