// using api.Dtos.Device;
// using api.Models;

// namespace api.Mappers
// {
//     public static class DeviceMapper
//     {
//         public static DeviceDto ToDeviceDto(this Device deviceModel)
//         {
//             return new DeviceDto
//             {
//                 Id = deviceModel.Id,
//                 DeviceName = deviceModel.DeviceName,
//                 DeviceType = deviceModel.DeviceType,
//                 Quantity = deviceModel.Quantity,
//                 DeviceStatus = deviceModel.DeviceStatus,
//                 IsAvailable = deviceModel.IsAvailable
//             };
//         }

//         public static Device ToDeviceFromCreateDto(this CreateDeviceRequestDto deviceDto)
//         {
//             return new Device
//             {
//                 DeviceName = deviceDto.DeviceName,
//                 DeviceType = deviceDto.DeviceType,
//                 Quantity = deviceDto.Quantity,
//                 DeviceStatus = deviceDto.DeviceStatus,
//                 IsAvailable = deviceDto.IsAvailable,
//                 DatePurchased = DateTime.UtcNow
//             };
//         }

//         public static void UpdateDeviceFromDto(this Device device, UpdateDeviceRequestDto dto)
//         {
//             device.DeviceName = dto.DeviceName;
//             device.DeviceType = dto.DeviceType;
//             device.Quantity = dto.Quantity;
//             device.DeviceStatus = dto.DeviceStatus;
//             device.IsAvailable = dto.IsAvailable;
//         }
//     }
// }
