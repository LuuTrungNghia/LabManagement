using api.Dtos.Device;
using api.Models;

namespace api.Interfaces
{
    public interface IDeviceRepository
    {
        Task<IEnumerable<Device>> GetAllAsync();
        Task<Device?> GetByIdAsync(int id);
        Task CreateAsync(Device device);
        Task<Device?> UpdateAsync(int id, UpdateDeviceRequestDto deviceDto);
        Task<Device?> DeleteAsync(int id);
        Task ImportDevices(IEnumerable<Device> devices);
        
    }
}
