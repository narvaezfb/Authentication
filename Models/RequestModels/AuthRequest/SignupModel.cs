using System.ComponentModel.DataAnnotations;

namespace Authentication_Service.Models.RequestModels
{
	public class SignupModel
	{
        [Required(ErrorMessage = "Email is required!")]
        [EmailAddress(ErrorMessage = "Email is invalid")]
        public required string Email { get; set; }

        [Required(ErrorMessage = "Password is required!")]
        public required string Password { get; set; }

    }
}

