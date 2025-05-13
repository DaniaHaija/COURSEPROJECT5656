namespace COURSEPROJECT.Dto.Request
{
    public class CourseUpdate
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public IFormFile? Image { get; set; }
        public decimal Price { get; set; }
        public int CategoryId { get; set; }

        public string UserId { get; set; }
    }
}
