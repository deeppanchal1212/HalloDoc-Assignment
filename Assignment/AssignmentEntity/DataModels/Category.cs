using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AssignmentEntity.DataModels;

[Table("Category")]
public partial class Category
{
    [Column(TypeName = "character varying")]
    public string Name { get; set; } = null!;

    [Key]
    public int Id { get; set; }

    [InverseProperty("CategoryNavigation")]
    public virtual ICollection<Task> Tasks { get; set; } = new List<Task>();
}
