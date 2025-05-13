using System.ComponentModel.DataAnnotations;



namespace COURSEPROJECT.Dto.Request

{
    public class ChangePassword
    {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        [Compare(nameof(NewPassword))]
        public string ConfirmPassword { get; set; }
    }
}
