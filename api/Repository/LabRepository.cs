using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos.Lab;
using api.Interface;
using api.Mappers;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repositories
{
    public class LabRepository : ILabRepository
    {
        private readonly ApplicationDBContext _context;

        public LabRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<LabDto>> GetAllAsync()
        {
            return await _context.Labs
                .Select(lab => lab.ToDto())
                .ToListAsync();
        }

        public async Task<LabDto> GetByIdAsync(int id)
        {
            var lab = await _context.Labs.FindAsync(id);
            return lab?.ToDto();
        }

        public async Task<LabDto> CreateAsync(CreateLabRequestDto labDto)
        {
            var lab = labDto.ToModel();
            _context.Labs.Add(lab);
            await _context.SaveChangesAsync();
            return lab.ToDto();
        }

        public async Task<LabDto> UpdateAsync(int id, UpdateLabRequestDto labDto)
        {
            var lab = await _context.Labs.FindAsync(id);
            if (lab == null) return null;

            lab.UpdateModel(labDto);
            await _context.SaveChangesAsync();
            return lab.ToDto();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var lab = await _context.Labs.FindAsync(id);
            if (lab == null) return false;

            _context.Labs.Remove(lab);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
