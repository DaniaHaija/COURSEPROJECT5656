namespace COURSEPROJECT.Model
{
    public class CourseMaterial
    {
        public int ID { get; set; }
       
        public string FileUrl { get; set; } 
        public string Type { get; set; } 
        public int CourseId { get; set; }
        public Course Course { get; set; }

    }
}
