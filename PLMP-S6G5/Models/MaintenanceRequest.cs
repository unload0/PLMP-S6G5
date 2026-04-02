using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PLMP_S6G5.Models;

[Table("MaintenanceRequest")]
public partial class MaintenanceRequest
{
    [Key]
    [Column("RequestID")]
    public int RequestId { get; set; }

    [Column("TenantID")]
    public int TenantId { get; set; }

    [Column("StaffID")]
    public int StaffId { get; set; }

    [StringLength(300)]
    public string? CategoryType { get; set; }

    [StringLength(300)]
    public string Priority { get; set; } = null!;

    public string? Description { get; set; }

    [StringLength(300)]
    public string? Status { get; set; }

    [ForeignKey("StaffId")]
    [InverseProperty("MaintenanceRequests")]
    public virtual MaintenanceStaff Staff { get; set; } = null!;

    [ForeignKey("TenantId")]
    [InverseProperty("MaintenanceRequests")]
    public virtual Tenant Tenant { get; set; } = null!;
}
