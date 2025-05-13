using COURSEPROJECT.Model;

namespace COURSEPROJECT.Dto.Request
{
    public class CourseMaterialRequest
    {
       
        public IFormFile FileUrl { get; set; }
        public string Type { get; set; }
        public int CourseId { get; set; }

       
    }
}
