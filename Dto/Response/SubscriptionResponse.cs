namespace COURSEPROJECT.Dto.Response
{
    public class SubscriptionResponse
    {
        public string UserId { get; set; }
        public int CourseId { get; set; }
        public string UserEmail {  get; set; }
        public string User { get; set; }
        public string CourseTitle { get; set; }
        public string CourseDescription { get; set; }
        public decimal CoursePrice { get; set; }
    }
}
