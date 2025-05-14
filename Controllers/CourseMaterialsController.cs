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
    [Authorize(Roles = $"{StaticData.Admin}")]
    public class CourseMaterialsController(ApplicationDbContext context) : ControllerBase
    {
        private readonly ApplicationDbContext _context =context;
        public static List<CourseMaterial> CourseMaterialsList = new List<CourseMaterial>();

        [HttpGet("")]
        [AllowAnonymous]
        public IActionResult Get()
        {
            var coursematerials = _context.CourseMaterials.ToList();
            if (coursematerials == null || !coursematerials.Any())
            {
                return NotFound();
            }

            var response = coursematerials.Select(coursematerial => new CourseMaterialResponse
            {
                ID = coursematerial.ID,
                CourseId = coursematerial.CourseId,
                FileUrl = coursematerial.FileUrl?.Split(';').ToList() ?? new List<string>(),
               
            }).ToList();

            return Ok(response);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public IActionResult GetById([FromRoute] int id)
        {
            var coursematerial = _context.CourseMaterials.Find(id);
            if (coursematerial == null)
            {
                return NotFound();
            }

            var response = new CourseMaterialResponse
            {
                ID = coursematerial.ID,
                CourseId = coursematerial.CourseId,
                FileUrl = coursematerial.FileUrl?.Split(';').ToList() ?? new List<string>(),
               
            };

            return Ok(response);
        }

        [HttpPost("")]
        public IActionResult Create([FromForm] CourseMaterialRequest coursematerialrequest)
        {
            if (coursematerialrequest.FileUrl != null && coursematerialrequest.FileUrl.Any())
            {
                var coursematerial = coursematerialrequest.Adapt<CourseMaterial>();

                var fileNames = new List<string>();

                foreach (var file in coursematerialrequest.FileUrl)
                {
                    if (file.Length > 0)
                    {
                        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Files", fileName);

                        using (var stream = System.IO.File.Create(filePath))
                        {
                            file.CopyTo(stream);
                        }

                        fileNames.Add(fileName);
                    }
                }

               
                coursematerial.FileUrl = string.Join(";", fileNames);
              
                
                _context.CourseMaterials.Add(coursematerial);
                _context.SaveChanges(); 
                return CreatedAtAction(nameof(GetById), new { id = coursematerial.ID }, coursematerial);
            }

            return BadRequest();
        }


       

        [HttpPut("{id}")]
        public IActionResult Update([FromRoute] int id, [FromForm] CourseMaterialRequest coursematerialrequest)
        {
            var coursematerialInDb = _context.CourseMaterials.AsNoTracking().FirstOrDefault(cm => cm.ID == id);
            if (coursematerialInDb == null)
                return NotFound();

            var coursematerial = coursematerialrequest.Adapt<CourseMaterial>();
            var newFileNames = new List<string>();

            if (coursematerialrequest.FileUrl != null && coursematerialrequest.FileUrl.Any())
            {
                
                foreach (var file in coursematerialrequest.FileUrl)
                {
                    if (file.Length > 0)
                    {
                        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Files", fileName);

                        using (var stream = System.IO.File.Create(filePath))
                        {
                            file.CopyTo(stream);
                        }

                        newFileNames.Add(fileName);
                    }
                }

                var oldFiles = coursematerialInDb.FileUrl?.Split(';') ?? Array.Empty<string>();
                foreach (var oldFile in oldFiles)
                {
                    var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Files", oldFile);
                    if (System.IO.File.Exists(oldFilePath))
                    {
                        System.IO.File.Delete(oldFilePath);
                    }
                }

                coursematerial.FileUrl = string.Join(";", newFileNames);
               
            }
            else
            {
                coursematerial.FileUrl = coursematerialInDb.FileUrl;
               
            }

            coursematerial.ID = id;

            _context.CourseMaterials.Update(coursematerial);
            _context.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete([FromRoute] int id)
        {
            var coursematerial= _context.CourseMaterials.Find(id);
            if(coursematerial == null) { return NotFound();}
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Files", coursematerial.FileUrl);


            if (System.IO.File.Exists(filePath))
                System.IO.File.Delete(filePath);
            _context.CourseMaterials.Remove(coursematerial);
            _context.SaveChanges(); 
            return NoContent();
        }

    }
      
       
    }

