using COURSEPROJECT.Data;
using COURSEPROJECT.Dto;
using COURSEPROJECT.Dto.Response;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Mapster;
using System.Linq;
using static System.Net.Mime.MediaTypeNames;
using COURSEPROJECT.Model;


namespace COURSEPROJECT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController(ApplicationDbContext context) : ControllerBase
    {
        private readonly ApplicationDbContext context = context;
        [HttpGet("")]
        public IActionResult Index()
        {
            var baseUrl = $"{Request.Scheme}://{Request.Host}";
            var viewModel = new DashboardViewModel
            {

                UsersCount = context.Users.Count(),
                CoursesCount = context.Courses.Count(),
                Earning = context.Orders.Sum(o => o.FinalPrice),
                TopCourses = context.Courses
    .Include(c => c.Subscriptions)
    .Include(c => c.Category)
    .Include(c => c.User)
    .Include(c => c.CourseMaterials)
        .ThenInclude(cm => cm.CourseFiles)

                    .OrderByDescending(c => c.Subscriptions.Count)
                    .Take(3).Select(c => new CourseResponse
                    {
                     ID=c.ID,
                     Title=c.Title,
                     Description=c.Description,
                        Image=c.Image,
                        Price=c.Price,
                    StartDate = c.StartDate,
                    EndDate =c.EndDate,
                        CategoryId=c.CategoryId,
                        CategoryName=c.Category.Name,
                        UserId=c.UserId,
                        UserName = c.User.UserName,
                        CourseMaterials = c.CourseMaterials.Select(cm => new CourseMaterialResponse
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




                    }).ToList(),
            };

            return Ok(viewModel);
        }
        }


    }



