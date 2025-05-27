using COURSEPROJECT.Model;

namespace COURSEPROJECT.Dto.Response
{
    public class CourseMaterialResponse
    {


        public int ID { get; set; }
        public int CourseId { get; set; }
        public List<CourseFileResponse> Files { get; set; } = new List<CourseFileResponse>();
        public DateTime? LiveStartTime { get; set; }

    }
}