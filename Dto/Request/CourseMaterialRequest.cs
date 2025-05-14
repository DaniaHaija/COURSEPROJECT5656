using COURSEPROJECT.Model;

namespace COURSEPROJECT.Dto.Request
{
    public class CourseMaterialRequest
    {
       
        public List <IFormFile> FileUrl { get; set; }
        public List  <string> Type { get; set; }
        public int CourseId { get; set; }

       
    }
}
