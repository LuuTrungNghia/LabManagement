using api.Data;
using api.Dtos.Device;
using api.Helper;
using api.Interface;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repository
{
    public class DeviceRepository : IDeviceRepository
    {
        private readonly ApplicationDBContext _context;
        public DeviceRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<Device> CreateAsync(Device deviceModel)
        {
            await _context.Devices.AddAsync(deviceModel);
            await _context.SaveChangesAsync();
            return deviceModel;
        }

        public async Task<Device?> GetDeviceByIdAsync(int id)
        {
            return await _context.Devices.FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<List<Device>> GetAllAsync()
        {
            return await _context.Devices.ToListAsync();
        }

        public async Task<List<Device>> GetListAsync(QueryObject query)
        {
            var devices = _context.Devices.AsQueryable();

            if (!string.IsNullOrWhiteSpace(query.SortBy))
            {
                devices = query.SortBy.Equals("DeviceName", StringComparison.OrdinalIgnoreCase)
                    ? (query.IsDescending ? devices.OrderByDescending(s => s.DeviceName) : devices.OrderBy(s => s.DeviceName))
                    : devices;
            }

            var skipNumber = (query.Skip - 1) * query.Take;
            return await devices.Skip(skipNumber).Take(query.Take).ToListAsync();
        }

        public async Task<Device?> UpdateAsync(int id, UpdateDeviceRequestDto updateDto)
        {
            var existingDevice = await _context.Devices.FirstOrDefaultAsync(s => s.Id == id);

            if (existingDevice == null)
            {
                return null;
            }

            existingDevice.DeviceName = updateDto.DeviceName;
            existingDevice.DeviceType = updateDto.DeviceType;
            existingDevice.Quantity = updateDto.Quantity;
            existingDevice.DeviceStatus = updateDto.DeviceStatus;
            existingDevice.IsAvailable = updateDto.IsAvailable;

            await _context.SaveChangesAsync();
            return existingDevice;
        }

        public async Task<Device?> DeleteAsync(int id)
        {
            var deviceModel = await _context.Devices.FirstOrDefaultAsync(s => s.Id == id);

            if (deviceModel == null)
            {
                return null;
            }

            _context.Devices.Remove(deviceModel);
            await _context.SaveChangesAsync();
            return deviceModel;
        }

        public async Task<List<Device>> ImportDevicesAsync(List<CreateDeviceRequestDto> deviceDtos)
        {
            var devices = deviceDtos.Select(dto => new Device
            {
                DeviceName = dto.DeviceName,
                DeviceType = dto.DeviceType,
                Quantity = dto.Quantity,
                DeviceStatus = dto.DeviceStatus,
                IsAvailable = dto.IsAvailable
            }).ToList();

            await _context.Devices.AddRangeAsync(devices);
            await _context.SaveChangesAsync();
            return devices;
        }
    }
}