using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace PLMP_S6G5.Models;

public partial class PLMPS6G5 : DbContext
{
    public PLMPS6G5()
    {
    }

    public PLMPS6G5(DbContextOptions<PLMPS6G5> options)
        : base(options)
    {
    }

    public virtual DbSet<Building> Buildings { get; set; }

    public virtual DbSet<Lease> Leases { get; set; }

    public virtual DbSet<MaintenanceRequest> MaintenanceRequests { get; set; }

    public virtual DbSet<MaintenanceStaff> MaintenanceStaffs { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<PropertyManager> PropertyManagers { get; set; }

    public virtual DbSet<Tenant> Tenants { get; set; }

    public virtual DbSet<Unit> Units { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=PLMPS6G5;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Lease>(entity =>
        {
            entity.HasOne(d => d.Manager).WithMany(p => p.Leases)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PropertyManager_TO_Lease");

            entity.HasOne(d => d.Tenant).WithMany(p => p.Leases)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Tenant_TO_Lease");

            entity.HasOne(d => d.Unit).WithMany(p => p.Leases)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Unit_TO_Lease");
        });

        modelBuilder.Entity<MaintenanceRequest>(entity =>
        {
            entity.HasOne(d => d.Staff).WithMany(p => p.MaintenanceRequests).HasConstraintName("FK_MaintenanceStaff_TO_MaintenanceRequest");

            entity.HasOne(d => d.Tenant).WithMany(p => p.MaintenanceRequests)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Tenant_TO_MaintenanceRequest");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasOne(d => d.Lease).WithMany(p => p.Payments)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Lease_TO_Payment");
        });

        modelBuilder.Entity<Unit>(entity =>
        {
            entity.HasOne(d => d.Building).WithMany(p => p.Units)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Building_TO_Unit");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
