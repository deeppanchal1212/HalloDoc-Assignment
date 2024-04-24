using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc_N_Tier_Entity.ViewModel
{
    public class AdminAllCaseViewModel
    {
        public int? RequestId { get; set; }

        public string? Email { get; set; }

        public string? TypeOfRequestor { get; set; }

        public int? ReqToDisplay { get; set; }

        public int? ReqTypeId { get; set; }

        [StringLength(100)]
        public string FirstName { get; set; } = null!;

        [StringLength(100)]
        public string? LastName { get; set; }

        public DateOnly? DateOfBirth { get; set; }

        public string? RequestorName { get; set; }

        public DateTime? RequestedDate { get; set; }

        public string? Mobile { get; set; }

        public string? RequestorMobile { get; set; }

        public string? Address { get; set; }

        public string? Notes { get; set; }

        public List<string>? TransferNotes { get; set; }

        public string? PhysicianName { get; set; }

        public DateTime? DateOfService { get; set; }

        public string? Region { get; set; }

        public int? CurrentPage { get; set; }

        public int? TotalPages { get; set; }

        public bool PreviousPage { get; set; }

        public bool NextPage { get; set; }

    }
}
