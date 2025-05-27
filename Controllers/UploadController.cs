using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace COURSEPROJECT.Controllers
{
   [ApiController]
[Route("api/[controller]")]
public class UploadController : ControllerBase
{
    private readonly CloudinaryService _cloudinaryService;

    public UploadController(CloudinaryService cloudinaryService)
    {
        _cloudinaryService = cloudinaryService;
    }

    [HttpPost("image")]
    public async Task<IActionResult> UploadImage([FromForm] IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("No file uploaded.");

        var url = await _cloudinaryService.UploadFileAsync(file);
        return Ok(new { Url = url });
    }
}

}
