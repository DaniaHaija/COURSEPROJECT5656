namespace COURSEPROJECT.Dto.Response
{
    public class CourseResponse
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public decimal Price { get; set; }
        public int CategoryId { get; set; }
        public string Categoryname { get; set; }

        public string UserId { get; set; }
        public string User { get; set; }
        public List<CourseMaterialResponse> CourseMaterials  { get; set; }
    }
}
