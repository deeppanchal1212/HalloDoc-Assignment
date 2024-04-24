using HalloDoc_N_Tier_Entity.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc_N_Tier_Entity.ViewModel
{
    public class DashboardViewModel
    {
        public int NewCount { get; set; }

        public int PendingCount { get; set; }

        public int ActiveCount { get; set; }

        public int ConcludeCount { get; set; }

        public int ToCloseCount { get; set; }

        public int UnpaidCount { get; set; }

        public List<Region>? RegionName { get; set; }
    }
}
