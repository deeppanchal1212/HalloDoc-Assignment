using HalloDoc_N_Tier_Entity.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc_N_Tier_Entity.ViewModel
{
    public class AdminAssignCaseViewModel
    {
        public List<Region>? AllRegion { get; set; }

        public List<Physician>? AllPhysician { get; set; }

        public int? RequestId { get; set; }

        public int? RegionId { get; set; }

        public int? PhysicianId { get; set; }

        public string? Description { get; set; }
    }
}
