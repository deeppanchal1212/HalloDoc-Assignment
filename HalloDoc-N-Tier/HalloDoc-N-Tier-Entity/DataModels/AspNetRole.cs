using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HalloDoc_N_Tier_Entity.DataModels;

public partial class AspNetRole
{
    [Key]
    public int Id { get; set; }

    [StringLength(256)]
    public string Name { get; set; } = null!;

    [InverseProperty("Role")]
    public virtual ICollection<AspNetUserRole> AspNetUserRoles { get; } = new List<AspNetUserRole>();
}
