// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Threading.Tasks;
// using api.Helper;
// using api.Models;
// using api.Dtos.Lab;

// namespace api.Interface
// {
//     public interface ILabRepository
//     {
//         Task<IEnumerable<LabDto>> GetAllAsync();
//         Task<LabDto> GetByIdAsync(int id);
//         Task<LabDto> CreateAsync(CreateLabRequestDto labDto);
//         Task<LabDto> UpdateAsync(int id, UpdateLabRequestDto labDto);
//         Task<bool> DeleteAsync(int id);
//     }
// }
