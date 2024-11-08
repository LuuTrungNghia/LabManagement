using api.Models;
using api.Dtos.Device;

namespace api.Interfaces
{
    public interface IDeviceRepository
    {
        Task<List<Device>> GetAllAsync();
        Task<Device?> GetByIdAsync(int id);
        Task<Device> CreateAsync(Device device);
        Task<Device?> UpdateAsync(int id, UpdateDeviceRequestDto deviceDto);
        Task<Device?> DeleteAsync(int id);
        Task ImportDevices(List<Device> devices);
    }
}
