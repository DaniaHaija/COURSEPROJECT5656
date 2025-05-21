namespace COURSEPROJECT.Model
{
    public class UserCertificate
    {
        public int Id { get; set; }

        public string FileUrl { get; set; }

        public string FileName { get; set; }

        public DateTime UploadedAt { get; set; }

        public string UserId { get; set; }
     

        public ApplicationUser User { get; set; }
    }
}
