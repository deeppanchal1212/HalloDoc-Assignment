﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HalloDoc_N_Tier_Entity.DataModels;

[Table("Physician")]
public partial class Physician
{
    [Key]
    public int PhysicianId { get; set; }

    public int? AspNetUserId { get; set; }

    [StringLength(100)]
    public string FirstName { get; set; } = null!;

    [StringLength(100)]
    public string? LastName { get; set; }

    [StringLength(50)]
    public string Email { get; set; } = null!;

    [StringLength(20)]
    public string? Mobile { get; set; }

    [StringLength(500)]
    public string? MedicalLicense { get; set; }

    [StringLength(100)]
    public string? Photo { get; set; }

    [StringLength(500)]
    public string? AdminNotes { get; set; }

    [StringLength(500)]
    public string? Address1 { get; set; }

    [StringLength(500)]
    public string? Address2 { get; set; }

    [StringLength(100)]
    public string? City { get; set; }

    public int? RegionId { get; set; }

    [StringLength(10)]
    public string? Zip { get; set; }

    [StringLength(20)]
    public string? AltPhone { get; set; }

    public int? CreatedBy { get; set; }

    [Column(TypeName = "timestamp without time zone")]
    public DateTime CreatedDate { get; set; }

    public int? ModifiedBy { get; set; }

    [Column(TypeName = "timestamp without time zone")]
    public DateTime? ModifiedDate { get; set; }

    public short? Status { get; set; }

    [StringLength(100)]
    public string BusinessName { get; set; } = null!;

    [StringLength(200)]
    public string BusinessWebsite { get; set; } = null!;

    public int? RoleId { get; set; }

    [Column("NPINumber")]
    [StringLength(500)]
    public string? Npinumber { get; set; }

    [StringLength(100)]
    public string? Signature { get; set; }

    [StringLength(50)]
    public string? SyncEmailAddress { get; set; }

    public bool? IsAgreementDoc { get; set; }

    public bool? IsBackgroundDoc { get; set; }

    public bool? IsTrainingDoc { get; set; }

    public bool? IsNonDisclosureDoc { get; set; }

    public bool? IsDeleted { get; set; }

    public bool? IsLicenseDoc { get; set; }

    public bool? IsCredentialDoc { get; set; }

    public bool? IsTokenGenerate { get; set; }

    [ForeignKey("AspNetUserId")]
    [InverseProperty("PhysicianAspNetUsers")]
    public virtual AspNetUser? AspNetUser { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("PhysicianCreatedByNavigations")]
    public virtual AspNetUser? CreatedByNavigation { get; set; }

    [ForeignKey("ModifiedBy")]
    [InverseProperty("PhysicianModifiedByNavigations")]
    public virtual AspNetUser? ModifiedByNavigation { get; set; }

    [InverseProperty("Physician")]
    public virtual ICollection<PhysicianNotification> PhysicianNotifications { get; } = new List<PhysicianNotification>();

    [InverseProperty("Physician")]
    public virtual ICollection<PhysicianRegion> PhysicianRegions { get; } = new List<PhysicianRegion>();

    [InverseProperty("Physician")]
    public virtual ICollection<RequestStatusLog> RequestStatusLogPhysicians { get; } = new List<RequestStatusLog>();

    [InverseProperty("TransToPhysician")]
    public virtual ICollection<RequestStatusLog> RequestStatusLogTransToPhysicians { get; } = new List<RequestStatusLog>();

    [InverseProperty("Physician")]
    public virtual ICollection<RequestWiseFile> RequestWiseFiles { get; } = new List<RequestWiseFile>();

    [InverseProperty("Physician")]
    public virtual ICollection<Request> Requests { get; } = new List<Request>();

    [InverseProperty("Physician")]
    public virtual ICollection<Shift> Shifts { get; } = new List<Shift>();
}
