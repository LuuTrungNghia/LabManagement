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

            // Update the device properties with the data from the DTO
            device = deviceDto.ToDevice(device); // Assuming ToDevice maps the DTO to the Device entity

            // Save the updated device to the database
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

        // Implementation of the missing method
        public async Task UpdateDeviceStatusAsync(int deviceId, string status)
        {
            // Lấy thiết bị theo deviceId
            var device = await GetByIdAsync(deviceId);
            if (device == null) return;

            // Cập nhật trạng thái cho tất cả các DeviceItems của thiết bị này (nếu có)
            foreach (var deviceItem in device.DeviceItems)
            {
                deviceItem.DeviceItemStatus = Enum.Parse<DeviceItemStatus>(status);
            }

            // Cập nhật thiết bị và lưu vào cơ sở dữ liệu
            _context.Devices.Update(device);
            await _context.SaveChangesAsync();
        }
    }
}
