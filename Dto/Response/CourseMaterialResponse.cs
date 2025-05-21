using COURSEPROJECT.Model;

namespace COURSEPROJECT.Dto.Response
{
    public class CourseMaterialResponse
    {


        public int ID { get; set; }
        public int CourseId { get; set; }
        public List<CourseFile> Files { get; set; } = new List<CourseFile>();
        public DateTime? LiveStartTime { get; set; }

    }
}