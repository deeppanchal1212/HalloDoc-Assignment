using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc_N_Tier_Entity.ViewModel
{
    public class GenetareTokenViewModel
    {
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; } = null!;

        public string? Role { get; set; }

        public int User_Id { get; set; }

        public string? Username { get; set; }

    }
}
