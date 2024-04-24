using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc_N_Tier_Entity.ViewModel
{
    public class AdminViewCaseViewModel
    {
        public int Status { get; set; }

        public int RequestId { get; set; }

        public int RequestTypeId { get; set; }

        public string? ConformationNumber { get; set; }

        public string? PatientNotes { get; set; }

        public string? PatientFirstName { get; set; }

        public string? PatientLastName { get; set; }

        public DateOnly? PatientBirthDate { get; set;}

        public string? PatientMobile { get; set; }

        public string? PatientEmail { get; set; }

        public string? PatientRegion { get; set; }

        public string? PatientAddress { get; set; }

        public string? BusinessName { get; set; }

        public string? Room { get; set;}
    }
}
