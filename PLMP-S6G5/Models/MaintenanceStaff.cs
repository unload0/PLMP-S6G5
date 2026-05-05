using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PLMP_S6G5.Models;

[Table("MaintenanceStaff")]
public partial class MaintenanceStaff
{
    [Key]
    [Column("StaffID")]
    public int StaffId { get; set; }

    [StringLength(300)]
    public string Name { get; set; } = null!;

    [StringLength(20)]
    public string? PhoneNumber { get; set; }

    [StringLength(300)]
    public string Email { get; set; } = null!;

    [StringLength(300)]
    public string? SkillProfile { get; set; }

    public bool Available { get; set; }

    [InverseProperty("Staff")]
    public virtual ICollection<MaintenanceRequest> MaintenanceRequests { get; set; } = new List<MaintenanceRequest>();
}
