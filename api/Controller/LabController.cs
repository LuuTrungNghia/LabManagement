using api.Data;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class LabController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly ApplicationDbContext _context;

    public LabController(IMapper mapper, ApplicationDbContext context)
    {
        _mapper = mapper;
        _context = context;
    }

    // API để xem thông tin phòng lab
    [HttpGet]
    public async Task<IActionResult> GetLabInfo()
    {
        // Giả sử chỉ có một phòng lab duy nhất với Id = 1
        var lab = await _context.Labs.FindAsync(1);

        if (lab == null)
        {
            return NotFound("Lab not found.");
        }

        var labDto = _mapper.Map<LabDto>(lab);
        return Ok(labDto);
    }

    // API để cập nhật thông tin phòng lab
    [HttpPut]
    public async Task<IActionResult> UpdateLabInfo([FromBody] UpdateLabDto updateLabDto)
    {
        // Giả sử chỉ có một phòng lab duy nhất với Id = 1
        var lab = await _context.Labs.FindAsync(1);

        if (lab == null)
        {
            return NotFound("Lab not found.");
        }

        // Cập nhật thông tin từ DTO vào entity
        _mapper.Map(updateLabDto, lab);

        // Lưu thay đổi vào cơ sở dữ liệu
        _context.Labs.Update(lab);
        await _context.SaveChangesAsync();

        return NoContent(); // Trả về trạng thái 204 - No Content nếu cập nhật thành công
    }
}