using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AssignmentEntity.DataModels;

[Table("City")]
public partial class City
{
    [Key]
    public int Id { get; set; }

    [Column("City_Name", TypeName = "character varying")]
    public string? CityName { get; set; }
}
