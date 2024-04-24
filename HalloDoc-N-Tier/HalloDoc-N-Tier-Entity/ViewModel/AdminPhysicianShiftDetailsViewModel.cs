using HalloDoc_N_Tier_Entity.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc_N_Tier_Entity.ViewModel
{
    public class AdminPhysicianShiftDetailsViewModel
    {
        public Physician PhysicianDetails { get; set; } = null!;

        public List<ShiftDetail>? PhysicianShiftDetail { get; set; }
    }
}
