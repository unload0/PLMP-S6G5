using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PLMP_S6G5.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Building",
                columns: table => new
                {
                    BuildingID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    Address = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Building", x => x.BuildingID);
                });

            migrationBuilder.CreateTable(
                name: "MaintenanceStaff",
                columns: table => new
                {
                    StaffID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    SkillProfile = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    Available = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaintenanceStaff", x => x.StaffID);
                });

            migrationBuilder.CreateTable(
                name: "PropertyManager",
                columns: table => new
                {
                    ManagerID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PropertyManager", x => x.ManagerID);
                });

            migrationBuilder.CreateTable(
                name: "Tenant",
                columns: table => new
                {
                    TenantID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tenant", x => x.TenantID);
                });

            migrationBuilder.CreateTable(
                name: "Unit",
                columns: table => new
                {
                    UnitID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BuildingID = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    Size = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    RentAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    AvailabilityStatus = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Unit", x => x.UnitID);
                    table.ForeignKey(
                        name: "FK_Building_TO_Unit",
                        column: x => x.BuildingID,
                        principalTable: "Building",
                        principalColumn: "BuildingID");
                });

            migrationBuilder.CreateTable(
                name: "MaintenanceRequest",
                columns: table => new
                {
                    RequestID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantID = table.Column<int>(type: "int", nullable: false),
                    StaffID = table.Column<int>(type: "int", nullable: false),
                    CategoryType = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    Priority = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaintenanceRequest", x => x.RequestID);
                    table.ForeignKey(
                        name: "FK_MaintenanceStaff_TO_MaintenanceRequest",
                        column: x => x.StaffID,
                        principalTable: "MaintenanceStaff",
                        principalColumn: "StaffID");
                    table.ForeignKey(
                        name: "FK_Tenant_TO_MaintenanceRequest",
                        column: x => x.TenantID,
                        principalTable: "Tenant",
                        principalColumn: "TenantID");
                });

            migrationBuilder.CreateTable(
                name: "Lease",
                columns: table => new
                {
                    LeaseID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UnitID = table.Column<int>(type: "int", nullable: false),
                    TenantID = table.Column<int>(type: "int", nullable: false),
                    ManagerID = table.Column<int>(type: "int", nullable: false),
                    ApplicationStatus = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    LeaseStatus = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lease", x => x.LeaseID);
                    table.ForeignKey(
                        name: "FK_PropertyManager_TO_Lease",
                        column: x => x.ManagerID,
                        principalTable: "PropertyManager",
                        principalColumn: "ManagerID");
                    table.ForeignKey(
                        name: "FK_Tenant_TO_Lease",
                        column: x => x.TenantID,
                        principalTable: "Tenant",
                        principalColumn: "TenantID");
                    table.ForeignKey(
                        name: "FK_Unit_TO_Lease",
                        column: x => x.UnitID,
                        principalTable: "Unit",
                        principalColumn: "UnitID");
                });

            migrationBuilder.CreateTable(
                name: "Payment",
                columns: table => new
                {
                    PaymentID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LeaseID = table.Column<int>(type: "int", nullable: false),
                    InstallmentAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    DateOfIssue = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Balance = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PaymentStatus = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payment", x => x.PaymentID);
                    table.ForeignKey(
                        name: "FK_Lease_TO_Payment",
                        column: x => x.LeaseID,
                        principalTable: "Lease",
                        principalColumn: "LeaseID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Lease_ManagerID",
                table: "Lease",
                column: "ManagerID");

            migrationBuilder.CreateIndex(
                name: "IX_Lease_TenantID",
                table: "Lease",
                column: "TenantID");

            migrationBuilder.CreateIndex(
                name: "IX_Lease_UnitID",
                table: "Lease",
                column: "UnitID");

            migrationBuilder.CreateIndex(
                name: "IX_MaintenanceRequest_StaffID",
                table: "MaintenanceRequest",
                column: "StaffID");

            migrationBuilder.CreateIndex(
                name: "IX_MaintenanceRequest_TenantID",
                table: "MaintenanceRequest",
                column: "TenantID");

            migrationBuilder.CreateIndex(
                name: "IX_Payment_LeaseID",
                table: "Payment",
                column: "LeaseID");

            migrationBuilder.CreateIndex(
                name: "IX_Unit_BuildingID",
                table: "Unit",
                column: "BuildingID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MaintenanceRequest");

            migrationBuilder.DropTable(
                name: "Payment");

            migrationBuilder.DropTable(
                name: "MaintenanceStaff");

            migrationBuilder.DropTable(
                name: "Lease");

            migrationBuilder.DropTable(
                name: "PropertyManager");

            migrationBuilder.DropTable(
                name: "Tenant");

            migrationBuilder.DropTable(
                name: "Unit");

            migrationBuilder.DropTable(
                name: "Building");
        }
    }
}
