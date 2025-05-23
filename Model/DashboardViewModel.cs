using COURSEPROJECT.Dto.Response;
using COURSEPROJECT.Model;
using Microsoft.EntityFrameworkCore;

namespace COURSEPROJECT
{

    public class DashboardViewModel
    {
        public int UsersCount { get; set; }
        public int CoursesCount { get; set; }
        public decimal Earning { get; set; }
        public List<CourseResponse> TopCourses { get; set; }

    }
}