using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc_N_Tier_Entity.ViewModel
{
    public class AdminShowShiftViewModel
    {
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dddd, MMM d, yyyy}")]
        public DateTime Date { get; set; }

        public List<AdminPhysicianShiftDetailsViewModel> PhysicianShiftDetails { get; set; } = null!;
    }
}
