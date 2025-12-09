using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TSGTS.DataAccess.Repositories;
using TSGTS.Core.Entities;

namespace TSGTS.WebUI.Controllers.Api;

[ApiController]
[Route("api/[controller]")]
[AllowAnonymous]
public class PartsController : ControllerBase
{
    private readonly IGenericRepository<SparePart> _partRepository;

    public PartsController(IGenericRepository<SparePart> partRepository)
    {
        _partRepository = partRepository;
    }

    [HttpGet("search")]
    public async Task<IActionResult> Search([FromQuery] string q)
    {
        q ??= string.Empty;
        var results = await _partRepository.FindAsync(p =>
            p.PartName.Contains(q) || p.PartCode.Contains(q));
        return Ok(results);
    }
}
