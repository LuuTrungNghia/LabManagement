using api.Data;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

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
}
