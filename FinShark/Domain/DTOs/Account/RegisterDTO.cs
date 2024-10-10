using Domain.Validation;
using System.ComponentModel.DataAnnotations;

namespace Domain.DTOs.Account
{
    public class RegisterDTO
    {
        [Required]
        public string? UserName { get; set; }

        [Required]
        [EmailAddress]
        public string? Email {  get; set; }

        //[Required]
        //[MinLength(8, ErrorMessage = "Passwords Must Be At Least 8 Characters Long.")]
        //[MaxLength(50, ErrorMessage = "Passwords Cannot Be Longer Than 50 Characters.")]
        //[RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*[\d])(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,50}$",
        //    ErrorMessage = "Password Must Contain 1 LowerCase, 1 UpperCase, 1 Digit and 1 Special Character.")]
        [CustomPassword(ErrorMessage = "Password Must Be Complex.")]
        public string? Password { get; set; }

        //[Required(ErrorMessage = "Passwords Need to Be Confirmed Before SubMission.")]
        //[Compare("Password", ErrorMessage = "Passwords Do Not Match.")]
        [CustomConfirmPassword("Password")]
        public string? ConfirmPassword { get; set; }
    }
}
