using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PLMP_S6G5.Models;

[Table("Payment")]
public partial class Payment
{
    [Key]
    [Column("PaymentID")]
    public int PaymentId { get; set; }

    [Column("LeaseID")]
    public int LeaseId { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? InstallmentAmount { get; set; }

    public DateTime? DateOfIssue { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? Balance { get; set; }

    [StringLength(300)]
    public string PaymentStatus { get; set; } = null!;

    [ForeignKey("LeaseId")]
    [InverseProperty("Payments")]
    public virtual Lease Lease { get; set; } = null!;
}
