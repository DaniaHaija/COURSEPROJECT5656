namespace COURSEPROJECT.Model
{
    public class Payment
    {
        public int ID { get; set; }
        public decimal Amount { get; set; }
        public string Status { get; set; }
        public string Method { get; set; }
        public DateTime Date { get; set; }
        public int CourseId { get; set; }
        public string UserId { get; set; }

        public Course Course { get; set; }
        public ApplicationUser User { get; set; }
    }
}
