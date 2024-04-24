using HalloDoc_N_Tier_Entity.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc_N_Tier_Entity.ViewModel
{
    public class AdminSchedulingViewModel
    {
        public List<Region>? RegionList { get; set; }

        public List<Physician>? PhysicsList { get; set; }

        public DateTime ShiftDate { get; set; }

        public TimeOnly StartTime { get; set; }

        public TimeOnly EndTime { get; set; }

        public int SelectedRegionId { get; set; }

        public int SelectedPhysicianId { get; set; }

        public List<int>? SelectedDays { get; set; }

        public int NumberOfTimesToRepeat { get; set; }

        //public string? Message { get; set; }
    }
}
