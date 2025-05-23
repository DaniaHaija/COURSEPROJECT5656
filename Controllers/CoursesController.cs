using COURSEPROJECT.Data;
using COURSEPROJECT.Dto.Request;
using COURSEPROJECT.Dto.Response;
using COURSEPROJECT.Model;
using COURSEPROJECT.Utility;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace COURSEPROJECT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
  
    public class CoursesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IEmailSender emailSender) : ControllerBase
    {
        private readonly ApplicationDbContext _context = context;
        private readonly UserManager<ApplicationUser> userManager = userManager;
        private readonly IEmailSender emailSender = emailSender;

        [HttpGet("")]
        [AllowAnonymous]
        public IActionResult Get([FromQuery] string? query)
        {
            IQueryable<Course> courses = _context.Courses
                .Include(c => c.Category)
                .Include(c => c.User).Where(c => c.IsApproved);

            if (!string.IsNullOrWhiteSpace(query))
            {
                if (decimal.TryParse(query, out decimal parsedPrice))
                {
                    courses = courses.Where(course => course.Price >= parsedPrice);
                }
                else
                {
                    courses = courses.Where(course =>
                        course.Title.Contains(query) ||
                        course.Description.Contains(query) ||
                        course.Category.Name.Contains(query)
                    );
                }
            }
      


         

            if (courses is null)
            {
                return NotFound();
            }
            var baseUrl = $"{Request.Scheme}://{Request.Host}/Images/";
            var mappCourse = courses.ToList().Select(r =>
            {
                var dto = r.Adapt<CourseResponse>();
                dto.Image = baseUrl + r.Image;
                return dto;
            });

            return Ok(mappCourse);
        }
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var course = await _context.Courses
                .Include(c => c.CourseMaterials)
              .ThenInclude(c => c.CourseFiles)
                .Include(c => c.Category)
                .Include(c => c.User)
                .FirstOrDefaultAsync(c => c.ID == id);

            if (course == null)
                return NotFound(new { message = "Course not found" });
            var baseUrl = $"{Request.Scheme}://{Request.Host}";
            var response = new CourseResponse
            {
                ID = course.ID,
                Title = course.Title,
                Description = course.Description,
                Image = course.Image,
                Price = course.Price,
                StartDate= course.StartDate,
                EndDate= course.EndDate,
                CategoryId = course.CategoryId,
                CategoryName = course.Category?.Name,
                UserId = course.UserId,
                User = course.User?.UserName,
                CourseMaterials = course.CourseMaterials.Select(cm => new CourseMaterialResponse
                {
                    ID = cm.ID,
                    CourseId = cm.CourseId,
                    LiveStartTime = cm.LiveStartTime,
                    Files = cm.CourseFiles.Select(f => new CourseFile
                    {
                        ID = f.ID,
                        FileName = f.FileName,
                        FileType = f.FileType,
                        FileUrl = $"{baseUrl}/Files/{f.FileUrl}",
                        CourseMaterialId = f.CourseMaterialId
                    }).ToList()
                }).ToList()
            };
        

            return Ok(response);
        }


        [HttpGet("Moderator")]
        [Authorize(Roles = $"{StaticData.Moderator}")]
        public async Task<IActionResult> GetCourseModerator()
        {
            var appUser = User.FindFirst("id")?.Value;

            var courses = await _context.Courses
                .Include(c => c.User)
                .Include(c => c.Category)
                .Include(c => c.CourseMaterials)
                .ThenInclude(c => c.CourseFiles)
                .Where(c => c.UserId == appUser)
                .ToListAsync();

            if (!courses.Any())
            {
                return NotFound(new { message = "لا يوجد كورسات لهذا المستخدم." });
            }
            var baseUrl = $"{Request.Scheme}://{Request.Host}/Images/";


            var response = courses.Select(course => new CourseResponse
            {
                ID = course.ID,
                Title = course.Title,
                Description = course.Description,
                Image = course.Image,
                Price = course.Price,
                StartDate = course.StartDate,
                EndDate = course.EndDate,
                CategoryId = course.CategoryId,
                CategoryName = course.Category?.Name,
                UserId = course.UserId,
                User = course.User?.UserName,
                CourseMaterials = course.CourseMaterials.Select(cm => new CourseMaterialResponse
                {
                    ID = cm.ID,
                    CourseId = cm.CourseId,
                    LiveStartTime = cm.LiveStartTime,
                    Files = cm.CourseFiles.Select(f => new CourseFile
                    {
                        ID = f.ID,
                        FileName = f.FileName,
                        FileType = f.FileType,
                        FileUrl = $"{baseUrl}/Files/{f.FileUrl}",
                        CourseMaterialId = f.CourseMaterialId
                    }).ToList()
                }).ToList()
            });

            return Ok(response);
        }
        [HttpPost("")]
        [Authorize(Roles = $"{StaticData.Moderator}")]
        public async Task<IActionResult> Create([FromForm] CourseRequest courserequest)
        {
            var userId = User.FindFirst("id")?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User ID not found in token.");
            }

            var userapp = await context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (userapp == null)
            {
                return NotFound("User not found.");
            }

            if (userapp.IsApproved != true)

            {
                return BadRequest("You are not approved by the admin to add courses.");
            }

            var file = courserequest.Image;
            if (file == null || file.Length == 0)
            {
                return BadRequest("Image file is required.");
            }

            var course = courserequest.Adapt<Course>();
            course.IsApproved = false;
            course.UserId = userId;

            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Images", fileName);

            using (var stream = System.IO.File.Create(filePath))
            {
                file.CopyTo(stream);
            }

            course.Image = fileName;
            _context.Courses.Add(course);
            await _context.SaveChangesAsync();

            var students = await userManager.GetUsersInRoleAsync(StaticData.Student);
            foreach (var student in students)
            {
                await emailSender.SendEmailAsync(
                    student.Email,
                    "New Course Added",
                    $@"
<div style='font-family: Arial, sans-serif; padding: 20px; background-color: #f9f9f9; color: #333;'>
    <h1 style='color: #4CAF50;'>Hello, {student.Email}</h1>
    <p style='font-size: 16px;'>We are excited to inform you that a new course has been added to <strong>Hup Academy</strong>.</p>
    <p style='font-size: 14px;'>Visit the platform to check out the latest courses and start learning today!</p>
    <p style='font-size: 12px; color: #888;'>If you have any questions, feel free to contact us.</p>
</div>"
                );
            }

            return CreatedAtAction(nameof(GetById), new { id = course.ID }, course.Adapt<CourseRespon2>());
        }


        [HttpPut("{id}")]
        [Authorize(Roles = $"{StaticData.Moderator}")]

        public IActionResult Update([FromRoute] int id, [FromForm] CourseUpdate courserequest)
        {
            var userId = User.FindFirst("id")?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User ID not found in token.");
            }



            var courseInDb = _context.Courses.AsNoTracking().FirstOrDefault(c => c.ID == id);
            if (courseInDb == null)
            {
                return NotFound("Course not found.");
            }


            if (courseInDb.UserId != userId)
            {
                return BadRequest("You are not allowed to edit this course.");
            }

            var course = courserequest.Adapt<Course>();
            course.ID = id;
            course.UserId = userId;

            var file = courserequest.Image;
            if (file != null && file.Length > 0)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Images", fileName);

                using (var stream = System.IO.File.Create(filePath))
                {
                    file.CopyTo(stream);
                }

                var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Images", courseInDb.Image);
                if (System.IO.File.Exists(oldFilePath))
                {
                    System.IO.File.Delete(oldFilePath);
                }

                course.Image = fileName;
            }
            else
            {
                course.Image = courseInDb.Image;
            }

            _context.Courses.Update(course);
            _context.SaveChanges();

            return NoContent();
        }





        [HttpDelete("{id}")]
        [Authorize(Roles = $"{StaticData.Moderator}")]
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

        [HttpGet("PendingCourses")]
        [Authorize(Roles = $"{StaticData.Admin}")]
        public IActionResult GetPendingCourses()
        {

            var courses = _context.Courses.Where(c => !c.IsApproved).ToList();
            return Ok(courses);




        }
       
        [HttpPut("ApproveCourse/{id}")]
        [Authorize(Roles = $"{StaticData.Admin}")]
        public async Task<IActionResult> ApproveCourse( [FromRoute] int id)
        {

            var course = await _context.Courses.FirstOrDefaultAsync(c => c.ID == id);
            if (course == null) { return NotFound(); }
            else
            {
                course.IsApproved = true;
                await _context.SaveChangesAsync();
                return Ok(new { message = "تم اعتماد الكورس" });
            }

        }


    }
}
