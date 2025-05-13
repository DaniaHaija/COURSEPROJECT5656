using COURSEPROJECT.Data;
using COURSEPROJECT.Model;
using COURSEPROJECT.Services.IServices;

namespace COURSEPROJECT.Services
{
    public class CourseService(ApplicationDbContext context) : Service<Course>(context), ICourseService
    {
        private readonly ApplicationDbContext context = context;

    }
}
