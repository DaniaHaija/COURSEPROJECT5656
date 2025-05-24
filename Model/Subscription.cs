using Microsoft.EntityFrameworkCore;

namespace COURSEPROJECT.Model
{
    public class Subscription
    {
       public int Id { get; set; }
        public string UserId { get; set; }
        public int CourseId { get; set; }
      public ApplicationUser User { get; set; }
        public Course Course { get; set; }

    }
}
