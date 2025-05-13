namespace COURSEPROJECT.Dto.Response
{
    public class CategroyResponse
    {
        public int id { get; set; }
        public string name { get; set; }
        public List<CourseResponse> Courses { get; set; }
    }
}
