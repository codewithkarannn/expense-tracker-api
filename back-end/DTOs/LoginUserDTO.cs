using System.ComponentModel.DataAnnotations;

namespace Budget_Tracker_WebAPI.DTOs
{
    public class LoginUserDTO
    {

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string UserEmail { get; set; }


        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }
    }
}
