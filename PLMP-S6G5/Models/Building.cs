using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PLMP_S6G5.Models;

[Table("Building")]
public partial class Building
{
    [Key]
    [Column("BuildingID")]
    public int BuildingId { get; set; }

    [StringLength(300)]
    public string? Name { get; set; }

    [StringLength(300)]
    public string? Address { get; set; }

    [InverseProperty("Building")]
    public virtual ICollection<Unit> Units { get; set; } = new List<Unit>();
}
