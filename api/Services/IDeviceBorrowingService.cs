public interface IDeviceBorrowingService
{
    Task<DeviceBorrowingRequestDto> CreateDeviceBorrowingRequest(CreateDeviceBorrowingRequestDto requestDto);
    Task<List<DeviceBorrowingRequestDto>> GetDeviceBorrowingRequests();
    Task<DeviceBorrowingRequestDto> GetDeviceBorrowingRequestById(int id);
    Task<DeviceBorrowingRequestDto> UpdateDeviceBorrowingRequest(int id, UpdateDeviceBorrowingRequestDto requestDto);
    Task<bool> ApproveDeviceBorrowingRequest(int id); 
    Task<bool> RejectDeviceBorrowingRequest(int id);
    Task<List<DeviceBorrowingRequestHistoryDto>> GetDeviceBorrowingHistory(string username);
    Task<bool> ReturnDevice(DeviceReturnDto deviceReturnDto);
    Task<DeviceBorrowingRequest> CheckIfDeviceIsAvailable(int deviceItemId); 
    Task<bool> DeleteDeviceBorrowingRequest(int id);
}
