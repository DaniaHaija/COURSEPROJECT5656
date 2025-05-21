using COURSEPROJECT.Model;

namespace COURSEPROJECT.Dto.Request
{
    public class CourseMateriaRequest
    {
        public int CourseId { get; set; }

        public List<IFormFile> Files { get; set; } = new List<IFormFile>();
        public List<string> FileTypes { get; set; } = new List<string>();
        public DateTime? LiveStartTime { get; set; }
    }

}
