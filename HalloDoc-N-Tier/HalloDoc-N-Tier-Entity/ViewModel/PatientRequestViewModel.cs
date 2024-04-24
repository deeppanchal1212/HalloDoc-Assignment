using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc_N_Tier_Entity.ViewModel
{
    public class PatientRequestViewModel
    {
        [StringLength(500)]
        public string? Symptoms { get; set; }

        [Required(ErrorMessage = "First Name is required")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Please enter alphabets only")]
        [StringLength(100)]
        public string FirstName { get; set; } = null!;

        [Required(ErrorMessage = "Last Name is required")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Please enter alphabets only")]
        [StringLength(100)]
        public string LastName { get; set; } = null!;

        [Required(ErrorMessage = "Date of Birth is required")]
        [DataType(DataType.Date, ErrorMessage = "Please enter a valid date.")]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address.")]
        [StringLength(50)]
        public string Email { get; set; } = null!;

        [RegularExpression(@"^(?=.*[A-Z])(?=.*[!@#$%^&*])(?=.*[0-9])(?=.*[a-z]).{6,}$",ErrorMessage = "Password must be at least 6 characters long, with at least one uppercase letter, one special character, one digit, and one lowercase letter.")]
        public string? Password { get; set; } = null!;

        [Compare(nameof(Password),ErrorMessage ="Confirm Password incorrect")]
        public string? ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Mobile Number is required")]
        [RegularExpression(@"\+[1-9][0-9]{11}", ErrorMessage = "Enter valid Phone number (numbers only,country code,maximum10 digits)")]
        public string Mobile { get; set; } = null!;

        [Required(ErrorMessage = "Street is required")]
        [StringLength(100)]
        public string Street { get; set; } = null!;

        [Required(ErrorMessage = "City is required")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Please enter alphabets only")]
        [StringLength(100)]
        public string City { get; set; } = null!;

        [Required(ErrorMessage = "State is required")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Please enter alphabets only")]
        [StringLength(100)]
        public string State { get; set; } = null!;

        [Required(ErrorMessage = "Zip-Code is required")]
        [RegularExpression(@"^\d{1,6}$", ErrorMessage = "Enter a valid zip code (numbers only, maximum 6 digits)")]
        [StringLength(6)]
        public string ZipCode { get; set; } = null!;

        [StringLength(100)]
        public string? Room { get; set; }

        public List<IFormFile>? file { get; set; }
    }
}
