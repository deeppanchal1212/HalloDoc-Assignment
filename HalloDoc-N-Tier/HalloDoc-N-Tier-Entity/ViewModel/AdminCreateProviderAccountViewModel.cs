using HalloDoc_N_Tier_Entity.DataModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc_N_Tier_Entity.ViewModel
{
    public class AdminCreateProviderAccountViewModel
    {
        [Required(ErrorMessage = "Username is required")]
        public string? UserName { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string? Password { get; set; }

        public List<AspNetRole>? Role { get; set; }

        [Required(ErrorMessage ="FirstName is required")]
        public string? FirstName { get; set; }

        [Required(ErrorMessage = "LastName is required")]
        public string? LastName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Phone Number is required")]
        public string? PhoneNumber { get; set; }

        [Required(ErrorMessage = "Medical License is required")]
        public string? MedicalLicense { get; set; }

        [Required(ErrorMessage = "NPI Number is required")]
        public string? NPINumber { get; set; }

        public List<AdminSelectedRegionViewModel>? SelectedRegions { get; set; }

        [Required(ErrorMessage = "Address1 is required")]
        public string? Address1 { get; set; }

        public string? Address2 { get; set; }

        [Required(ErrorMessage = "City is required")]
        public string? City { get; set; }

        public List<Region>? Region { get; set; }

        public Region? State { get; set; }

        [Required(ErrorMessage = "Zipcode is required")]
        public string? Zipcode { get; set; }

        [Required(ErrorMessage = "Alternate Phone Number is required")]
        public string? PhoneNumber2 { get; set; }

        [Required(ErrorMessage = "Business Name is required")]
        public string? BusinessName { get; set; }

        [Required(ErrorMessage = "Business Website is required")]
        public string? BusinessWebsite { get; set; }

        [Required(ErrorMessage = "Photo is required")]
        public IFormFile? Photo { get; set; }

        [Required(ErrorMessage = "Notes is required")]
        public string? AdminNotes { get; set; }

        public IFormFile? IndependentContractorAgrement { get; set; }

        public IFormFile? BackgroundCheck { get; set; }

        public IFormFile? HIPAACompliance { get; set; }

        public IFormFile? NonDisclosureAgreement { get; set; }

        public int? RegionId { get; set; }

        public int? RoleId { get; set; }

        [Required(ErrorMessage = "Region is required")]
        public List<int>? SelectedRegion { get; set; }
    }
}
