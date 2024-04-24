using DocumentFormat.OpenXml.Office.CustomUI;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc_N_Tier_Entity.ViewModel
{
    public class AdminCreateRoleViewModel
    {
        public string? RoleName { get; set; }

        public int RoleId { get; set; }

        public List<HalloDoc_N_Tier_Entity.DataModels.Menu>? MenuList { get; set; }

        [Required]
        public List<int> SelectedMenuId { get; set; } = null!;
    }
}
