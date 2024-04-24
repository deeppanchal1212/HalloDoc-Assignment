using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc_N_Tier_Entity.ViewModel
{
    public class AdminProviderInfoViewModel
    {
        public Boolean StopNotification { get; set; }

        public string PhisicianName { get; set; } = null!;

        public string Role { get; set; } = null!;

        public string OnCallStatus { get; set; } = null!;

        public string Status { get; set; } = null!;

        public int PhysicianId { get; set; }
    }
}
