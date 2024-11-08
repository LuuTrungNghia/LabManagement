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
    private readonly ApplicationDbContext _context;

    public LabRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Lab>> GetAllAsync()
    {
        return await _context.Labs.ToListAsync();
    }

    public async Task<Lab> GetByIdAsync(int id)
    {
        return await _context.Labs.FindAsync(id);
    }

    public async Task CreateAsync(Lab lab)
    {
        await _context.Labs.AddAsync(lab);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Lab lab)
    {
        _context.Labs.Update(lab);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var lab = await _context.Labs.FindAsync(id);
        if (lab != null)
        {
            _context.Labs.Remove(lab);
            await _context.SaveChangesAsync();
        }
    }
}

}
