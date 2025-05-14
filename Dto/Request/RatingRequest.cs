namespace COURSEPROJECT.Dto.Request
{
    public class RatingRequest
    {
        public int Score { get; set; }
        public string Comment { get; set; }
     
        public int CourseId { get; set; }

    }
}