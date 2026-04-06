using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore;

namespace PLMP_S6G5.Models;

[Table("Unit")]
public partial class Unit
{
    [Key]
    [Column("UnitID")]
    public int UnitId { get; set; }

    [Column("BuildingID")]
    public int BuildingId { get; set; }

    [StringLength(300)]
    public string? Type { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? Size { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? RentAmount { get; set; }

    [StringLength(300)]
    public string AvailabilityStatus { get; set; } = null!;

    [ForeignKey("BuildingId")]
    [InverseProperty("Units")]
    [ValidateNever]
    public virtual Building Building { get; set; } = null!;

    [InverseProperty("Unit")]
    [ValidateNever]
    public virtual ICollection<Lease> Leases { get; set; } = new List<Lease>();
}