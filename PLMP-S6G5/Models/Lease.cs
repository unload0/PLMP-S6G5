using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PLMP_S6G5.Models;

[Table("Lease")]
public partial class Lease
{
    [Key]
    [Column("LeaseID")]
    public int LeaseId { get; set; }

    [Column("UnitID")]
    public int UnitId { get; set; }

    [Column("TenantID")]
    public int TenantId { get; set; }

    [Column("ManagerID")]
    public int ManagerId { get; set; }

    [StringLength(300)]
    public string ApplicationStatus { get; set; } = null!;

    [StringLength(300)]
    public string LeaseStatus { get; set; } = null!;

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    [ForeignKey("ManagerId")]
    [InverseProperty("Leases")]
    public virtual PropertyManager Manager { get; set; } = null!;

    [InverseProperty("Lease")]
    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    [ForeignKey("TenantId")]
    [InverseProperty("Leases")]
    public virtual Tenant Tenant { get; set; } = null!;

    [ForeignKey("UnitId")]
    [InverseProperty("Leases")]
    public virtual Unit Unit { get; set; } = null!;
}
