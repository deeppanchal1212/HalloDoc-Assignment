using HalloDoc_N_Tier_Entity.DataModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc_N_Tier_Entity.ViewModel
{
    public class AdminProfileViewModel
    {
        public string Username { get; set; } = null!;

        public string? Password { get; set; }

        public string? Status { get; set; }

        public string? Role { get; set; }

        [Required(ErrorMessage ="First Name is Required")]
        public string AdminFirstName { get; set; } = null!;

        [Required(ErrorMessage = "Last Name is Required")]
        public string? AdminLastName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        public string AdminEmail { get; set; } = null!;

        [Required(ErrorMessage = "Confirm Email is required")]
        [Compare(nameof(AdminEmail),ErrorMessage ="Email and ConfirmEmail not match")]
        public string? AdminConfirmEmail { get; set;}

        [Required(ErrorMessage = "Confirm Email is required")]
        public string? AdminPhone { get; set;}

        [Required(ErrorMessage = "Address1 is required")]
        public string? Address1 { get; set; }

        [Required(ErrorMessage = "Address2 is required")]
        public string? Address2 { get; set; }

        [Required(ErrorMessage = "City is required")]
        public string? City { get; set; }

        [Required(ErrorMessage = "State is required")]
        public string? State { get; set; }

        [Required(ErrorMessage = "Zipcode is required")]
        public string? Zipcode { get; set; }

        [Required(ErrorMessage = "Alternate Phone Number is required")]
        public string? AdminPhone2 { get; set; }

        public List<AdminSelectedRegionViewModel>? AdminSelectedRegions { get; set; }

        public List<int>? AdminChangedRegion { get; set; }
    }
}
