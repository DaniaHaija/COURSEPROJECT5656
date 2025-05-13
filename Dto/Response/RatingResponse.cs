namespace COURSEPROJECT.Dto.Response
{
    public class RatingResponse
    {
        public int ID { get; set; }
        public int Score { get; set; }
        public string Comment { get; set; }
        public string UserId { get; set; }
        public int CourseId { get; set; }
    }
}
