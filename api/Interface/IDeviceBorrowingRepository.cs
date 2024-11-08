using api.Dtos.DeviceBorrowing;
using api.Models;

namespace api.Interfaces
{
    public interface IDeviceBorrowingRepository
    {
        Task<DeviceBorrowing?> GetByIdAsync(int id);
        Task<IEnumerable<DeviceBorrowing>> GetByUserIdAsync(int userId);
        Task CreateAsync(DeviceBorrowing borrowing);
        Task<DeviceBorrowing?> UpdateAsync(int id, UpdateDeviceBorrowingRequestDto borrowingDto);
        Task<DeviceBorrowing?> ApproveAsync(int id);
        Task<DeviceBorrowing?> ReturnAsync(int id, DeviceStatus status);
    }
}
