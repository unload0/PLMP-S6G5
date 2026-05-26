
CREATE TABLE Building
(
  BuildingID INT IDENTITY(1,1)          NOT NULL,
  Name       NVARCHAR(300) NOT NULL,
  Address    NVARCHAR(300),
  CONSTRAINT PK_Building PRIMARY KEY (BuildingID)
)
GO

CREATE TABLE Lease
(
  LeaseID           INT IDENTITY(1,1)           NOT NULL,
  UnitID            INT          NOT NULL,
  TenantID          INT          NOT NULL,
  ManagerID         INT          NOT NULL,
  ApplicationStatus NVARCHAR(300) NOT NULL CHECK (ApplicationStatus IN ('Screening', 'Approved', 'Rejected')),
  LeaseStatus       NVARCHAR(300) NOT NULL CHECK (LeaseStatus IN ('Active', 'Pending', 'Termination', 'Renewal')),
  StartDate         DATETIME2    ,
  EndDate           DATETIME2    ,
  CONSTRAINT PK_Lease PRIMARY KEY (LeaseID)
)
GO

CREATE TABLE MaintenanceRequest
(
  RequestID    INT IDENTITY(1,1)           NOT NULL,
  TenantID     INT          NOT NULL,
  StaffID      INT          ,
  CategoryType NVARCHAR(300) CHECK (CategoryType IN ('Hazard', 'Electricity', 'HVAC', 'Wi-Fi', 'Plumbing', 'General')),
  Priority     NVARCHAR(300) NOT NULL CHECK (Priority IN ('Low', 'Medium', 'High')),
  Description  NVARCHAR(MAX),
  Status       NVARCHAR(300) CHECK (Status IN ('Submitted', 'Assigned', 'In Progress', 'Resolved', 'Closed')),
  CONSTRAINT PK_MaintenanceRequest PRIMARY KEY (RequestID)
)
GO

CREATE TABLE MaintenanceStaff
(
  StaffID      INT IDENTITY(1,1)           NOT NULL,
  Name         NVARCHAR(300) NOT NULL,
  PhoneNumber NVARCHAR(20),
  Email       NVARCHAR(300) NOT NULL,
  SkillProfile NVARCHAR(300) CHECK (SkillProfile IN ('Plumber', 'Electrician')),
  Available    BIT          NOT NULL,
  CONSTRAINT PK_MaintenanceStaff PRIMARY KEY (StaffID)
)
GO

CREATE TABLE Payment
(
  PaymentID         INT IDENTITY(1,1)           NOT NULL,
  LeaseID           INT          NOT NULL,
  InstallmentAmount DECIMAL(18,2)         ,
  DateOfIssue       DATETIME2    ,
  Balance           DECIMAL(18,2)       ,
  PaymentStatus     NVARCHAR(300) NOT NULL CHECK (PaymentStatus IN ('Paid', 'Overdue')),
  CONSTRAINT PK_Payment PRIMARY KEY (PaymentID)
)
GO

CREATE TABLE PropertyManager
(
  ManagerID   INT IDENTITY(1,1)           NOT NULL,
  Name        NVARCHAR(300) NOT NULL,
  PhoneNumber NVARCHAR(20),
  Email       NVARCHAR(300) NOT NULL,
  CONSTRAINT PK_PropertyManager PRIMARY KEY (ManagerID)
)
GO

CREATE TABLE Tenant
(
  TenantID    INT IDENTITY(1,1)           NOT NULL,
  Name        NVARCHAR(300) NOT NULL,
  PhoneNumber NVARCHAR(20),
  Email       NVARCHAR(300) NOT NULL,
  CONSTRAINT PK_Tenant PRIMARY KEY (TenantID)
)
GO

CREATE TABLE Unit
(
  UnitID             INT IDENTITY(1,1)           NOT NULL,
  BuildingID         INT          NOT NULL,
  Type               NVARCHAR(300) CHECK (Type IN ('Residential', 'Studio', 'Commercial')),
  Size               DECIMAL(18,2)       ,
  RentAmount         DECIMAL(18,2)       ,
  AvailabilityStatus NVARCHAR(300) NOT NULL CHECK (AvailabilityStatus IN ('Vacant', 'Pending', 'Leased')),
  CONSTRAINT PK_Unit PRIMARY KEY (UnitID)
)
GO

ALTER TABLE Unit
  ADD CONSTRAINT FK_Building_TO_Unit
    FOREIGN KEY (BuildingID)
    REFERENCES Building (BuildingID)
GO

ALTER TABLE Lease
  ADD CONSTRAINT FK_Unit_TO_Lease
    FOREIGN KEY (UnitID)
    REFERENCES Unit (UnitID)
GO

ALTER TABLE Lease
  ADD CONSTRAINT FK_Tenant_TO_Lease
    FOREIGN KEY (TenantID)
    REFERENCES Tenant (TenantID)
GO

ALTER TABLE MaintenanceRequest
  ADD CONSTRAINT FK_Tenant_TO_MaintenanceRequest
    FOREIGN KEY (TenantID)
    REFERENCES Tenant (TenantID)
GO

ALTER TABLE Payment
  ADD CONSTRAINT FK_Lease_TO_Payment
    FOREIGN KEY (LeaseID)
    REFERENCES Lease (LeaseID)
GO

ALTER TABLE MaintenanceRequest
  ADD CONSTRAINT FK_MaintenanceStaff_TO_MaintenanceRequest
    FOREIGN KEY (StaffID)
    REFERENCES MaintenanceStaff (StaffID)
GO

ALTER TABLE Lease
  ADD CONSTRAINT FK_PropertyManager_TO_Lease
    FOREIGN KEY (ManagerID)
    REFERENCES PropertyManager (ManagerID)
GO
