using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc_N_Tier_Entity.ViewModel
{
    public class AdminViewNotesViewModel
    {
        public List<string>? TransferNotes { get; set; }

        public string? PhysicianNotes { get; set; }

        public string? AdminNotes { get; set; }

        public string? AdminAdditionalNotes { get; set; }

        public int? RequestId { get; set; }
    }
}
