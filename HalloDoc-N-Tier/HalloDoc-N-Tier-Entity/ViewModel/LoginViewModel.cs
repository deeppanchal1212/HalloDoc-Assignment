using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc_N_Tier_Entity.ViewModel
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Email is Required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address.")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Password is Required")]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*[!@#$%^&*])(?=.*[0-9])(?=.*[a-z]).{6,}$", ErrorMessage = "Password must be at least 6 characters long, with at least one uppercase letter, one special character, one digit, and one lowercase letter.")]
        public string Password { get; set; } = null!;
    }
}
