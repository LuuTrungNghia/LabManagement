using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Device;
using api.Helper;
using api.Models;

namespace api.Interface
{
    public interface IDeviceRepository
    {
        Task<List<Device>> GetAllAsync();
        Task<Device?> GetDeviceByIdAsync(int id);
        Task<List<Device>> GetListAsync(QueryObject query);
        Task<Device> CreateAsync(Device deviceModel);
        Task<Device?> UpdateAsync(int id, UpdateDeviceRequestDto updateDto);
        Task<Device?> DeleteAsync(int id);
        Task<List<Device>> ImportDevicesAsync(List<CreateDeviceRequestDto> deviceDtos);
    }
}