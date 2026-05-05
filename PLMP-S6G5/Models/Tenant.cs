using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PLMP_S6G5.Models;

[Table("Tenant")]
public partial class Tenant
{
    [Key]
    [Column("TenantID")]
    public int TenantId { get; set; }

    [StringLength(300)]
    public string Name { get; set; } = null!;

    [StringLength(20)]
    public string? PhoneNumber { get; set; }

    [StringLength(300)]
    public string Email { get; set; } = null!;

    [InverseProperty("Tenant")]
    public virtual ICollection<Lease> Leases { get; set; } = new List<Lease>();

    [InverseProperty("Tenant")]
    public virtual ICollection<MaintenanceRequest> MaintenanceRequests { get; set; } = new List<MaintenanceRequest>();
}
