using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PLMP_S6G5.Models
{
    public class Application
    {
        [Key]
        public int ApplicationId { get; set; }

        public int TenantId { get; set; }

        public int UnitId { get; set; }

        public DateTime ApplicationDate { get; set; }

        public string? ApplicationStatus { get; set; }

        [ForeignKey("TenantId")]
        public virtual Tenant Tenant { get; set; } = null!;

        [ForeignKey("UnitId")]
        public virtual Unit Unit { get; set; } = null!;
    }
}