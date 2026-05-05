using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PLMP_S6G5.Models;

[Table("PropertyManager")]
public partial class PropertyManager
{
    [Key]
    [Column("ManagerID")]
    public int ManagerId { get; set; }

    [StringLength(300)]
    public string Name { get; set; } = null!;

    [StringLength(20)]
    public string? PhoneNumber { get; set; }

    [StringLength(300)]
    public string Email { get; set; } = null!;

    [InverseProperty("Manager")]
    public virtual ICollection<Lease> Leases { get; set; } = new List<Lease>();
}
