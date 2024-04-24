using HalloDoc_N_Tier_Entity.DataModels;
using System.ComponentModel.DataAnnotations;

namespace HalloDoc.Entity.ViewModels
{
    public class NewStateAdminViewModel
    {

        [Required(ErrorMessage = "Please enter the first name")]
        public string Firstname { get; set; }
        [Required(ErrorMessage = "Please enter the last name")]
        public string Lastname { get; set; }
        public string? UserId { get; set; }
        public int? Intdate { get; set; }
        public int? Intyear { get; set; }
        public string Strmonth { get; set; }
        public string RequestorFirstname { get; set; }
        public string RequestorLastname { get; set; }
        public DateTime Createddate { get; set; }
        [Required(ErrorMessage = "Please enter the phone number")]
        public string Phonenumber { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Street { get; set; }
        public string Zipcode { get; set; }
        public string Notes { get; set; }
        public int Status { get; set; }
        public int RequestTypeId { get; set; }
        [Required(ErrorMessage = "Please enter the email address")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "Please enter a valid email address")]
        public string Email { get; set; }
        public string UserName { get; set; }

        public int RequestId { get; set; }
        public string? ConfirmationNumber { get; set; }

        public DateTime dob { get; set; }
        public string Address { get; set; }
        public string TabId { get; set; }
        public List<CaseTag> caseTags { get; set; }
        public List<Region> regions { get; set; }
        public string PhysicianName { get; set; }
        public DateTime? dateofservice { get; set; }
    }
}