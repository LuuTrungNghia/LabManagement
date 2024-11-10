using api.Data;
using api.Dtos.Device;
using api.Interfaces;
using api.Mappers;
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

        public async Task<IEnumerable<Device>> GetAllAsync() => await _context.Devices.Include(d => d.DeviceItems).ToListAsync();

        public async Task<Device?> GetByIdAsync(int id) => await _context.Devices.Include(d => d.DeviceItems).FirstOrDefaultAsync(d => d.DeviceId == id);

        public async Task CreateAsync(Device device)
        {
            _context.Devices.Add(device);
            await _context.SaveChangesAsync();
        }

        public async Task<Device?> UpdateAsync(int id, UpdateDeviceRequestDto deviceDto)
        {
            var device = await GetByIdAsync(id);
            if (device == null) return null;
            device = deviceDto.ToDevice(device);        
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
