USE [master]
GO
/****** Object:  Database [FinalProjectDB]    Script Date: 10/05/2024 2:29:31 pm ******/
CREATE DATABASE [FinalProjectDB]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'FinalProjectDB', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.MSSQLSERVER\MSSQL\DATA\FinalProjectDB.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'FinalProjectDB_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.MSSQLSERVER\MSSQL\DATA\FinalProjectDB_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO
ALTER DATABASE [FinalProjectDB] SET COMPATIBILITY_LEVEL = 160
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [FinalProjectDB].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [FinalProjectDB] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [FinalProjectDB] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [FinalProjectDB] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [FinalProjectDB] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [FinalProjectDB] SET ARITHABORT OFF 
GO
ALTER DATABASE [FinalProjectDB] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [FinalProjectDB] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [FinalProjectDB] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [FinalProjectDB] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [FinalProjectDB] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [FinalProjectDB] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [FinalProjectDB] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [FinalProjectDB] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [FinalProjectDB] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [FinalProjectDB] SET  DISABLE_BROKER 
GO
ALTER DATABASE [FinalProjectDB] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [FinalProjectDB] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [FinalProjectDB] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [FinalProjectDB] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [FinalProjectDB] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [FinalProjectDB] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [FinalProjectDB] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [FinalProjectDB] SET RECOVERY FULL 
GO
ALTER DATABASE [FinalProjectDB] SET  MULTI_USER 
GO
ALTER DATABASE [FinalProjectDB] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [FinalProjectDB] SET DB_CHAINING OFF 
GO
ALTER DATABASE [FinalProjectDB] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [FinalProjectDB] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [FinalProjectDB] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [FinalProjectDB] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'FinalProjectDB', N'ON'
GO
ALTER DATABASE [FinalProjectDB] SET QUERY_STORE = ON
GO
ALTER DATABASE [FinalProjectDB] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [FinalProjectDB]
GO
/****** Object:  Table [dbo].[Lookup]    Script Date: 10/05/2024 2:29:32 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Lookup](
	[lookupID] [int] IDENTITY(1,1) NOT NULL,
	[Value] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](255) NOT NULL,
 CONSTRAINT [PK_Lookup] PRIMARY KEY CLUSTERED 
(
	[lookupID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Person]    Script Date: 10/05/2024 2:29:32 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Person](
	[PersonID] [int] IDENTITY(1,1) NOT NULL,
	[Gender] [int] NOT NULL,
	[CNIC] [nvarchar](13) NOT NULL,
	[FirstName] [nvarchar](50) NOT NULL,
	[LastName] [nvarchar](50) NULL,
	[Email] [nvarchar](255) NOT NULL,
	[PrimaryPhone] [nvarchar](15) NOT NULL,
	[AlternatePhone] [nvarchar](15) NULL,
	[DateOfBirth] [datetime] NOT NULL,
	[Address] [nvarchar](255) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[PersonID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Applicant]    Script Date: 10/05/2024 2:29:32 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Applicant](
	[ApplicantID] [int] NOT NULL,
	[DesiredDesignation] [int] NOT NULL,
	[isSelected] [bit] NULL,
	[isShortlisted] [bit] NULL,
	[isRejected] [bit] NULL,
	[CVName] [nvarchar](50) NULL,
	[PictureName] [nvarchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[ApplicantID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[ViewApplicants]    Script Date: 10/05/2024 2:29:32 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[ViewApplicants] AS
  SELECT *
  FROM Person p 
  JOIN Applicant a ON p.PersonID = a.ApplicantID
  JOIN Lookup L ON a.DesiredDesignation = L.lookupID
GO
/****** Object:  Table [dbo].[Employee]    Script Date: 10/05/2024 2:29:32 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Employee](
	[EmployeeID] [int] NOT NULL,
	[DesignationID] [int] NOT NULL,
	[UpdatedBy] [int] NULL,
	[UpdatedOn] [date] NULL,
	[EmployeeNumber] [nvarchar](50) NOT NULL,
	[StartDate] [date] NOT NULL,
	[EndDate] [date] NULL,
	[IsActive] [bit] NOT NULL,
	[BaseSalary] [decimal](8, 2) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[EmployeeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[ViewEmployee]    Script Date: 10/05/2024 2:29:32 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
  CREATE VIEW [dbo].[ViewEmployee] AS
  SELECT *
  FROM Employee E
  JOIN Person P ON P.PersonID = E.EmployeeID
  JOIN Lookup l ON E.DesignationID = l.lookupID
GO
/****** Object:  Table [dbo].[Item]    Script Date: 10/05/2024 2:29:32 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Item](
	[ItemID] [int] IDENTITY(1,1) NOT NULL,
	[ItemName] [nvarchar](100) NOT NULL,
	[AvailableQuantity] [int] NOT NULL,
	[Description] [nvarchar](300) NULL,
	[MeasurementUnit] [nvarchar](50) NOT NULL,
	[SalePrice] [decimal](8, 2) NOT NULL,
	[UpdatedBy] [int] NULL,
	[UpdatedOn] [date] NULL,
PRIMARY KEY CLUSTERED 
(
	[ItemID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[ViewItems]    Script Date: 10/05/2024 2:29:32 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[ViewItems] AS
 SELECT * 
 FROM Item
GO
/****** Object:  Table [dbo].[Salary]    Script Date: 10/05/2024 2:29:32 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Salary](
	[EmployeeID] [int] NOT NULL,
	[PaidDate] [date] NOT NULL,
	[IsPaid] [bit] NOT NULL,
	[PaidAmount] [decimal](8, 2) NULL,
	[Salary] [decimal](8, 2) NOT NULL,
	[ExpectedDate] [date] NULL,
	[UpdatedBy] [int] NULL,
	[UpdatedOn] [date] NULL,
PRIMARY KEY CLUSTERED 
(
	[EmployeeID] ASC,
	[PaidDate] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[ViewSalary]    Script Date: 10/05/2024 2:29:32 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 CREATE VIEW [dbo].[ViewSalary] AS
 SELECT E.EmployeeID, PaidDate, PaidAmount, IsPaid, IsActive, Salary, BaseSalary, ExpectedDate, DesignationID, l.Value AS Designation, Gender, l2.Value AS GenderValue,EmployeeNumber, StartDate, EndDate, FirstName, LastName, Email, PrimaryPhone, AlternatePhone, Address, DateOfBirth
 FROM Salary S
 JOIN Employee E ON S.EmployeeID = E.EmployeeID
 JOIN Applicant A ON A.ApplicantID = E.EmployeeID
 JOIN Person P ON P.PersonID = A.ApplicantID
 JOIN Lookup l ON l.lookupID = DesignationID
 JOIN Lookup l2 ON l2.lookupID = Gender
GO
/****** Object:  Table [dbo].[Client]    Script Date: 10/05/2024 2:29:32 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Client](
	[ClientID] [int] IDENTITY(1,1) NOT NULL,
	[ClientName] [nvarchar](100) NOT NULL,
	[City] [nvarchar](50) NULL,
	[ClientEmail] [nvarchar](255) NOT NULL,
	[PhoneNumber] [nvarchar](15) NULL,
	[UpdatedBy] [int] NULL,
	[UpdatedOn] [date] NULL,
PRIMARY KEY CLUSTERED 
(
	[ClientID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Credentials]    Script Date: 10/05/2024 2:29:32 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Credentials](
	[Username] [nvarchar](50) NOT NULL,
	[Password] [nvarchar](14) NOT NULL,
	[EmployeeID] [int] NOT NULL,
	[UpdatedBy] [int] NULL,
	[UpdatedOn] [date] NULL,
PRIMARY KEY CLUSTERED 
(
	[Username] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Delivery]    Script Date: 10/05/2024 2:29:32 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Delivery](
	[DeliveryID] [int] IDENTITY(1,1) NOT NULL,
	[VehicleID] [int] NOT NULL,
	[EmployeeID] [int] NOT NULL,
	[DriverID] [int] NOT NULL,
	[ClientID] [int] NOT NULL,
	[DeliveryStatus] [int] NOT NULL,
	[DeliveryDate] [date] NULL,
	[UpdatedBy] [int] NULL,
	[UpdatedOn] [date] NULL,
	[Payment] [decimal](8, 2) NULL,
PRIMARY KEY CLUSTERED 
(
	[DeliveryID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Driver]    Script Date: 10/05/2024 2:29:32 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Driver](
	[DriverID] [int] IDENTITY(1,1) NOT NULL,
	[DriverName] [nvarchar](100) NULL,
	[DriverCNIC] [nvarchar](13) NOT NULL,
	[LicenseNumber] [nvarchar](11) NOT NULL,
	[UpdatedBy] [int] NULL,
	[UpdatedOn] [date] NULL,
	[LicenseExpiryDate] [date] NULL,
PRIMARY KEY CLUSTERED 
(
	[DriverID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EmergencyContact]    Script Date: 10/05/2024 2:29:32 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EmergencyContact](
	[EmergencyContactID] [int] NOT NULL,
	[EmployeeID] [int] NOT NULL,
	[Relationship] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[EmployeeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Order]    Script Date: 10/05/2024 2:29:32 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Order](
	[OrderID] [int] IDENTITY(1,1) NOT NULL,
	[QuotationID] [int] NOT NULL,
	[ItemID] [int] NULL,
	[ClientID] [int] NOT NULL,
	[UpdatedBy] [int] NULL,
	[UpdatedOn] [date] NULL,
	[RequiredDate] [date] NULL,
	[OrderDate] [date] NOT NULL,
	[IsServed] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[OrderID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[OtherExpenses]    Script Date: 10/05/2024 2:29:32 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OtherExpenses](
	[ExpenseID] [int] IDENTITY(1,1) NOT NULL,
	[Description] [nvarchar](255) NOT NULL,
	[ExpenseTitle] [nvarchar](50) NOT NULL,
	[ExpenseDate] [date] NULL,
	[ExpenseAmount] [decimal](8, 2) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ExpenseID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Quotation]    Script Date: 10/05/2024 2:29:32 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Quotation](
	[QuotationID] [int] NOT NULL,
	[ItemID] [int] NOT NULL,
	[ClientID] [int] NOT NULL,
	[UpdatedBy] [int] NULL,
	[UpdatedOn] [date] NULL,
	[ItemQuantity] [int] NOT NULL,
	[Discount] [decimal](3, 2) NULL,
	[TotalAmount] [decimal](8, 2) NULL,
	[PayableAmount] [decimal](8, 2) NULL,
PRIMARY KEY CLUSTERED 
(
	[QuotationID] ASC,
	[ItemID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Stock]    Script Date: 10/05/2024 2:29:32 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Stock](
	[StockId] [int] NOT NULL,
	[ItemId] [int] NOT NULL,
	[UpdatedBy] [int] NULL,
	[UpdatedOn] [date] NULL,
	[StockArrivalDate] [date] NOT NULL,
	[CostPrice] [decimal](8, 2) NOT NULL,
	[CurrentStockQuantity] [int] NOT NULL,
	[InitialQuantity] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[StockId] ASC,
	[ItemId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Vehicle]    Script Date: 10/05/2024 2:29:32 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Vehicle](
	[VehicleID] [int] IDENTITY(1,1) NOT NULL,
	[DriverID] [int] NOT NULL,
	[VehicleType] [int] NOT NULL,
	[PlateNumber] [nvarchar](8) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[VehicleID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [dbo].[Applicant] ([ApplicantID], [DesiredDesignation], [isSelected], [isShortlisted], [isRejected], [CVName], [PictureName]) VALUES (1, 5, 1, 1, 0, N'20240504111803185.pdf', N'20240504111801373.png')
INSERT [dbo].[Applicant] ([ApplicantID], [DesiredDesignation], [isSelected], [isShortlisted], [isRejected], [CVName], [PictureName]) VALUES (2, 5, 1, 1, 0, N'20240504140234797.pdf', N'20240504140233876.png')
INSERT [dbo].[Applicant] ([ApplicantID], [DesiredDesignation], [isSelected], [isShortlisted], [isRejected], [CVName], [PictureName]) VALUES (3, 3, 1, 1, 0, N'20240504225316852.pdf', N'20240504225316840.png')
INSERT [dbo].[Applicant] ([ApplicantID], [DesiredDesignation], [isSelected], [isShortlisted], [isRejected], [CVName], [PictureName]) VALUES (4, 3, 1, 1, 0, N'20240505021123266.pdf', N'20240505021123257.png')
INSERT [dbo].[Applicant] ([ApplicantID], [DesiredDesignation], [isSelected], [isShortlisted], [isRejected], [CVName], [PictureName]) VALUES (7, 3, 1, 1, 0, N'20240505024423833.pdf', N'20240505024423827.png')
INSERT [dbo].[Applicant] ([ApplicantID], [DesiredDesignation], [isSelected], [isShortlisted], [isRejected], [CVName], [PictureName]) VALUES (12, 3, 1, 1, 0, N'20240507032116306.pdf', N'20240507032116289.png')
INSERT [dbo].[Applicant] ([ApplicantID], [DesiredDesignation], [isSelected], [isShortlisted], [isRejected], [CVName], [PictureName]) VALUES (14, 3, 1, 1, 0, N'20240507032454599.pdf', N'20240507032454599.png')
INSERT [dbo].[Applicant] ([ApplicantID], [DesiredDesignation], [isSelected], [isShortlisted], [isRejected], [CVName], [PictureName]) VALUES (16, 5, 1, 1, 0, N'20240507034933159.pdf', N'20240507034933151.png')
INSERT [dbo].[Applicant] ([ApplicantID], [DesiredDesignation], [isSelected], [isShortlisted], [isRejected], [CVName], [PictureName]) VALUES (17, 5, 1, 1, 0, N'20240507113516717.pdf', N'20240507113516697.jpg')
INSERT [dbo].[Applicant] ([ApplicantID], [DesiredDesignation], [isSelected], [isShortlisted], [isRejected], [CVName], [PictureName]) VALUES (19, 6, 1, 1, 0, N'20240510013547645.pdf', N'20240510013547631.jpg')
GO
SET IDENTITY_INSERT [dbo].[Client] ON 

INSERT [dbo].[Client] ([ClientID], [ClientName], [City], [ClientEmail], [PhoneNumber], [UpdatedBy], [UpdatedOn]) VALUES (1, N'Nestle', N'Karachi', N'nestle@.org', NULL, NULL, NULL)
INSERT [dbo].[Client] ([ClientID], [ClientName], [City], [ClientEmail], [PhoneNumber], [UpdatedBy], [UpdatedOn]) VALUES (2, N'Pepsico', N'Sheikopura', N'pepsico@.org', NULL, NULL, NULL)
INSERT [dbo].[Client] ([ClientID], [ClientName], [City], [ClientEmail], [PhoneNumber], [UpdatedBy], [UpdatedOn]) VALUES (3, N'Dari Mouch', N'Lahore', N'darimouch@.org', NULL, NULL, NULL)
SET IDENTITY_INSERT [dbo].[Client] OFF
GO
INSERT [dbo].[Credentials] ([Username], [Password], [EmployeeID], [UpdatedBy], [UpdatedOn]) VALUES (N'afraz1', N'1212', 1, NULL, NULL)
INSERT [dbo].[Credentials] ([Username], [Password], [EmployeeID], [UpdatedBy], [UpdatedOn]) VALUES (N'Ahmad14', N'6651', 14, NULL, NULL)
INSERT [dbo].[Credentials] ([Username], [Password], [EmployeeID], [UpdatedBy], [UpdatedOn]) VALUES (N'Alishba17', N'2830', 17, NULL, NULL)
INSERT [dbo].[Credentials] ([Username], [Password], [EmployeeID], [UpdatedBy], [UpdatedOn]) VALUES (N'Fasi3', N'1214', 3, NULL, NULL)
GO
SET IDENTITY_INSERT [dbo].[Delivery] ON 

INSERT [dbo].[Delivery] ([DeliveryID], [VehicleID], [EmployeeID], [DriverID], [ClientID], [DeliveryStatus], [DeliveryDate], [UpdatedBy], [UpdatedOn], [Payment]) VALUES (4, 4, 1, 2, 1, 17, NULL, NULL, NULL, NULL)
SET IDENTITY_INSERT [dbo].[Delivery] OFF
GO
SET IDENTITY_INSERT [dbo].[Driver] ON 

INSERT [dbo].[Driver] ([DriverID], [DriverName], [DriverCNIC], [LicenseNumber], [UpdatedBy], [UpdatedOn], [LicenseExpiryDate]) VALUES (1, N'Asim', N'3520212345678', N'GK-1', NULL, NULL, CAST(N'2024-11-22' AS Date))
INSERT [dbo].[Driver] ([DriverID], [DriverName], [DriverCNIC], [LicenseNumber], [UpdatedBy], [UpdatedOn], [LicenseExpiryDate]) VALUES (2, N'Abid', N'3520212555678', N'GS-1', NULL, NULL, CAST(N'2025-01-23' AS Date))
INSERT [dbo].[Driver] ([DriverID], [DriverName], [DriverCNIC], [LicenseNumber], [UpdatedBy], [UpdatedOn], [LicenseExpiryDate]) VALUES (3, N'Fasi', N'3520297872065', N'KL-9001', NULL, NULL, CAST(N'2026-01-01' AS Date))
INSERT [dbo].[Driver] ([DriverID], [DriverName], [DriverCNIC], [LicenseNumber], [UpdatedBy], [UpdatedOn], [LicenseExpiryDate]) VALUES (4, N'Wali Chaprasi', N'3520291212189', N'KL-9022', NULL, NULL, CAST(N'2029-12-12' AS Date))
SET IDENTITY_INSERT [dbo].[Driver] OFF
GO
INSERT [dbo].[EmergencyContact] ([EmergencyContactID], [EmployeeID], [Relationship]) VALUES (3, 4, 7)
INSERT [dbo].[EmergencyContact] ([EmergencyContactID], [EmployeeID], [Relationship]) VALUES (1, 12, 7)
INSERT [dbo].[EmergencyContact] ([EmergencyContactID], [EmployeeID], [Relationship]) VALUES (3, 14, 10)
INSERT [dbo].[EmergencyContact] ([EmergencyContactID], [EmployeeID], [Relationship]) VALUES (3, 16, 7)
INSERT [dbo].[EmergencyContact] ([EmergencyContactID], [EmployeeID], [Relationship]) VALUES (18, 17, 7)
INSERT [dbo].[EmergencyContact] ([EmergencyContactID], [EmployeeID], [Relationship]) VALUES (3, 19, 7)
GO
INSERT [dbo].[Employee] ([EmployeeID], [DesignationID], [UpdatedBy], [UpdatedOn], [EmployeeNumber], [StartDate], [EndDate], [IsActive], [BaseSalary]) VALUES (1, 5, NULL, NULL, N'Worker-1', CAST(N'2024-05-04' AS Date), NULL, 1, CAST(100000.00 AS Decimal(8, 2)))
INSERT [dbo].[Employee] ([EmployeeID], [DesignationID], [UpdatedBy], [UpdatedOn], [EmployeeNumber], [StartDate], [EndDate], [IsActive], [BaseSalary]) VALUES (2, 5, NULL, NULL, N'HR-2', CAST(N'2024-05-06' AS Date), NULL, 1, CAST(1000.00 AS Decimal(8, 2)))
INSERT [dbo].[Employee] ([EmployeeID], [DesignationID], [UpdatedBy], [UpdatedOn], [EmployeeNumber], [StartDate], [EndDate], [IsActive], [BaseSalary]) VALUES (3, 3, NULL, NULL, N'Worker-3', CAST(N'2024-05-06' AS Date), NULL, 1, CAST(10000.00 AS Decimal(8, 2)))
INSERT [dbo].[Employee] ([EmployeeID], [DesignationID], [UpdatedBy], [UpdatedOn], [EmployeeNumber], [StartDate], [EndDate], [IsActive], [BaseSalary]) VALUES (4, 3, NULL, NULL, N'Worker-4', CAST(N'2024-05-06' AS Date), NULL, 0, CAST(100000.00 AS Decimal(8, 2)))
INSERT [dbo].[Employee] ([EmployeeID], [DesignationID], [UpdatedBy], [UpdatedOn], [EmployeeNumber], [StartDate], [EndDate], [IsActive], [BaseSalary]) VALUES (7, 3, NULL, NULL, N'Worker-7', CAST(N'2024-05-06' AS Date), NULL, 0, CAST(100000.00 AS Decimal(8, 2)))
INSERT [dbo].[Employee] ([EmployeeID], [DesignationID], [UpdatedBy], [UpdatedOn], [EmployeeNumber], [StartDate], [EndDate], [IsActive], [BaseSalary]) VALUES (12, 3, NULL, NULL, N'Worker-12', CAST(N'2024-05-07' AS Date), NULL, 0, CAST(10000.00 AS Decimal(8, 2)))
INSERT [dbo].[Employee] ([EmployeeID], [DesignationID], [UpdatedBy], [UpdatedOn], [EmployeeNumber], [StartDate], [EndDate], [IsActive], [BaseSalary]) VALUES (14, 3, NULL, NULL, N'Worker-14', CAST(N'2024-05-07' AS Date), NULL, 1, CAST(100000.00 AS Decimal(8, 2)))
INSERT [dbo].[Employee] ([EmployeeID], [DesignationID], [UpdatedBy], [UpdatedOn], [EmployeeNumber], [StartDate], [EndDate], [IsActive], [BaseSalary]) VALUES (16, 5, NULL, NULL, N'HR-16', CAST(N'2024-05-07' AS Date), NULL, 0, CAST(100000.00 AS Decimal(8, 2)))
INSERT [dbo].[Employee] ([EmployeeID], [DesignationID], [UpdatedBy], [UpdatedOn], [EmployeeNumber], [StartDate], [EndDate], [IsActive], [BaseSalary]) VALUES (17, 5, NULL, NULL, N'HR-17', CAST(N'2024-05-07' AS Date), NULL, 1, CAST(100000.00 AS Decimal(8, 2)))
INSERT [dbo].[Employee] ([EmployeeID], [DesignationID], [UpdatedBy], [UpdatedOn], [EmployeeNumber], [StartDate], [EndDate], [IsActive], [BaseSalary]) VALUES (19, 6, NULL, NULL, N'Inventory Manager-19', CAST(N'2024-05-10' AS Date), NULL, 1, CAST(100.00 AS Decimal(8, 2)))
GO
SET IDENTITY_INSERT [dbo].[Item] ON 

INSERT [dbo].[Item] ([ItemID], [ItemName], [AvailableQuantity], [Description], [MeasurementUnit], [SalePrice], [UpdatedBy], [UpdatedOn]) VALUES (1, N'Sariya', 3200, N'Iron Rods', N'Feets', CAST(100.00 AS Decimal(8, 2)), NULL, NULL)
INSERT [dbo].[Item] ([ItemID], [ItemName], [AvailableQuantity], [Description], [MeasurementUnit], [SalePrice], [UpdatedBy], [UpdatedOn]) VALUES (2, N'cement', 2100, NULL, N'Kg', CAST(100.00 AS Decimal(8, 2)), NULL, NULL)
INSERT [dbo].[Item] ([ItemID], [ItemName], [AvailableQuantity], [Description], [MeasurementUnit], [SalePrice], [UpdatedBy], [UpdatedOn]) VALUES (3, N'Plastic Pipe', 3000, NULL, N'Feets', CAST(100.00 AS Decimal(8, 2)), NULL, NULL)
INSERT [dbo].[Item] ([ItemID], [ItemName], [AvailableQuantity], [Description], [MeasurementUnit], [SalePrice], [UpdatedBy], [UpdatedOn]) VALUES (4, N'Bricks', 1000, NULL, N'kg', CAST(100.00 AS Decimal(8, 2)), NULL, NULL)
SET IDENTITY_INSERT [dbo].[Item] OFF
GO
SET IDENTITY_INSERT [dbo].[Lookup] ON 

INSERT [dbo].[Lookup] ([lookupID], [Value], [Description]) VALUES (1, N'Male', N'Gender')
INSERT [dbo].[Lookup] ([lookupID], [Value], [Description]) VALUES (2, N'Female', N'Gender')
INSERT [dbo].[Lookup] ([lookupID], [Value], [Description]) VALUES (3, N'Worker', N'Designation')
INSERT [dbo].[Lookup] ([lookupID], [Value], [Description]) VALUES (4, N'Accountant', N'Designation')
INSERT [dbo].[Lookup] ([lookupID], [Value], [Description]) VALUES (5, N'HR', N'Designation')
INSERT [dbo].[Lookup] ([lookupID], [Value], [Description]) VALUES (6, N'InventoryManager', N'Designation')
INSERT [dbo].[Lookup] ([lookupID], [Value], [Description]) VALUES (7, N'Sibling', N'Relationship')
INSERT [dbo].[Lookup] ([lookupID], [Value], [Description]) VALUES (8, N'Spouse', N'Relationship')
INSERT [dbo].[Lookup] ([lookupID], [Value], [Description]) VALUES (9, N'Child', N'Relationship')
INSERT [dbo].[Lookup] ([lookupID], [Value], [Description]) VALUES (10, N'Parent', N'Relationship')
INSERT [dbo].[Lookup] ([lookupID], [Value], [Description]) VALUES (11, N'Other', N'Relationship')
INSERT [dbo].[Lookup] ([lookupID], [Value], [Description]) VALUES (12, N'Truck', N'VehicleType')
INSERT [dbo].[Lookup] ([lookupID], [Value], [Description]) VALUES (13, N'Van', N'VehicleType')
INSERT [dbo].[Lookup] ([lookupID], [Value], [Description]) VALUES (14, N'Rickshaw', N'VehicleType')
INSERT [dbo].[Lookup] ([lookupID], [Value], [Description]) VALUES (15, N'PickUp', N'VehicleType')
INSERT [dbo].[Lookup] ([lookupID], [Value], [Description]) VALUES (16, N'Car', N'VehicleType')
INSERT [dbo].[Lookup] ([lookupID], [Value], [Description]) VALUES (17, N'Dispatched', N'Delivery Status')
INSERT [dbo].[Lookup] ([lookupID], [Value], [Description]) VALUES (18, N'Delivered', N'Delivery Status')
INSERT [dbo].[Lookup] ([lookupID], [Value], [Description]) VALUES (19, N'Canceled', N'Delivery Status')
SET IDENTITY_INSERT [dbo].[Lookup] OFF
GO
SET IDENTITY_INSERT [dbo].[Order] ON 

INSERT [dbo].[Order] ([OrderID], [QuotationID], [ItemID], [ClientID], [UpdatedBy], [UpdatedOn], [RequiredDate], [OrderDate], [IsServed]) VALUES (1, 3, 1, 1, NULL, NULL, CAST(N'2024-05-09' AS Date), CAST(N'2024-05-09' AS Date), 0)
INSERT [dbo].[Order] ([OrderID], [QuotationID], [ItemID], [ClientID], [UpdatedBy], [UpdatedOn], [RequiredDate], [OrderDate], [IsServed]) VALUES (2, 3, 1, 1, NULL, NULL, CAST(N'2024-05-09' AS Date), CAST(N'2024-05-09' AS Date), 0)
INSERT [dbo].[Order] ([OrderID], [QuotationID], [ItemID], [ClientID], [UpdatedBy], [UpdatedOn], [RequiredDate], [OrderDate], [IsServed]) VALUES (3, 3, 1, 1, NULL, NULL, CAST(N'2024-05-09' AS Date), CAST(N'2024-05-09' AS Date), 0)
INSERT [dbo].[Order] ([OrderID], [QuotationID], [ItemID], [ClientID], [UpdatedBy], [UpdatedOn], [RequiredDate], [OrderDate], [IsServed]) VALUES (4, 3, 1, 1, NULL, NULL, CAST(N'2024-05-09' AS Date), CAST(N'2024-05-09' AS Date), 0)
SET IDENTITY_INSERT [dbo].[Order] OFF
GO
SET IDENTITY_INSERT [dbo].[Person] ON 

INSERT [dbo].[Person] ([PersonID], [Gender], [CNIC], [FirstName], [LastName], [Email], [PrimaryPhone], [AlternatePhone], [DateOfBirth], [Address]) VALUES (1, 1, N'1234567891214', N'Afraz', N'Tahir', N'afraz.tahir3145@gmail.com', N'03214208015', N'03214562046', CAST(N'1999-09-01T00:00:00.000' AS DateTime), N'House # 15, Street # 42 Mohallah Muhammadi Wassan Pura, Lahore')
INSERT [dbo].[Person] ([PersonID], [Gender], [CNIC], [FirstName], [LastName], [Email], [PrimaryPhone], [AlternatePhone], [DateOfBirth], [Address]) VALUES (2, 1, N'3520184390647', N'Wali', N'Allah', N'waliallah019@gmail.com', N'03366470094', N'03214313029', CAST(N'2004-12-19T00:00:00.000' AS DateTime), N'Kothi Mian Bashir Ahmad, Toheed park, Darogawala, Lahore, Pakistan')
INSERT [dbo].[Person] ([PersonID], [Gender], [CNIC], [FirstName], [LastName], [Email], [PrimaryPhone], [AlternatePhone], [DateOfBirth], [Address]) VALUES (3, 1, N'3520297872065', N'Fasi', N'Tahir', N'fasitahir2019@gmail.com', N'03030904069', N'03224273548', CAST(N'2003-05-24T00:00:00.000' AS DateTime), N'Street#42 Muhammdi Muhala Wasan Pura Lahore')
INSERT [dbo].[Person] ([PersonID], [Gender], [CNIC], [FirstName], [LastName], [Email], [PrimaryPhone], [AlternatePhone], [DateOfBirth], [Address]) VALUES (4, 1, N'1221324354365', N'Abdul Manan', N'Tahir', N'ehunt2080@gmail.com', N'03224273548', NULL, CAST(N'1995-10-31T00:00:00.000' AS DateTime), N'House # 15, Street # 42 Mohallah Muhammadi Wassan Pura, Lahore')
INSERT [dbo].[Person] ([PersonID], [Gender], [CNIC], [FirstName], [LastName], [Email], [PrimaryPhone], [AlternatePhone], [DateOfBirth], [Address]) VALUES (7, 1, N'1234567891234', N'Ali', N'Ahmad', N'someoneElse@gmail.com', N'12345678901', NULL, CAST(N'1990-12-12T00:00:00.000' AS DateTime), N'House # 16, Street # 47 Mohallah Muhammadi Wassan Pura, Lahore')
INSERT [dbo].[Person] ([PersonID], [Gender], [CNIC], [FirstName], [LastName], [Email], [PrimaryPhone], [AlternatePhone], [DateOfBirth], [Address]) VALUES (12, 1, N'3520184390147', N'Aslam', N'Khan', N'aslamkhan@gmail.com', N'03214208015', NULL, CAST(N'1990-12-12T00:00:00.000' AS DateTime), N'House # 15, Street # 43 Mohallah Muhammadi Wassan Pura, Lahore, Pakistan')
INSERT [dbo].[Person] ([PersonID], [Gender], [CNIC], [FirstName], [LastName], [Email], [PrimaryPhone], [AlternatePhone], [DateOfBirth], [Address]) VALUES (14, 1, N'1234567892234', N'Ahmad', NULL, N'Ahmad12@gmail.com', N'03214208012', NULL, CAST(N'1998-12-12T00:00:00.000' AS DateTime), N'Ahmad nagar Lahore, Pakistan')
INSERT [dbo].[Person] ([PersonID], [Gender], [CNIC], [FirstName], [LastName], [Email], [PrimaryPhone], [AlternatePhone], [DateOfBirth], [Address]) VALUES (16, 2, N'3456789111111', N'Fatima', NULL, N'fatima@gmail.com', N'09007860199', NULL, CAST(N'2002-12-12T00:00:00.000' AS DateTime), N'House #12 faisal town block 5 Lahore')
INSERT [dbo].[Person] ([PersonID], [Gender], [CNIC], [FirstName], [LastName], [Email], [PrimaryPhone], [AlternatePhone], [DateOfBirth], [Address]) VALUES (17, 2, N'4230144384470', N'Alishba', N'Sheikh', N'sheikhalishba878@gmail.com', N'03010966661', N'03010966661', CAST(N'2003-06-17T00:00:00.000' AS DateTime), N'Ghulam Muhammad din Sui gas road (sheikhupura)road ,multan')
INSERT [dbo].[Person] ([PersonID], [Gender], [CNIC], [FirstName], [LastName], [Email], [PrimaryPhone], [AlternatePhone], [DateOfBirth], [Address]) VALUES (18, 2, N'4230109666611', N'rimsha', N'sheikh', N'alishba64@gmail.com', N'03134536642', NULL, CAST(N'2003-06-17T00:00:00.000' AS DateTime), N'House # 15, Street # 42 Mohallah Muhammadi Wassan Pura, Lahore')
INSERT [dbo].[Person] ([PersonID], [Gender], [CNIC], [FirstName], [LastName], [Email], [PrimaryPhone], [AlternatePhone], [DateOfBirth], [Address]) VALUES (19, 2, N'1721892919281', N'Irha', N'Ahmad', N'irhaAhmad21@gmail.com', N'03366470094', NULL, CAST(N'1999-12-12T00:00:00.000' AS DateTime), N'Street#42 Muhammdi Muhala Wasan Pura Lahore')
SET IDENTITY_INSERT [dbo].[Person] OFF
GO
INSERT [dbo].[Quotation] ([QuotationID], [ItemID], [ClientID], [UpdatedBy], [UpdatedOn], [ItemQuantity], [Discount], [TotalAmount], [PayableAmount]) VALUES (3, 1, 1, NULL, NULL, 20, CAST(0.00 AS Decimal(3, 2)), CAST(10000.00 AS Decimal(8, 2)), CAST(10000.00 AS Decimal(8, 2)))
INSERT [dbo].[Quotation] ([QuotationID], [ItemID], [ClientID], [UpdatedBy], [UpdatedOn], [ItemQuantity], [Discount], [TotalAmount], [PayableAmount]) VALUES (4, 2, 2, NULL, NULL, 50, CAST(0.00 AS Decimal(3, 2)), CAST(12500.00 AS Decimal(8, 2)), CAST(12500.00 AS Decimal(8, 2)))
GO
INSERT [dbo].[Salary] ([EmployeeID], [PaidDate], [IsPaid], [PaidAmount], [Salary], [ExpectedDate], [UpdatedBy], [UpdatedOn]) VALUES (14, CAST(N'2023-04-01' AS Date), 1, CAST(10000.00 AS Decimal(8, 2)), CAST(10000.00 AS Decimal(8, 2)), CAST(N'2023-04-01' AS Date), NULL, NULL)
INSERT [dbo].[Salary] ([EmployeeID], [PaidDate], [IsPaid], [PaidAmount], [Salary], [ExpectedDate], [UpdatedBy], [UpdatedOn]) VALUES (17, CAST(N'2001-04-23' AS Date), 1, CAST(10000.00 AS Decimal(8, 2)), CAST(10000.00 AS Decimal(8, 2)), CAST(N'2023-04-01' AS Date), NULL, NULL)
GO
INSERT [dbo].[Stock] ([StockId], [ItemId], [UpdatedBy], [UpdatedOn], [StockArrivalDate], [CostPrice], [CurrentStockQuantity], [InitialQuantity]) VALUES (1, 1, NULL, NULL, CAST(N'2023-12-12' AS Date), CAST(90.00 AS Decimal(8, 2)), 1000, 1000)
INSERT [dbo].[Stock] ([StockId], [ItemId], [UpdatedBy], [UpdatedOn], [StockArrivalDate], [CostPrice], [CurrentStockQuantity], [InitialQuantity]) VALUES (2, 1, NULL, NULL, CAST(N'2023-12-12' AS Date), CAST(900.00 AS Decimal(8, 2)), 1000, 1000)
INSERT [dbo].[Stock] ([StockId], [ItemId], [UpdatedBy], [UpdatedOn], [StockArrivalDate], [CostPrice], [CurrentStockQuantity], [InitialQuantity]) VALUES (3, 2, NULL, NULL, CAST(N'2023-12-12' AS Date), CAST(900.00 AS Decimal(8, 2)), 1000, 1000)
INSERT [dbo].[Stock] ([StockId], [ItemId], [UpdatedBy], [UpdatedOn], [StockArrivalDate], [CostPrice], [CurrentStockQuantity], [InitialQuantity]) VALUES (4, 1, NULL, NULL, CAST(N'2023-11-12' AS Date), CAST(1000.00 AS Decimal(8, 2)), 100, 100)
INSERT [dbo].[Stock] ([StockId], [ItemId], [UpdatedBy], [UpdatedOn], [StockArrivalDate], [CostPrice], [CurrentStockQuantity], [InitialQuantity]) VALUES (5, 3, NULL, NULL, CAST(N'2022-12-12' AS Date), CAST(10.00 AS Decimal(8, 2)), 1000, 1000)
INSERT [dbo].[Stock] ([StockId], [ItemId], [UpdatedBy], [UpdatedOn], [StockArrivalDate], [CostPrice], [CurrentStockQuantity], [InitialQuantity]) VALUES (6, 3, NULL, NULL, CAST(N'2023-12-12' AS Date), CAST(90.00 AS Decimal(8, 2)), 1000, 1000)
INSERT [dbo].[Stock] ([StockId], [ItemId], [UpdatedBy], [UpdatedOn], [StockArrivalDate], [CostPrice], [CurrentStockQuantity], [InitialQuantity]) VALUES (7, 1, NULL, NULL, CAST(N'2024-11-12' AS Date), CAST(100.00 AS Decimal(8, 2)), 100, 100)
INSERT [dbo].[Stock] ([StockId], [ItemId], [UpdatedBy], [UpdatedOn], [StockArrivalDate], [CostPrice], [CurrentStockQuantity], [InitialQuantity]) VALUES (8, 1, NULL, NULL, CAST(N'2024-12-12' AS Date), CAST(100.00 AS Decimal(8, 2)), 1000, 1000)
INSERT [dbo].[Stock] ([StockId], [ItemId], [UpdatedBy], [UpdatedOn], [StockArrivalDate], [CostPrice], [CurrentStockQuantity], [InitialQuantity]) VALUES (8, 2, NULL, NULL, CAST(N'2024-12-12' AS Date), CAST(100.00 AS Decimal(8, 2)), 1000, 1000)
INSERT [dbo].[Stock] ([StockId], [ItemId], [UpdatedBy], [UpdatedOn], [StockArrivalDate], [CostPrice], [CurrentStockQuantity], [InitialQuantity]) VALUES (8, 3, NULL, NULL, CAST(N'2024-12-12' AS Date), CAST(100.00 AS Decimal(8, 2)), 1000, 1000)
INSERT [dbo].[Stock] ([StockId], [ItemId], [UpdatedBy], [UpdatedOn], [StockArrivalDate], [CostPrice], [CurrentStockQuantity], [InitialQuantity]) VALUES (8, 4, NULL, NULL, CAST(N'2019-12-12' AS Date), CAST(100.00 AS Decimal(8, 2)), 1000, 1000)
GO
SET IDENTITY_INSERT [dbo].[Vehicle] ON 

INSERT [dbo].[Vehicle] ([VehicleID], [DriverID], [VehicleType], [PlateNumber]) VALUES (1, 1, 12, N'LEX-1122')
INSERT [dbo].[Vehicle] ([VehicleID], [DriverID], [VehicleType], [PlateNumber]) VALUES (3, 1, 12, N'LEX-1122')
INSERT [dbo].[Vehicle] ([VehicleID], [DriverID], [VehicleType], [PlateNumber]) VALUES (4, 2, 16, N'LEQ-1323')
SET IDENTITY_INSERT [dbo].[Vehicle] OFF
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UC_ClientEmail]    Script Date: 10/05/2024 2:29:32 pm ******/
ALTER TABLE [dbo].[Client] ADD  CONSTRAINT [UC_ClientEmail] UNIQUE NONCLUSTERED 
(
	[ClientEmail] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ_DriverCNIC]    Script Date: 10/05/2024 2:29:32 pm ******/
ALTER TABLE [dbo].[Driver] ADD  CONSTRAINT [UQ_DriverCNIC] UNIQUE NONCLUSTERED 
(
	[DriverCNIC] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ_LicenseNumber]    Script Date: 10/05/2024 2:29:32 pm ******/
ALTER TABLE [dbo].[Driver] ADD  CONSTRAINT [UQ_LicenseNumber] UNIQUE NONCLUSTERED 
(
	[LicenseNumber] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UC_EmployeeNumber]    Script Date: 10/05/2024 2:29:32 pm ******/
ALTER TABLE [dbo].[Employee] ADD  CONSTRAINT [UC_EmployeeNumber] UNIQUE NONCLUSTERED 
(
	[EmployeeNumber] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UC_ItemName]    Script Date: 10/05/2024 2:29:32 pm ******/
ALTER TABLE [dbo].[Item] ADD  CONSTRAINT [UC_ItemName] UNIQUE NONCLUSTERED 
(
	[ItemName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UC_CNIC]    Script Date: 10/05/2024 2:29:32 pm ******/
ALTER TABLE [dbo].[Person] ADD  CONSTRAINT [UC_CNIC] UNIQUE NONCLUSTERED 
(
	[CNIC] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UC_Email]    Script Date: 10/05/2024 2:29:32 pm ******/
ALTER TABLE [dbo].[Person] ADD  CONSTRAINT [UC_Email] UNIQUE NONCLUSTERED 
(
	[Email] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Applicant]  WITH CHECK ADD FOREIGN KEY([ApplicantID])
REFERENCES [dbo].[Person] ([PersonID])
GO
ALTER TABLE [dbo].[Applicant]  WITH CHECK ADD FOREIGN KEY([DesiredDesignation])
REFERENCES [dbo].[Lookup] ([lookupID])
GO
ALTER TABLE [dbo].[Client]  WITH CHECK ADD FOREIGN KEY([UpdatedBy])
REFERENCES [dbo].[Employee] ([EmployeeID])
GO
ALTER TABLE [dbo].[Credentials]  WITH CHECK ADD FOREIGN KEY([EmployeeID])
REFERENCES [dbo].[Employee] ([EmployeeID])
GO
ALTER TABLE [dbo].[Credentials]  WITH CHECK ADD FOREIGN KEY([UpdatedBy])
REFERENCES [dbo].[Employee] ([EmployeeID])
GO
ALTER TABLE [dbo].[Delivery]  WITH CHECK ADD FOREIGN KEY([ClientID])
REFERENCES [dbo].[Client] ([ClientID])
GO
ALTER TABLE [dbo].[Delivery]  WITH CHECK ADD FOREIGN KEY([DeliveryStatus])
REFERENCES [dbo].[Lookup] ([lookupID])
GO
ALTER TABLE [dbo].[Delivery]  WITH CHECK ADD FOREIGN KEY([DriverID])
REFERENCES [dbo].[Driver] ([DriverID])
GO
ALTER TABLE [dbo].[Delivery]  WITH CHECK ADD FOREIGN KEY([EmployeeID])
REFERENCES [dbo].[Employee] ([EmployeeID])
GO
ALTER TABLE [dbo].[Delivery]  WITH CHECK ADD FOREIGN KEY([UpdatedBy])
REFERENCES [dbo].[Employee] ([EmployeeID])
GO
ALTER TABLE [dbo].[Delivery]  WITH CHECK ADD FOREIGN KEY([VehicleID])
REFERENCES [dbo].[Vehicle] ([VehicleID])
GO
ALTER TABLE [dbo].[Driver]  WITH CHECK ADD FOREIGN KEY([UpdatedBy])
REFERENCES [dbo].[Employee] ([EmployeeID])
GO
ALTER TABLE [dbo].[EmergencyContact]  WITH CHECK ADD FOREIGN KEY([EmergencyContactID])
REFERENCES [dbo].[Person] ([PersonID])
GO
ALTER TABLE [dbo].[EmergencyContact]  WITH CHECK ADD FOREIGN KEY([EmployeeID])
REFERENCES [dbo].[Employee] ([EmployeeID])
GO
ALTER TABLE [dbo].[EmergencyContact]  WITH CHECK ADD FOREIGN KEY([Relationship])
REFERENCES [dbo].[Lookup] ([lookupID])
GO
ALTER TABLE [dbo].[Employee]  WITH CHECK ADD FOREIGN KEY([DesignationID])
REFERENCES [dbo].[Lookup] ([lookupID])
GO
ALTER TABLE [dbo].[Employee]  WITH CHECK ADD FOREIGN KEY([EmployeeID])
REFERENCES [dbo].[Applicant] ([ApplicantID])
GO
ALTER TABLE [dbo].[Employee]  WITH CHECK ADD FOREIGN KEY([UpdatedBy])
REFERENCES [dbo].[Employee] ([EmployeeID])
GO
ALTER TABLE [dbo].[Item]  WITH CHECK ADD FOREIGN KEY([UpdatedBy])
REFERENCES [dbo].[Employee] ([EmployeeID])
GO
ALTER TABLE [dbo].[Order]  WITH CHECK ADD FOREIGN KEY([QuotationID], [ItemID])
REFERENCES [dbo].[Quotation] ([QuotationID], [ItemID])
GO
ALTER TABLE [dbo].[Order]  WITH CHECK ADD FOREIGN KEY([ClientID])
REFERENCES [dbo].[Client] ([ClientID])
GO
ALTER TABLE [dbo].[Order]  WITH CHECK ADD FOREIGN KEY([UpdatedBy])
REFERENCES [dbo].[Employee] ([EmployeeID])
GO
ALTER TABLE [dbo].[Person]  WITH CHECK ADD FOREIGN KEY([Gender])
REFERENCES [dbo].[Lookup] ([lookupID])
GO
ALTER TABLE [dbo].[Quotation]  WITH CHECK ADD FOREIGN KEY([ClientID])
REFERENCES [dbo].[Client] ([ClientID])
GO
ALTER TABLE [dbo].[Quotation]  WITH CHECK ADD FOREIGN KEY([ItemID])
REFERENCES [dbo].[Item] ([ItemID])
GO
ALTER TABLE [dbo].[Quotation]  WITH CHECK ADD FOREIGN KEY([UpdatedBy])
REFERENCES [dbo].[Employee] ([EmployeeID])
GO
ALTER TABLE [dbo].[Salary]  WITH CHECK ADD FOREIGN KEY([EmployeeID])
REFERENCES [dbo].[Employee] ([EmployeeID])
GO
ALTER TABLE [dbo].[Salary]  WITH CHECK ADD FOREIGN KEY([UpdatedBy])
REFERENCES [dbo].[Employee] ([EmployeeID])
GO
ALTER TABLE [dbo].[Stock]  WITH CHECK ADD FOREIGN KEY([ItemId])
REFERENCES [dbo].[Item] ([ItemID])
GO
ALTER TABLE [dbo].[Stock]  WITH CHECK ADD FOREIGN KEY([UpdatedBy])
REFERENCES [dbo].[Employee] ([EmployeeID])
GO
ALTER TABLE [dbo].[Vehicle]  WITH CHECK ADD FOREIGN KEY([DriverID])
REFERENCES [dbo].[Driver] ([DriverID])
GO
ALTER TABLE [dbo].[Vehicle]  WITH CHECK ADD FOREIGN KEY([VehicleType])
REFERENCES [dbo].[Lookup] ([lookupID])
GO
ALTER TABLE [dbo].[Applicant]  WITH CHECK ADD  CONSTRAINT [CHK_DesiredDesignation] CHECK  (([DesiredDesignation]=(6) OR [DesiredDesignation]=(5) OR [DesiredDesignation]=(4) OR [DesiredDesignation]=(3)))
GO
ALTER TABLE [dbo].[Applicant] CHECK CONSTRAINT [CHK_DesiredDesignation]
GO
ALTER TABLE [dbo].[Applicant]  WITH CHECK ADD  CONSTRAINT [CHK_isSelected] CHECK  (([isSelected]=(1) OR [isSelected]=(0)))
GO
ALTER TABLE [dbo].[Applicant] CHECK CONSTRAINT [CHK_isSelected]
GO
ALTER TABLE [dbo].[Applicant]  WITH CHECK ADD  CONSTRAINT [CHK_isShortlisted] CHECK  (([isShortlisted]=(1) OR [isShortlisted]=(0)))
GO
ALTER TABLE [dbo].[Applicant] CHECK CONSTRAINT [CHK_isShortlisted]
GO
ALTER TABLE [dbo].[Client]  WITH CHECK ADD  CONSTRAINT [CHK_City_Length] CHECK  ((len([City])<=(50)))
GO
ALTER TABLE [dbo].[Client] CHECK CONSTRAINT [CHK_City_Length]
GO
ALTER TABLE [dbo].[Client]  WITH CHECK ADD  CONSTRAINT [CHK_ClientEmail_Format] CHECK  (([ClientEmail] like '%@%.%' AND len([ClientEmail])<=(255)))
GO
ALTER TABLE [dbo].[Client] CHECK CONSTRAINT [CHK_ClientEmail_Format]
GO
ALTER TABLE [dbo].[Client]  WITH CHECK ADD  CONSTRAINT [CHK_PhoneNumber_Format] CHECK  (([PhoneNumber] like '[0-9]%'))
GO
ALTER TABLE [dbo].[Client] CHECK CONSTRAINT [CHK_PhoneNumber_Format]
GO
ALTER TABLE [dbo].[Delivery]  WITH CHECK ADD  CONSTRAINT [CHK_Payment] CHECK  (([Payment]>=(0)))
GO
ALTER TABLE [dbo].[Delivery] CHECK CONSTRAINT [CHK_Payment]
GO
ALTER TABLE [dbo].[Driver]  WITH CHECK ADD  CONSTRAINT [CHK_LicenseExpiryDate] CHECK  (([LicenseExpiryDate]>=getdate()))
GO
ALTER TABLE [dbo].[Driver] CHECK CONSTRAINT [CHK_LicenseExpiryDate]
GO
ALTER TABLE [dbo].[Driver]  WITH CHECK ADD  CONSTRAINT [DriverCNIC_check] CHECK  (([DriverCNIC] like '[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]'))
GO
ALTER TABLE [dbo].[Driver] CHECK CONSTRAINT [DriverCNIC_check]
GO
ALTER TABLE [dbo].[EmergencyContact]  WITH CHECK ADD  CONSTRAINT [CHK_Relationship] CHECK  (([Relationship]=(11) OR [Relationship]=(10) OR [Relationship]=(9) OR [Relationship]=(8) OR [Relationship]=(7)))
GO
ALTER TABLE [dbo].[EmergencyContact] CHECK CONSTRAINT [CHK_Relationship]
GO
ALTER TABLE [dbo].[Employee]  WITH CHECK ADD  CONSTRAINT [CHK_BaseSalary] CHECK  (([BaseSalary]>=(0)))
GO
ALTER TABLE [dbo].[Employee] CHECK CONSTRAINT [CHK_BaseSalary]
GO
ALTER TABLE [dbo].[Employee]  WITH CHECK ADD  CONSTRAINT [CHK_EndDate] CHECK  (([EndDate] IS NULL OR [EndDate]>=[StartDate]))
GO
ALTER TABLE [dbo].[Employee] CHECK CONSTRAINT [CHK_EndDate]
GO
ALTER TABLE [dbo].[Employee]  WITH CHECK ADD  CONSTRAINT [CHK_IsActive] CHECK  (([IsActive]=(1) OR [IsActive]=(0)))
GO
ALTER TABLE [dbo].[Employee] CHECK CONSTRAINT [CHK_IsActive]
GO
ALTER TABLE [dbo].[Employee]  WITH CHECK ADD  CONSTRAINT [CHK_StartDate] CHECK  (([StartDate]<=getdate()))
GO
ALTER TABLE [dbo].[Employee] CHECK CONSTRAINT [CHK_StartDate]
GO
ALTER TABLE [dbo].[Item]  WITH CHECK ADD  CONSTRAINT [CHK_AvailableQuantity] CHECK  (([AvailableQuantity]>=(0)))
GO
ALTER TABLE [dbo].[Item] CHECK CONSTRAINT [CHK_AvailableQuantity]
GO
ALTER TABLE [dbo].[Item]  WITH CHECK ADD  CONSTRAINT [CHK_Description_Length] CHECK  ((len([Description])<=(300)))
GO
ALTER TABLE [dbo].[Item] CHECK CONSTRAINT [CHK_Description_Length]
GO
ALTER TABLE [dbo].[Item]  WITH CHECK ADD  CONSTRAINT [CHK_ItemName_Length] CHECK  ((len([ItemName])<=(100)))
GO
ALTER TABLE [dbo].[Item] CHECK CONSTRAINT [CHK_ItemName_Length]
GO
ALTER TABLE [dbo].[Item]  WITH CHECK ADD  CONSTRAINT [CHK_MeasurementUnit_Length] CHECK  ((len([MeasurementUnit])<=(50)))
GO
ALTER TABLE [dbo].[Item] CHECK CONSTRAINT [CHK_MeasurementUnit_Length]
GO
ALTER TABLE [dbo].[Item]  WITH CHECK ADD  CONSTRAINT [CHK_SalePrice] CHECK  (([SalePrice]>=(0)))
GO
ALTER TABLE [dbo].[Item] CHECK CONSTRAINT [CHK_SalePrice]
GO
ALTER TABLE [dbo].[Order]  WITH CHECK ADD  CONSTRAINT [CHK_OrderDate] CHECK  (([OrderDate]<=getdate()))
GO
ALTER TABLE [dbo].[Order] CHECK CONSTRAINT [CHK_OrderDate]
GO
ALTER TABLE [dbo].[Order]  WITH CHECK ADD  CONSTRAINT [CHK_RequiredDate] CHECK  (([RequiredDate]>=[OrderDate]))
GO
ALTER TABLE [dbo].[Order] CHECK CONSTRAINT [CHK_RequiredDate]
GO
ALTER TABLE [dbo].[OtherExpenses]  WITH CHECK ADD  CONSTRAINT [CHK_ExpenseDate] CHECK  (([ExpenseDate]<=getdate()))
GO
ALTER TABLE [dbo].[OtherExpenses] CHECK CONSTRAINT [CHK_ExpenseDate]
GO
ALTER TABLE [dbo].[Person]  WITH CHECK ADD  CONSTRAINT [CHK_DOB] CHECK  (([DateOfBirth]<=getdate() AND [DateOfBirth]>=dateadd(year,(-80),getdate())))
GO
ALTER TABLE [dbo].[Person] CHECK CONSTRAINT [CHK_DOB]
GO
ALTER TABLE [dbo].[Person]  WITH CHECK ADD  CONSTRAINT [CHK_Email_Format] CHECK  (([Email] like '%@%.%' AND len([Email])<=(255)))
GO
ALTER TABLE [dbo].[Person] CHECK CONSTRAINT [CHK_Email_Format]
GO
ALTER TABLE [dbo].[Person]  WITH CHECK ADD  CONSTRAINT [CHK_FirstName] CHECK  ((len([FirstName])<=(50)))
GO
ALTER TABLE [dbo].[Person] CHECK CONSTRAINT [CHK_FirstName]
GO
ALTER TABLE [dbo].[Person]  WITH CHECK ADD  CONSTRAINT [CHK_LastName] CHECK  ((len([LastName])<=(50)))
GO
ALTER TABLE [dbo].[Person] CHECK CONSTRAINT [CHK_LastName]
GO
ALTER TABLE [dbo].[Person]  WITH CHECK ADD  CONSTRAINT [CHK_Person_Gender] CHECK  (([Gender]=(2) OR [Gender]=(1)))
GO
ALTER TABLE [dbo].[Person] CHECK CONSTRAINT [CHK_Person_Gender]
GO
ALTER TABLE [dbo].[Person]  WITH CHECK ADD  CONSTRAINT [CHK_PrimaryPhone] CHECK  (([PrimaryPhone] like '[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]'))
GO
ALTER TABLE [dbo].[Person] CHECK CONSTRAINT [CHK_PrimaryPhone]
GO
ALTER TABLE [dbo].[Person]  WITH CHECK ADD  CONSTRAINT [CHK_SecondaryPhone] CHECK  (([AlternatePhone] like '[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]'))
GO
ALTER TABLE [dbo].[Person] CHECK CONSTRAINT [CHK_SecondaryPhone]
GO
ALTER TABLE [dbo].[Person]  WITH CHECK ADD  CONSTRAINT [CNIC_check] CHECK  (([CNIC] like '[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]'))
GO
ALTER TABLE [dbo].[Person] CHECK CONSTRAINT [CNIC_check]
GO
ALTER TABLE [dbo].[Quotation]  WITH CHECK ADD  CONSTRAINT [CHK_Discount] CHECK  (([Discount]>=(0) AND [Discount]<=(100)))
GO
ALTER TABLE [dbo].[Quotation] CHECK CONSTRAINT [CHK_Discount]
GO
ALTER TABLE [dbo].[Quotation]  WITH CHECK ADD  CONSTRAINT [CHK_ItemQuantity] CHECK  (([ItemQuantity]>(0)))
GO
ALTER TABLE [dbo].[Quotation] CHECK CONSTRAINT [CHK_ItemQuantity]
GO
ALTER TABLE [dbo].[Quotation]  WITH CHECK ADD  CONSTRAINT [CHK_PayableAmount] CHECK  (([PayableAmount]>=(0)))
GO
ALTER TABLE [dbo].[Quotation] CHECK CONSTRAINT [CHK_PayableAmount]
GO
ALTER TABLE [dbo].[Quotation]  WITH CHECK ADD  CONSTRAINT [CHK_TotalAmount] CHECK  (([TotalAmount]>=(0)))
GO
ALTER TABLE [dbo].[Quotation] CHECK CONSTRAINT [CHK_TotalAmount]
GO
ALTER TABLE [dbo].[Salary]  WITH CHECK ADD  CONSTRAINT [CHK_IsPaid] CHECK  (([IsPaid]=(1) OR [IsPaid]=(0)))
GO
ALTER TABLE [dbo].[Salary] CHECK CONSTRAINT [CHK_IsPaid]
GO
ALTER TABLE [dbo].[Salary]  WITH CHECK ADD  CONSTRAINT [CHK_PaidAmount] CHECK  (([PaidAmount]>=(0)))
GO
ALTER TABLE [dbo].[Salary] CHECK CONSTRAINT [CHK_PaidAmount]
GO
ALTER TABLE [dbo].[Salary]  WITH CHECK ADD  CONSTRAINT [CHK_Salary] CHECK  (([Salary]>=(0)))
GO
ALTER TABLE [dbo].[Salary] CHECK CONSTRAINT [CHK_Salary]
GO
ALTER TABLE [dbo].[Salary]  WITH CHECK ADD  CONSTRAINT [CHK_UpdatedOn] CHECK  (([UpdatedOn]<=getdate()))
GO
ALTER TABLE [dbo].[Salary] CHECK CONSTRAINT [CHK_UpdatedOn]
GO
ALTER TABLE [dbo].[Stock]  WITH CHECK ADD  CONSTRAINT [CHK_InitialQuantity] CHECK  (([InitialQuantity]>=(0)))
GO
ALTER TABLE [dbo].[Stock] CHECK CONSTRAINT [CHK_InitialQuantity]
GO
ALTER TABLE [dbo].[Stock]  WITH CHECK ADD  CONSTRAINT [CHK_StockQuantity] CHECK  (([CurrentStockQuantity]>=(0)))
GO
ALTER TABLE [dbo].[Stock] CHECK CONSTRAINT [CHK_StockQuantity]
GO
ALTER TABLE [dbo].[Vehicle]  WITH CHECK ADD  CONSTRAINT [CHK_PlateNumber] CHECK  ((len([PlateNumber])<=(8)))
GO
ALTER TABLE [dbo].[Vehicle] CHECK CONSTRAINT [CHK_PlateNumber]
GO
/****** Object:  StoredProcedure [dbo].[stp_AddEmployee]    Script Date: 10/05/2024 2:29:32 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Fasi Tahir
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[stp_AddEmployee] 
	@Designation INT,
    @EmployeeId INT,
    @EmployeeNo NVARCHAR(50),
    @StartDate DATETIME,
    @BaseSalary DECIMAL(8, 2),
    @FirstName NVARCHAR(50),
    @LastName NVARCHAR(50) = NULL,
    @Gender INT,
    @DateOfBirth DATETIME,
    @Address NVARCHAR(255),
    @PrimaryPhone NVARCHAR(15),
    @AlternatePhone NVARCHAR(15) = NULL,
    @CNIC NVARCHAR(13),
    @Email NVARCHAR(255),
    @Relation INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    DECLARE @PersonId INT;

    BEGIN TRY
        BEGIN TRANSACTION;

        -- Check if a person with the same CNIC and email exists
        DECLARE @ExistingPersonId INT;
        SELECT @ExistingPersonId = PersonID 
        FROM Person 
        WHERE CNIC = @CNIC OR Email = @Email;

        -- Insert employee details
        INSERT INTO Employee
        VALUES (@EmployeeId, @Designation, null, null, @EmployeeNo, @StartDate, null, 1, @BaseSalary);

        -- If a person with the same CNIC and email already exists, use their ID
        IF @ExistingPersonId IS NULL
        BEGIN
            -- Insert new person details
            INSERT INTO Person
            VALUES (@Gender, @CNIC, @FirstName, @LastName, @Email, @PrimaryPhone, @AlternatePhone, @DateOfBirth, @Address);

            -- Get the ID of the newly inserted person
            SET @PersonId = SCOPE_IDENTITY();
        END
        
		ELSE
        BEGIN
            SET @PersonId = @ExistingPersonId;
        END

        -- Insert emergency contact details
        INSERT INTO EmergencyContact
        VALUES (@PersonId, @EmployeeId, @Relation);

        -- Update Applicant
        UPDATE Applicant
        SET IsSelected = 1
        WHERE ApplicantID = @EmployeeId;

        -- Commit transaction
        COMMIT TRANSACTION;
    END TRY
   
    BEGIN CATCH
        -- Rollback transaction if an error occurs
        IF @@TRANCOUNT > 0
        BEGIN
            ROLLBACK TRANSACTION;
        END

        -- Raise error
        DECLARE @ErrorMessage NVARCHAR(4000);
        SET @ErrorMessage = ERROR_MESSAGE();
        RAISERROR(@ErrorMessage, 16, 1);
    END CATCH;

END;
GO
/****** Object:  StoredProcedure [dbo].[stp_DeleteEmployee]    Script Date: 10/05/2024 2:29:32 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Fasi Tahir
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[stp_DeleteEmployee]
	@EmployeeId INT 
AS
BEGIN
	SET NOCOUNT ON;
	BEGIN TRY

		BEGIN Transaction
			UPDATE Employee 
			SET IsActive = 0 
			WHERE EmployeeID = @EmployeeId;


			DELETE FROM Credentials 
			WHERE EmployeeID = @EmployeeId
		COMMIT TRANSACTION
	END TRY

	BEGIN CATCH
		if @@Trancount > 0
			BEGIN
				ROLLBACK TRANSACTION;
			END
		
		DECLARE @ErrorMessage NVARCHAR(4000);
        SET @ErrorMessage = ERROR_MESSAGE();
        RAISERROR(@ErrorMessage, 16, 1);
    END CATCH;
END
GO
/****** Object:  StoredProcedure [dbo].[stp_UpdateEmployeePersonalInfo]    Script Date: 10/05/2024 2:29:32 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Fasi Tahir
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[stp_UpdateEmployeePersonalInfo] 
	-- Add the parameters for the stored procedure here
	@FirstName NVARCHAR(50),
    @LastName NVARCHAR(50) = NULL,
    @Address NVARCHAR(255),
    @PrimaryPhone NVARCHAR(15),
    @AlternatePhone NVARCHAR(15) = NULL,
    @CNIC NVARCHAR(13),
    @Email NVARCHAR(255),
	@EmployeeId INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	BEGIN TRY

    -- Insert statements for procedure here
		UPDATE Person
		SET FirstName = @FirstName, LastName = @LastName, CNIC = @CNIC, PrimaryPhone = @PrimaryPhone, AlternatePhone = @AlternatePhone, Address = @Address, Email = @Email
		WHERE PersonID = @EmployeeId
	
	END TRY

	BEGIN CATCH
		DECLARE @ErrorMessage NVARCHAR(MAX);
		SET @ErrorMessage = Error_Message();
		RAISERROR(@ErrorMessage, 16, 1);
	END CATCH
END
GO
/****** Object:  StoredProcedure [dbo].[stp_UpdateItem]    Script Date: 10/05/2024 2:29:32 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[stp_UpdateItem]
	@ItemName nvarchar(100),
	@ItemID int,
	@Description nvarchar(300),
	@MeasurementUnit nvarchar(50),
	@SalePrice decimal(8,2),
	@UpdatedBy int,
	@UpdatedOn date

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

		UPDATE Item 
		SET ItemName = @ItemName, Description = @Description, MeasurementUnit = @MeasurementUnit, SalePrice = @SalePrice, UpdatedOn = @UpdatedOn, UpdatedBy = @UpdatedBy
		WHERE @ItemID = ItemID
END
GO
USE [master]
GO
ALTER DATABASE [FinalProjectDB] SET  READ_WRITE 
GO
