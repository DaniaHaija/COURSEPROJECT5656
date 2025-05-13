namespace COURSEPROJECT.Model
{
    public class Category
    {
        public int id { get; set; }
        public string Name { get; set; }
        public ICollection<Course> Courses { get; } = new List<Course>();
    }
}
