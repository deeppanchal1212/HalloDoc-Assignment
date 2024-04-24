using HalloDoc_N_Tier_Entity.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc_N_Tier_Entity.ViewModel
{
    public class AdminCancleCaseViewModel
    {
        public string PatientName { get; set; } = null!;

        public List<CaseTag> CaseTags { get; set; } = null!;

        public string? AdditionalNotes { get; set; }

        public int RequestId { get; set; }

        public int CaseTagId { get; set; }
    }
}
