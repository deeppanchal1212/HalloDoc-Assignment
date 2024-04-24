using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc_N_Tier_Entity.ViewModel
{
    public class PatientConciergeRequestViewModel
    {
        [Required(ErrorMessage = "Your First Name is required")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Please enter alphabets only")]
        [StringLength(100)]
        public string ConciergeFirstName { get; set; } = null!;

        [Required(ErrorMessage = "Your Last Name is required")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Please enter alphabets only")]
        [StringLength(100)]
        public string? ConciergeLastName { get; set; }

        [Required(ErrorMessage = "Your Mobile Number is required")]
        [RegularExpression(@"\+[1-9][0-9]{11}", ErrorMessage = "Enter valid Phone number (numbers only,country code,maximum10 digits)")]
        public string? ConciergeMobile { get; set; }

        [Required(ErrorMessage = "Your Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address.")]
        [StringLength(50)]
        public string ConciergeEmail { get; set; } = null!;

        [Required(ErrorMessage = "Property Name is required")]
        [StringLength(50)]
        public string? HotelPropertyName { get; set; }

        [Required(ErrorMessage = "Street is required")]
        [StringLength(100)]
        public string? ConciergeStreet { get; set; }

        [Required(ErrorMessage = "City is required")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Please enter alphabets only")]
        [StringLength(100)]
        public string? ConciergeCity { get; set; }

        [Required(ErrorMessage = "State is required")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Please enter alphabets only")]
        [StringLength(100)]
        public string ConciergeState { get; set; } = null!;

        [Required(ErrorMessage = "Zip-code is required")]
        [RegularExpression(@"^\d{1,6}$", ErrorMessage = "Enter a valid zip code (numbers only, maximum 6 digits)")]
        [StringLength(10)]
        public string? ConciergeZipCode { get; set; }

        [StringLength(500)]
        public string? PatientSymptoms { get; set; }

        [Required(ErrorMessage = "Patient First Name is required")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Please enter alphabets only")]
        [StringLength(100)]
        public string PatientFirstName { get; set; } = null!;

        [Required(ErrorMessage = "Patient Last Name is required")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Please enter alphabets only")]
        [StringLength(100)]
        public string? PatientLastName { get; set; }

        [Required(ErrorMessage = "Patient Date of Birth is required")]
        [DataType(DataType.Date, ErrorMessage = "Please enter a valid date.")]
        public DateTime? PatientDateOfBirth { get; set; }

        [Required(ErrorMessage = "Patient Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address.")]
        [StringLength(50)]
        public string PatientEmail { get; set; } = null!;

        [Required(ErrorMessage = "Patient Mobile Number is required")]
        [RegularExpression(@"\+[1-9][0-9]{11}", ErrorMessage = "Enter valid Phone number (numbers only,country code,maximum10 digits)")]
        [StringLength(20)]
        public string? PatientMobile { get; set; }

        [StringLength(100)]
        public string? PatientRoom { get; set; }
    }
}
