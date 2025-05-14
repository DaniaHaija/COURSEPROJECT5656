namespace COURSEPROJECT.Dto.Request
{
    public class CourseRequest
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public IFormFile Image { get; set; }
        public decimal Price { get; set; }
        public int CategoryId { get; set; }

       
    }
}
