// using api.Dtos;
// using api.Models;
// using api.Repositories;
// using AutoMapper;
// using System.Collections.Generic;
// using System.Threading.Tasks;

// namespace api.Services
// {
//     public class LabService : ILabService
//     {
//         private readonly ILabRepository _labRepository;
//         private readonly IMapper _mapper;

//         public LabService(ILabRepository labRepository, IMapper mapper)
//         {
//             _labRepository = labRepository;
//             _mapper = mapper;
//         }

//         public async Task<IEnumerable<LabDto>> GetAllLabsAsync()
//         {
//             var labs = await _labRepository.GetAllLabsAsync();
//             return _mapper.Map<IEnumerable<LabDto>>(labs);
//         }

//         public async Task<LabDto> GetLabByIdAsync(int id)
//         {
//             var lab = await _labRepository.GetLabByIdAsync(id);
//             if (lab == null) return null;
//             return _mapper.Map<LabDto>(lab);
//         }

//         public async Task CreateLabAsync(CreateLabDto createLabDto)
//         {
//             var lab = _mapper.Map<Lab>(createLabDto);
//             await _labRepository.AddLabAsync(lab);
//         }

//         public async Task UpdateLabAsync(int id, UpdateLabDto updateLabDto)
//         {
//             var lab = await _labRepository.GetLabByIdAsync(id);
//             if (lab == null) return;

//             _mapper.Map(updateLabDto, lab);
//             await _labRepository.UpdateLabAsync(lab);
//         }

//         public async Task DeleteLabAsync(int id)
//         {
//             var lab = await _labRepository.GetLabByIdAsync(id);
//             if (lab == null) return;

//             await _labRepository.DeleteLabAsync(lab);
//         }
//     }
// }
