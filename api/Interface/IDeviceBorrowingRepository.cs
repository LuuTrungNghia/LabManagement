public interface IDeviceBorrowingRepository
{
    Task AddAsync(DeviceBorrowingRequest deviceBorrowingRequest);
    Task<List<DeviceBorrowingRequest>> GetAllAsync();
    Task<DeviceBorrowingRequest> GetByIdAsync(int id);
    Task UpdateAsync(DeviceBorrowingRequest deviceBorrowingRequest);
    Task<List<DeviceBorrowingRequest>> GetDeviceBorrowingHistory(string username);
    Task<bool> MarkDeviceAsReturned(int deviceId, int deviceItemId);
    Task<List<DeviceBorrowingRequest>> GetByUsernameAsync(string username);  
    Task<DeviceBorrowingRequest> GetByDeviceItemIdAsync(int deviceItemId); 
    Task DeleteAsync(DeviceBorrowingRequest request);
    Task<DeviceBorrowingRequest> CheckDeviceAvailability(int deviceItemId, DateTime startDate, DateTime endDate);
}
