using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc_N_Tier_Entity.ViewModel
{
    public class AdminSendAgreementViewModel
    {
        public int RequestId { get; set; }

        public int RequestTypeId { get; set; }

        public string? PhoneNumber { get; set; }

        public string Email { get; set; } = null!;
    }
}
