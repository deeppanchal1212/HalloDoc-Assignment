using HalloDoc_N_Tier_Entity.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc_N_Tier_Entity.ViewModel
{
    public class PatientDashboardViewModel
    {
        public DateTime? CreatedDate { get; set; }

        public int CurrentStatus { get; set; }

        public int RequestId { get; set; }

        public int filecount { get; set; }
    }
}
