using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc_N_Tier_Entity.ViewModel
{
    public class AdminCloseCaseViewModel
    {
        public List<DocumentsViewModel>? Documents { get; set; }

        public int RequestId { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? ConfirmationNumber { get; set; }

        public DateOnly? DateOfBirth { get; set; }

        public string? Mobile { get; set; }

        public string? Email { get; set; }
    }
}
