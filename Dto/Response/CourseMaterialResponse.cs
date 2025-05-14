using COURSEPROJECT.Model;

namespace COURSEPROJECT.Dto.Response
{
    public class CourseMaterialResponse
    {
        public int ID { get; set; }
        public List< string> FileUrl { get; set; }
        public List <string> Type { get; set; }
        public int CourseId { get; set; }
        
    }
}
