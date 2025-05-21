using System.ComponentModel.DataAnnotations.Schema;

namespace COURSEPROJECT.Model
{
    public class CourseMaterial
    {
        public int ID { get; set; }

        public DateTime? LiveStartTime { get; set; }

        public int CourseId { get; set; }
        public Course Course { get; set; }
      

        public List<CourseFile> CourseFiles { get; set; } = new List<CourseFile>();
    }

}
