using COURSEPROJECT.Model;

namespace COURSEPROJECT.Dto.Response
{
    public class CourseMaterialResponse
    {
        public int ID { get; set; }
        public string FileUrl { get; set; }
        public string Type { get; set; }
        public int CourseId { get; set; }
        
    }
}
