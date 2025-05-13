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
    public class CoursesController(ApplicationDbContext context) : ControllerBase
    {
        private readonly ApplicationDbContext _context = context;

        [HttpGet("")]
        [AllowAnonymous]
        public IActionResult Get([FromQuery] string? query)
           
        {
            IQueryable<Course> courses = _context.Courses.Include(c => c.Category).Include(c => c.User);

            if (query is not null)
            {

                courses=courses.Where(course=>course.Title.Contains(query) || course.Description.Contains(query) || course.Category.Name.Contains(query));
            }
            var result = courses.ToList();
            if (!result.Any())
            {
                return NotFound();
            }
            return Ok(result.Adapt<IEnumerable<CourseResponse>>());
        }
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async  Task<IActionResult> GetById([FromRoute] int id)
        {
            var course  = await _context.Courses.Include(c=>c.CourseMaterials).FirstOrDefaultAsync(c=>c.ID==id);
            if (course == null)
            {
                return NotFound();
            }

            return Ok(course.Adapt<CourseResponse>());

        }
        [HttpPost("")]
        public IActionResult Create([FromForm] CourseRequest courserequest)
        {
            var file = courserequest.Image;
            var course = courserequest.Adapt<Course>();
            if (file != null && file.Length > 0)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Images", fileName);

                using (var stream = System.IO.File.Create(filePath))
                {
                    file.CopyTo(stream);
                }
                course.Image = fileName;
                _context.Courses.Add(course);
                _context.SaveChanges();

                return CreatedAtAction(nameof(GetById), new { id = course.ID }, course);

            }

            return BadRequest();
            
        }
        [HttpPut("{id}")]
        public IActionResult Update([FromRoute] int id, [FromForm] CourseUpdate courserequest)
        {
            var courseInDb = _context.Courses.AsNoTracking().FirstOrDefault(course => course.ID == id);
            var course = courserequest.Adapt<Course>();
            var file = courserequest.Image;
            if (courseInDb != null)
            {
                if (file != null && file.Length > 0)
                {
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Images", fileName);

                    using (var stream = System.IO.File.Create(filePath))
                    {
                        file.CopyTo(stream);
                    }
                    var OldfilePath = Path.Combine(Directory.GetCurrentDirectory(), "Images", courseInDb.Image);
                    if (System.IO.File.Exists(OldfilePath))
                    {
                        System.IO.File.Delete(OldfilePath);
                    }
                    course.Image = fileName;




                }
                else
                {
                    course.Image = courseInDb.Image;
                }
                course.ID = id;
                _context.Courses.Update(course);
                _context.SaveChanges();
                return NoContent();
            }
            return NotFound();
        }




        [HttpDelete("{id}")]
        public IActionResult Delete([FromRoute] int id)
        {
            var course = _context.Courses.Find(id);
            if (course == null) { return NotFound(); }
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Images", course.Image);


            if (System.IO.File.Exists(filePath))
                System.IO.File.Delete(filePath);
            _context.Courses.Remove(course);
            _context.SaveChanges();
            return NoContent();
        }
    }
}
