using Microsoft.AspNetCore.Identity;

namespace COURSEPROJECT.Model
{
    public class ApplicationUser : IdentityUser

    {
       
        public ICollection<Course> Courses { get; set; }
         public ICollection<Payment> Payments { get; set; }
        public ICollection<Subscription>Subscriptions { get; set; }
        public ICollection<Rating> Ratings { get; set; }
     
       




    }
}
