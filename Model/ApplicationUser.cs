using Microsoft.AspNetCore.Identity;

namespace COURSEPROJECT.Model
{
    public enum UserType
    {
        Student,
        Moderator,
        
    }

    public class ApplicationUser : IdentityUser

    {
        public string? Bio { get; set; }
        public string? Specialty { get; set; }
        public bool? IsApproved { get; set; } = false;
        public List<UserCertificate>? Certificates { get; set; }
        public UserType UserType { get; set; }

        public ICollection<Course> Courses { get; set; }
       

        public ICollection<Subscription>Subscriptions { get; set; }
        public ICollection<Rating> Ratings { get; set; }
  






    }
}
