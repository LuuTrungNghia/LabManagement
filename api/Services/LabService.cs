using api.Data;
using Microsoft.EntityFrameworkCore;
using api.Models;
using System.Threading.Tasks;

public class LabService : ILabService
{
    private readonly ApplicationDbContext _context;

    public LabService(ApplicationDbContext context)
    {
        _context = context;
    }

    // Lấy phòng lab duy nhất
    public async Task<Lab> GetLabAsync()
    {
        return await _context.Labs.FirstOrDefaultAsync();
    }
}
