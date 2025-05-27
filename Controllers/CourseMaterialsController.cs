using COURSEPROJECT.Data;
using COURSEPROJECT.Dto.Request;
using COURSEPROJECT.Dto.Response;
using COURSEPROJECT.Model;
using COURSEPROJECT.Utility;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace COURSEPROJECT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
   
    public class CourseMaterialsController(ApplicationDbContext context, CloudinaryService cloudinaryService) : ControllerBase
    {
        private readonly ApplicationDbContext _context = context;
        private readonly CloudinaryService cloudinaryService = cloudinaryService;

        [HttpGet("")]
        [Authorize]
        public IActionResult Get()
        {
            var courseMaterials = _context.CourseMaterials
                .Include(cm => cm.CourseFiles)
                .ToList();

            if (courseMaterials == null || !courseMaterials.Any())
                return NotFound();

         

            var responses = courseMaterials.Select(cm => new CourseMaterialResponse
            {
                ID = cm.ID,
                CourseId = cm.CourseId,
                LiveStartTime = cm.LiveStartTime,
                Files = cm.CourseFiles.Select(f => new CourseFile
                {
                    ID= f.ID,
                    FileName = f.FileName,
                    FileType = f.FileType,
                    FileUrl = f.FileUrl,
                    CourseMaterialId = f.CourseMaterialId
                }).ToList()
            }).ToList();

            return Ok(responses);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task  <IActionResult> GetById([FromRoute] int id)
        {
            var courseMaterial = _context.CourseMaterials
                .Include(cm => cm.CourseFiles)
                .FirstOrDefault(cm => cm.ID == id);

            if (courseMaterial == null)
                return NotFound();

          

            var response = new CourseMaterialResponse
            {
                ID = courseMaterial.ID,
                CourseId = courseMaterial.CourseId,
                LiveStartTime = courseMaterial.LiveStartTime,
                Files = courseMaterial.CourseFiles.Select(f => new CourseFile
                {
                    ID= f.ID,
                    FileName = f.FileName,
                    FileType = f.FileType,
                    FileUrl = f.FileUrl
                }).ToList()
            };

            return Ok(response);
        }

        [RequestSizeLimit(100_000_000)]

        [HttpPost]
        [Authorize(Roles = $"{StaticData.Moderator}")]
        public async Task<IActionResult> Create([FromForm] CourseMateriaRequest request)
        {
            var userId = User.FindFirst("id")?.Value;

            if (userId == null)
                return Unauthorized();

            if (request.Files == null || !request.Files.Any())
                return BadRequest("يرجى رفع الملفات.");
            for (int i = 0; i < request.Files.Count; i++)
            {
                Console.WriteLine($"File {i}: {request.Files[i]?.FileName}");
                Console.WriteLine($"Type {i}: {request.FileTypes[i]}");
            }
            if (request.FileTypes == null || request.FileTypes.Count != request.Files.Count)
                return BadRequest("عدد الأنواع لا يطابق عدد الملفات.");

            var courseMaterial = new CourseMaterial
            {
                CourseId = request.CourseId,
                LiveStartTime = request.LiveStartTime,
                CourseFiles = new List<CourseFile>() 
            };

            for (int i = 0; i < request.Files.Count; i++)
            {
                var file = request.Files[i];
                var fileType = request.FileTypes[i];
               


                if (file.Length > 0)
                {
                    
                    var fileUrl = await cloudinaryService.UploadFileAsync(file);

                    courseMaterial.CourseFiles.Add(new CourseFile
                    {
                        FileName = file.FileName,
                        FileUrl = fileUrl,
                        FileType = fileType
                    });
                }
            }

            _context.CourseMaterials.Add(courseMaterial);
            await _context.SaveChangesAsync(); 

            return CreatedAtAction(nameof(GetById), new { id = courseMaterial.ID }, courseMaterial);
        }


        [RequestSizeLimit(100_000_000)]
        [HttpPut("{id}")]
        [Authorize(Roles = $"{StaticData.Moderator}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromForm] CourseMateriaRequest request)
        {
            var courseMaterial = _context.CourseMaterials
                .Include(cm => cm.CourseFiles)
                .FirstOrDefault(cm => cm.ID == id);

            if (courseMaterial == null)
                return NotFound();

            var currentUserId = User.FindFirst("id")?.Value;

            if (currentUserId == null)
                return Unauthorized();


            if (request.Files != null && request.Files.Count > 0)
            {
                _context.CourseFile.RemoveRange(courseMaterial.CourseFiles);
                courseMaterial.CourseFiles.Clear();

                for (int i = 0; i < request.Files.Count; i++)
                {
                    var file = request.Files[i];
                    var fileType = request.FileTypes[i];
                    var fileUrl = await cloudinaryService.UploadFileAsync(file);

                    courseMaterial.CourseFiles.Add(new CourseFile
                    {
                        FileName = file.FileName,
                        FileType = fileType,
                        FileUrl = fileUrl
                    });
                }
            }

            courseMaterial.CourseId = request.CourseId;
            courseMaterial.LiveStartTime = request.LiveStartTime;

            await _context.SaveChangesAsync();

            var response = new CourseMaterialResponse
            {
                ID = courseMaterial.ID,
                CourseId = courseMaterial.CourseId,
                LiveStartTime = courseMaterial.LiveStartTime,
                Files = courseMaterial.CourseFiles.Select(f => new CourseFile
                {
                    FileName = f.FileName,
                    FileType = f.FileType,
                    FileUrl = f.FileUrl
                }).ToList()
            };

            return Ok(response);
        }


        [HttpDelete("{id}")]
        [Authorize(Roles = $"{StaticData.Moderator}")]
        public IActionResult Delete([FromRoute] int id)
        {
            var courseMaterial = _context.CourseMaterials
                .Include(cm => cm.CourseFiles)
                .FirstOrDefault(cm => cm.ID == id);

            if (courseMaterial == null)
                return NotFound();

          

            _context.CourseFile.RemoveRange(courseMaterial.CourseFiles);
            _context.CourseMaterials.Remove(courseMaterial);
            _context.SaveChanges();

            return NoContent();
        }




    }
}

