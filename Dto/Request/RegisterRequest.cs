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


    }
}
