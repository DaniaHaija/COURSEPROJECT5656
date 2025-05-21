using COURSEPROJECT.Model;
using System.ComponentModel.DataAnnotations;

namespace COURSEPROJECT.Dto
{
    public class RegisterSupervisorDto
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
    
        public string? Bio { get; set; }
        public string? Specialty { get; set; }

        public List<UserCertificateDto>? Certificates { get; set; }

    }
}
