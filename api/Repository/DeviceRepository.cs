using api.Data;
using api.Dtos.Device;
using api.Interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repositories
{
    public class DeviceRepository : IDeviceRepository
    {
        private readonly ApplicationDbContext _context;

        public DeviceRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Device>> GetAllAsync() => await _context.Devices.ToListAsync();

        public async Task<Device?> GetByIdAsync(int id) => await _context.Devices.FindAsync(id);

        public async Task CreateAsync(Device device)
        {
            await _context.Devices.AddAsync(device);
            await _context.SaveChangesAsync();
        }

        public async Task<Device?> UpdateAsync(int id, UpdateDeviceRequestDto deviceDto)
        {
            var device = await GetByIdAsync(id);
            if (device == null) return null;

            device.DeviceName = deviceDto.DeviceName;
            device.Quantity = deviceDto.Quantity;
            device.DeviceStatus = Enum.Parse<DeviceStatus>(deviceDto.DeviceStatus);

            _context.Devices.Update(device);
            await _context.SaveChangesAsync();

            return device;
        }

        public async Task<Device?> DeleteAsync(int id)
        {
            var device = await GetByIdAsync(id);
            if (device == null) return null;

            _context.Devices.Remove(device);
            await _context.SaveChangesAsync();

            return device;
        }

        public async Task ImportDevices(IEnumerable<Device> devices)
        {
            await _context.Devices.AddRangeAsync(devices);
            await _context.SaveChangesAsync();
        }
    }
}
