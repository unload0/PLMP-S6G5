USE [PLMPS6G5]
GO

SET IDENTITY_INSERT [dbo].[Building] ON 
INSERT [dbo].[Building] ([BuildingID], [Name], [Address]) VALUES (1, N'Sunset Heights', N'123 Solar Way, Phoenix, AZ')
INSERT [dbo].[Building] ([BuildingID], [Name], [Address]) VALUES (2, N'The Grand Plaza', N'456 Corporate Blvd, New York, NY')
SET IDENTITY_INSERT [dbo].[Building] OFF
GO

SET IDENTITY_INSERT [dbo].[Unit] ON 
INSERT [dbo].[Unit] ([UnitID], [BuildingID], [Type], [Size], [RentAmount], [AvailabilityStatus]) VALUES (1, 1, N'Residential', 850.00, 1200.00, N'Leased')
INSERT [dbo].[Unit] ([UnitID], [BuildingID], [Type], [Size], [RentAmount], [AvailabilityStatus]) VALUES (2, 1, N'Studio', 450.00, 800.00, N'Vacant')
INSERT [dbo].[Unit] ([UnitID], [BuildingID], [Type], [Size], [RentAmount], [AvailabilityStatus]) VALUES (3, 2, N'Commercial', 2500.00, 5000.00, N'Leased')
SET IDENTITY_INSERT [dbo].[Unit] OFF
GO

SET IDENTITY_INSERT [dbo].[PropertyManager] ON 
INSERT [dbo].[PropertyManager] ([ManagerID], [Name], [PhoneNumber], [Email]) VALUES (1, N'Sarah Miller', N'555-9999', N's.miller@propertymanagement.com')
INSERT [dbo].[PropertyManager] ([ManagerID], [Name], [PhoneNumber], [Email]) VALUES (2, N'James Wilson', N'555-8888', N'j.wilson@propertymanagement.com')
SET IDENTITY_INSERT [dbo].[PropertyManager] OFF
GO

SET IDENTITY_INSERT [dbo].[Tenant] ON 
INSERT [dbo].[Tenant] ([TenantID], [Name], [PhoneNumber], [Email]) VALUES (1, N'Alice Smith', N'555-0101', N'alice.smith@email.com')
INSERT [dbo].[Tenant] ([TenantID], [Name], [PhoneNumber], [Email]) VALUES (2, N'Bob Jones', N'555-0102', N'bob.jones@email.com')
INSERT [dbo].[Tenant] ([TenantID], [Name], [PhoneNumber], [Email]) VALUES (3, N'Charlie Brown', N'555-0103', N'charlie.brown@email.com')
SET IDENTITY_INSERT [dbo].[Tenant] OFF
GO

SET IDENTITY_INSERT [dbo].[Lease] ON 
INSERT [dbo].[Lease] ([LeaseID], [UnitID], [TenantID], [ManagerID], [ApplicationStatus], [LeaseStatus], [StartDate], [EndDate]) VALUES (1, 1, 1, 1, N'Approved', N'Active', '2024-01-01', '2025-01-01')
INSERT [dbo].[Lease] ([LeaseID], [UnitID], [TenantID], [ManagerID], [ApplicationStatus], [LeaseStatus], [StartDate], [EndDate]) VALUES (2, 3, 2, 2, N'Screening', N'Active', '2023-06-01', '2024-06-01')
SET IDENTITY_INSERT [dbo].[Lease] OFF
GO

SET IDENTITY_INSERT [dbo].[Payment] ON 
INSERT [dbo].[Payment] ([PaymentID], [LeaseID], [InstallmentAmount], [DateOfIssue], [Balance], [PaymentStatus]) VALUES (1, 1, 1200.00, '2024-03-01', 0.00, N'Paid')
INSERT [dbo].[Payment] ([PaymentID], [LeaseID], [InstallmentAmount], [DateOfIssue], [Balance], [PaymentStatus]) VALUES (2, 1, 1200.00, '2024-04-01', 1200.00, N'Overdue')
SET IDENTITY_INSERT [dbo].[Payment] OFF
GO

SET IDENTITY_INSERT [dbo].[MaintenanceStaff] ON 
INSERT [dbo].[MaintenanceStaff] ([StaffID], [Name], [PhoneNumber], [Email], [SkillProfile], [Available]) VALUES (1, N'Mike Hammer', N'555-1234', N'm.hammer@service.com', N'Plumber', 1)
INSERT [dbo].[MaintenanceStaff] ([StaffID], [Name], [PhoneNumber], [Email], [SkillProfile], [Available]) VALUES (2, N'Sparky Watts', N'555-5678', N's.watts@service.com', N'Electrician', 1)
SET IDENTITY_INSERT [dbo].[MaintenanceStaff] OFF
GO

SET IDENTITY_INSERT [dbo].[MaintenanceRequest] ON 
INSERT [dbo].[MaintenanceRequest] ([RequestID], [TenantID], [StaffID], [CategoryType], [Priority], [Description], [Status]) VALUES (1, 1, NULL, N'Plumbing', N'High', N'Kitchen sink leaking heavily.', N'Submitted')
INSERT [dbo].[MaintenanceRequest] ([RequestID], [TenantID], [StaffID], [CategoryType], [Priority], [Description], [Status]) VALUES (2, 2, NULL, N'Electricity', N'Medium', N'Flickering lights in the main hall.', N'Submitted')
SET IDENTITY_INSERT [dbo].[MaintenanceRequest] OFF
GO