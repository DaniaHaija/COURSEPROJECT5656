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
    [Authorize(Roles = $"{StaticData.Moderator}")]
    public class CourseMaterialsController(ApplicationDbContext context) : ControllerBase
    {
        private readonly ApplicationDbContext _context = context;
        public static List<CourseMaterial> CourseMaterialsList = new List<CourseMaterial>();

        [HttpGet("")]
        [Authorize]
        public IActionResult Get()
        {
            var courseMaterials = _context.CourseMaterials
                .Include(cm => cm.CourseFiles)
                .ToList();

            if (courseMaterials == null || !courseMaterials.Any())
                return NotFound();

            var baseUrl = $"{Request.Scheme}://{Request.Host}";

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
                    FileUrl = $"{baseUrl}/Files/{f.FileUrl}",
                    CourseMaterialId = f.CourseMaterialId
                }).ToList()
            }).ToList();

            return Ok(responses);
        }

        [HttpGet("{id}")]
        [Authorize]
        public IActionResult GetById([FromRoute] int id)
        {
            var courseMaterial = _context.CourseMaterials
                .Include(cm => cm.CourseFiles)
                .FirstOrDefault(cm => cm.ID == id);

            if (courseMaterial == null)
                return NotFound();

            var baseUrl = $"{Request.Scheme}://{Request.Host}";

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
                    FileUrl = $"{baseUrl}/Files/{f.FileUrl}"
                }).ToList()
            };

            return Ok(response);
        }

        [RequestSizeLimit(100_000_000)]

        [HttpPost("")]
        public IActionResult Create([FromForm] CourseMateriaRequest request)
        {
            var UserId = User.FindFirst("id")?.Value;

            if (UserId == null)
                return Unauthorized();

            if (request.Files == null || !request.Files.Any())
                return BadRequest("يرجى رفع الملفات.");

            if (request.FileTypes == null || request.FileTypes.Count != request.Files.Count)
                return BadRequest("عدد الأنواع لا يطابق عدد الملفات.");

            var courseMaterial = new CourseMaterial
            {
                CourseId = request.CourseId,
           
                LiveStartTime =request.LiveStartTime,
            };

            for (int i = 0; i < request.Files.Count; i++)
            {
                var file = request.Files[i];
                var fileType = request.FileTypes[i];

                if (file.Length > 0)
                {
                    var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Files", fileName);

                    using (var stream = System.IO.File.Create(filePath))
                    {
                        file.CopyTo(stream);
                    }

                    courseMaterial.CourseFiles.Add(new CourseFile
                    {
                        FileName = file.FileName,
                        FileUrl = fileName,
                        FileType = fileType
                    });
                }
            }

            _context.CourseMaterials.Add(courseMaterial);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetById), new { id = courseMaterial.ID }, courseMaterial);
        }

        [RequestSizeLimit(100_000_000)] // 100 ميجابايت

        [HttpPut("{id}")]
        public IActionResult Update([FromRoute] int id, [FromForm] CourseMateriaRequest request)
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
                foreach (var file in courseMaterial.CourseFiles)
                {
                    var oldPath = Path.Combine(Directory.GetCurrentDirectory(), "Files", file.FileUrl);
                    if (System.IO.File.Exists(oldPath))
                        System.IO.File.Delete(oldPath);
                }

                _context.CourseFile.RemoveRange(courseMaterial.CourseFiles);
                courseMaterial.CourseFiles.Clear();

                for (int i = 0; i < request.Files.Count; i++)
                {
                    var file = request.Files[i];
                    var fileType = request.FileTypes[i];

                    var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Files", fileName);

                    using (var stream = System.IO.File.Create(filePath))
                    {
                        file.CopyTo(stream);
                    }

                    courseMaterial.CourseFiles.Add(new CourseFile
                    {
                        FileName = file.FileName,
                        FileType = fileType,
                        FileUrl = fileName
                    });
                }
            }

            courseMaterial.CourseId = request.CourseId;
            courseMaterial.LiveStartTime = request.LiveStartTime;

            _context.SaveChanges();

            var response = new CourseMaterialResponse
            {
                ID = courseMaterial.ID,
                CourseId = courseMaterial.CourseId,
                LiveStartTime = courseMaterial.LiveStartTime,
                Files = courseMaterial.CourseFiles.Select(f => new CourseFile
                {
                    FileName = f.FileName,
                    FileType = f.FileType,
                    FileUrl = Url.Content($"~/Files/{f.FileUrl}")
                }).ToList()
            };

            return Ok(response);
        }



        [HttpDelete("{id}")]
        public IActionResult Delete([FromRoute] int id)
        {
            var courseMaterial = _context.CourseMaterials
                .Include(cm => cm.CourseFiles)
                .FirstOrDefault(cm => cm.ID == id);

            if (courseMaterial == null)
                return NotFound();

            foreach (var file in courseMaterial.CourseFiles)
            {
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Files", file.FileUrl);
                if (System.IO.File.Exists(filePath))
                    System.IO.File.Delete(filePath);
            }

            _context.CourseFile.RemoveRange(courseMaterial.CourseFiles);
            _context.CourseMaterials.Remove(courseMaterial);
            _context.SaveChanges();

            return NoContent();
        }




    }
}

