﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AssignmentEntity.DataModels;

[Table("Task")]
public partial class Task
{
    [Column(TypeName = "character varying")]
    public string? TaskName { get; set; }

    [Column(TypeName = "character varying")]
    public string? Assignee { get; set; }

    public int? CategoryId { get; set; }

    [Column(TypeName = "character varying")]
    public string? Description { get; set; }

    public DateOnly? DueDate { get; set; }

    [Column(TypeName = "character varying")]
    public string? Category { get; set; }

    [Column(TypeName = "character varying")]
    public string? City { get; set; }

    [Key]
    public int Id { get; set; }

    [ForeignKey("CategoryId")]
    [InverseProperty("Tasks")]
    public virtual Category? CategoryNavigation { get; set; }
}
