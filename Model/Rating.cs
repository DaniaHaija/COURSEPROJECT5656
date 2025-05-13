namespace COURSEPROJECT.Model
{
    public class Rating
    {
        public int ID { get; set; }
        public int Score { get; set; }
        public string Comment { get; set; }
        public string UserId { get; set; }
        public int CourseId { get; set; }
        public ApplicationUser User { get; set; }
        public Course Course { get; set; }
    }
}
