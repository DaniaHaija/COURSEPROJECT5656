namespace COURSEPROJECT.Dto.Response
{
    public class CourseRespon2
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public decimal Price { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public string UserId { get; set; }
        public string User { get; set; }
    }
}
