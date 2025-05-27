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
using Microsoft.VisualBasic.FileIO;

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
                Files = cm.CourseFiles.Select(f => new CourseFileResponse
                {
                    ID= f.ID,
                    FileName = f.FileName,
                    FileType = f.FileType,
                    FileUrl = f.FileUrl,
                    CourseMaterialId=f.CourseMaterialId
                    
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
                Files = courseMaterial.CourseFiles.Select(f => new CourseFileResponse
                {
                    ID= f.ID,
                    FileName = f.FileName,
                    FileType = f.FileType,
                    FileUrl = f.FileUrl,
                     CourseMaterialId = f.CourseMaterialId
                }).ToList()
            };

            return Ok(response);
        }
        [RequestSizeLimit(100_000_000)]

        [HttpPost]
        [Authorize(Roles = $"{StaticData.Moderator}")]
        public async Task<IActionResult> Create([FromForm] CourseMateriaRequest request)
        {
            try
            {
                var userId = User.FindFirst("id")?.Value;
                if (userId == null)
                    return Unauthorized();

                if (request.Files == null || !request.Files.Any())
                    return BadRequest("يرجى رفع الملفات.");

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
                            FileType = fileType,
                            CourseMaterialId = courseMaterial.ID
                        });
                    }
                }

                _context.CourseMaterials.Add(courseMaterial);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetById), new { id = courseMaterial.ID }, new CourseMaterialResponse
                {
                    ID = courseMaterial.ID,
                    CourseId = courseMaterial.CourseId,
                    Files = courseMaterial.CourseFiles.Select(f => new CourseFileResponse
                    {
                        ID = f.ID,
                        FileName = f.FileName,
                        FileUrl = f.FileUrl,
                        FileType = f.FileType,


                          CourseMaterialId = f.CourseMaterialId
                    }).ToList()
                });
            }
            catch (Exception ex)
            {
                // يمكنك هنا تسجيل الخطأ في لوج الخاص بك إذا أردت
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    message = "حدث خطأ أثناء معالجة الطلب.",
                    detail = ex.Message
                });
            }
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
                        FileUrl = fileUrl,
                      CourseMaterialId= courseMaterial.ID
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
                Files = courseMaterial.CourseFiles.Select(f => new CourseFileResponse
                {
                    ID= f.ID,
                    FileName = f.FileName,
                    FileType = f.FileType,
                    FileUrl = f.FileUrl,
                     CourseMaterialId = f.CourseMaterialId
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

