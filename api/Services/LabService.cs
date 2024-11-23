using api.Data;
using Microsoft.EntityFrameworkCore;

public class LabService : ILabService
{
    private readonly ApplicationDbContext _context;

    public LabService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Lab> GetLabAsync()
    {
        // Lấy phòng lab duy nhất
        return await _context.Labs.FirstOrDefaultAsync();
    }

    public async Task<Lab> UpdateLabAsync(Lab lab)
    {
        // Lấy phòng lab duy nhất
        var existingLab = await _context.Labs.FirstOrDefaultAsync();

        if (existingLab == null)
        {
            return null;
        }

        // Cập nhật thông tin phòng lab
        existingLab.LabName = lab.LabName;
        existingLab.Description = lab.Description;
        existingLab.IsAvailable = lab.IsAvailable;

        await _context.SaveChangesAsync();
        return existingLab;
    }
}
