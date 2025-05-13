using System.ComponentModel.DataAnnotations;

namespace COURSEPROJECT.Dto.Request
{
    public class LogInRequest
    {
        [EmailAddress]
        public string Email { get; set; }
        public string Password { get; set; }
        public bool RememberMe { get; set; }


    }
}
