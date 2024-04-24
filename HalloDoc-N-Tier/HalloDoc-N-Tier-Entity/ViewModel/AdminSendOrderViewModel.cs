using HalloDoc_N_Tier_Entity.DataModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc_N_Tier_Entity.ViewModel
{
    public class AdminSendOrderViewModel
    {
        public List<HealthProfessionalType>? ProfesionType { get; set; }

        public List<HealthProfessional>? healthProfessionals { get; set; }

        public string? BusinessContact { get; set; }

        [Required(ErrorMessage = "Email is required")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Fax Number is required")]
        public string? FaxNumber { get; set; }

        public string? Prescription { get; set; }

        public int NumberOfRefills { get; set; }

        public int VenderId { get; set; }

        public int RequestId { get; set; }
    }
}
