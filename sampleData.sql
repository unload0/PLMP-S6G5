USE [PLMPS6G5]
GO
SET IDENTITY_INSERT [dbo].[Building] ON 
GO
INSERT [dbo].[Building] ([BuildingID], [Name], [Address]) VALUES (1, N'Sunset Heights', N'123 Solar Way, Phoenix, AZ')
GO
INSERT [dbo].[Building] ([BuildingID], [Name], [Address]) VALUES (2, N'The Grand Plaza', N'456 Corporate Blvd, New York, NY')
GO
SET IDENTITY_INSERT [dbo].[Building] OFF
GO
SET IDENTITY_INSERT [dbo].[Unit] ON 
GO
INSERT [dbo].[Unit] ([UnitID], [BuildingID], [Type], [Size], [RentAmount], [AvailabilityStatus]) VALUES (1, 1, N'Residential', CAST(850.00 AS Decimal(18, 2)), CAST(1200.00 AS Decimal(18, 2)), N'Leased')
GO
INSERT [dbo].[Unit] ([UnitID], [BuildingID], [Type], [Size], [RentAmount], [AvailabilityStatus]) VALUES (2, 1, N'Studio', CAST(450.00 AS Decimal(18, 2)), CAST(800.00 AS Decimal(18, 2)), N'Vacant')
GO
INSERT [dbo].[Unit] ([UnitID], [BuildingID], [Type], [Size], [RentAmount], [AvailabilityStatus]) VALUES (3, 2, N'Commercial', CAST(2500.00 AS Decimal(18, 2)), CAST(5000.00 AS Decimal(18, 2)), N'Leased')
GO
SET IDENTITY_INSERT [dbo].[Unit] OFF
GO
SET IDENTITY_INSERT [dbo].[PropertyManager] ON 
GO
INSERT [dbo].[PropertyManager] ([ManagerID], [Name], [PhoneNumber], [Email]) VALUES (1, N'Sarah Miller', N'555-9999', N's.miller@propertymanagement.com')
GO
INSERT [dbo].[PropertyManager] ([ManagerID], [Name], [PhoneNumber], [Email]) VALUES (2, N'James Wilson', N'555-8888', N'j.wilson@propertymanagement.com')
GO
SET IDENTITY_INSERT [dbo].[PropertyManager] OFF
GO
SET IDENTITY_INSERT [dbo].[Tenant] ON 
GO
INSERT [dbo].[Tenant] ([TenantID], [PhoneNumber], [Email]) VALUES (1, N'555-0101', N'alice.smith@email.com')
GO
INSERT [dbo].[Tenant] ([TenantID], [PhoneNumber], [Email]) VALUES (2, N'555-0102', N'bob.jones@email.com')
GO
INSERT [dbo].[Tenant] ([TenantID], [PhoneNumber], [Email]) VALUES (3, N'555-0103', N'charlie.brown@email.com')
GO
SET IDENTITY_INSERT [dbo].[Tenant] OFF
GO
SET IDENTITY_INSERT [dbo].[Lease] ON 
GO
INSERT [dbo].[Lease] ([LeaseID], [UnitID], [TenantID], [ManagerID], [ApplicationStatus], [LeaseStatus], [StartDate], [EndDate]) VALUES (1, 1, 1, 1, N'Approved/Rejected', N'Active', CAST(N'2024-01-01T00:00:00.0000000' AS DateTime2), CAST(N'2025-01-01T00:00:00.0000000' AS DateTime2))
GO
INSERT [dbo].[Lease] ([LeaseID], [UnitID], [TenantID], [ManagerID], [ApplicationStatus], [LeaseStatus], [StartDate], [EndDate]) VALUES (2, 3, 2, 2, N'Approved/Rejected', N'Active', CAST(N'2023-06-01T00:00:00.0000000' AS DateTime2), CAST(N'2024-06-01T00:00:00.0000000' AS DateTime2))
GO
SET IDENTITY_INSERT [dbo].[Lease] OFF
GO
SET IDENTITY_INSERT [dbo].[Payment] ON 
GO
INSERT [dbo].[Payment] ([PaymentID], [LeaseID], [InstallmentAmount], [DateOfIssue], [Balance], [PaymentStatus]) VALUES (1, 1, CAST(1200.00 AS Decimal(18, 2)), CAST(N'2024-03-01T00:00:00.0000000' AS DateTime2), CAST(0.00 AS Decimal(18, 2)), N'Paid')
GO
INSERT [dbo].[Payment] ([PaymentID], [LeaseID], [InstallmentAmount], [DateOfIssue], [Balance], [PaymentStatus]) VALUES (2, 1, CAST(1200.00 AS Decimal(18, 2)), CAST(N'2024-04-01T00:00:00.0000000' AS DateTime2), CAST(1200.00 AS Decimal(18, 2)), N'Overdue')
GO
SET IDENTITY_INSERT [dbo].[Payment] OFF
GO
SET IDENTITY_INSERT [dbo].[MaintenanceStaff] ON 
GO
INSERT [dbo].[MaintenanceStaff] ([StaffID], [Name], [SkillProfile], [Available]) VALUES (1, N'Mike Hammer', N'Plumber', 1)
GO
INSERT [dbo].[MaintenanceStaff] ([StaffID], [Name], [SkillProfile], [Available]) VALUES (2, N'Sparky Watts', N'Electrician', 1)
GO
SET IDENTITY_INSERT [dbo].[MaintenanceStaff] OFF
GO
SET IDENTITY_INSERT [dbo].[MaintenanceRequest] ON 
GO
INSERT [dbo].[MaintenanceRequest] ([RequestID], [TenantID], [StaffID], [CategoryType], [Priority], [Description], [Status]) VALUES (1, 1, 1, N'Plumbing', N'High', N'Kitchen sink leaking heavily.', N'In Progress')
GO
INSERT [dbo].[MaintenanceRequest] ([RequestID], [TenantID], [StaffID], [CategoryType], [Priority], [Description], [Status]) VALUES (2, 2, 2, N'Electricity', N'Medium', N'Flickering lights in the main hall.', N'Submitted')
GO
SET IDENTITY_INSERT [dbo].[MaintenanceRequest] OFF
GO
