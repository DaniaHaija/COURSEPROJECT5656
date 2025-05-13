namespace COURSEPROJECT.Model
{
    public class Course
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public decimal Price { get; set; }
        public int CategoryId { get; set; } 
    
        public string UserId { get; set; }
        public Category Category { get; set; }
        public IEnumerable<CourseMaterial> CourseMaterials { get; set;}
        public ApplicationUser User { get; set; }
        public ICollection<Subscription> Subscriptions { get; set; }
        public ICollection<Rating> Ratings { get; set; }


    }
}
