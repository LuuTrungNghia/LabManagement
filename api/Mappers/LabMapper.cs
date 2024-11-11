// using api.Dtos.Lab;
// using api.Models;

// namespace api.Mappers
// {
//     public static class LabMapper
//     {
//         public static LabDto ToLabDto(this Lab lab) => new LabDto
//         {
//             Id = lab.Id,
//             LabName = lab.LabName,
//             Description = lab.Description,
//             Location = lab.Location,
//             IsBorrowed = !lab.IsAvailable
//         };

//         public static Lab ToLab(this CreateLabRequestDto dto) => new Lab
//         {
//             LabName = dto.LabName,
//             Description = dto.Description,
//             Location = dto.Location,
//             IsAvailable = dto.IsAvailable
//         };

//         public static Lab ToLab(this UpdateLabRequestDto dto, Lab lab)
//         {
//             lab.LabName = dto.LabName;
//             lab.Description = dto.Description;
//             lab.Location = dto.Location;
//             lab.IsAvailable = dto.IsAvailable;
//             return lab;
//         }
//     }
// }
