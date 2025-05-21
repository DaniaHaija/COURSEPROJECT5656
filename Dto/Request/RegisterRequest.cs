using COURSEPROJECT.Model;
using System.ComponentModel.DataAnnotations;

namespace COURSEPROJECT.Dto.Request
{
    public class RegisterRequest
    {
      
        [MinLength(6)]
        public string UserName { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        [Compare(nameof(Password), ErrorMessage = "passsword do not match ")]
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public UserType UserType { get; set; }
        public string? Bio { get; set; }
        public string? Specialty { get; set; }

        public List<IFormFile>? CertificateFiles { get; set; }



    }
}
