using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc_N_Tier_Entity.ViewModel
{
    public class PatientProfileViewModel
    {
        [StringLength(100)]
        public string FirstName { get; set; } = null!;

        [StringLength(100)]
        public string? LastName { get; set; }

        public DateOnly? DateOfBirth { get; set; }

        [StringLength(20)]
        public string? Mobile { get; set; }

        [StringLength(50)]
        public string Email { get; set; } = null!;

        [StringLength(100)]
        public string? Street { get; set; }

        [StringLength(100)]
        public string? City { get; set; }

        [StringLength(100)]
        public string? State { get; set; }

        [StringLength(10)]
        public string? ZipCode { get; set; }
    }
}
