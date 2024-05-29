-- Table: Lookup
USE FinalProjectDB;
GO


CREATE TABLE [Lookup] (
    lookupID INT identity(1,1)  not null,
    Value NVARCHAR(50) not null,
    Description NVARCHAR(255)  not null, CONSTRAINT [PK_Lookup] PRIMARY KEY CLUSTERED 
(
	[LookupId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

SET IDENTITY_INSERT [dbo].[Lookup] ON

INSERT [dbo].[Lookup] ([LookupID], [Value], [Description]) VALUES (1, N'Male', N'Gender')
INSERT [dbo].[Lookup] ([LookupId], [Value], [Description]) VALUES (2, N'Female', N'Gender')
INSERT [dbo].[Lookup] ([LookupId], [Value], [Description]) VALUES (3, N'Worker', N'Designation')
INSERT [dbo].[Lookup] ([LookupId], [Value], [Description]) VALUES (4, N'Accountant', N'Designation')

INSERT [dbo].[Lookup] ([LookupId], [Value], [Description]) VALUES (5, N'HR', N'Designation')
INSERT [dbo].[Lookup] ([LookupId], [Value], [Description]) VALUES (6, N'InventoryManager', N'Designation')

INSERT [dbo].[Lookup] ([LookupId], [Value], [Description]) VALUES (7, N'Sibling', N'Relationship')
INSERT [dbo].[Lookup] ([LookupId], [Value], [Description]) VALUES (8, N'Spouse', N'Relationship')
INSERT [dbo].[Lookup] ([LookupId], [Value], [Description]) VALUES (9, N'Child', N'Relationship')
INSERT [dbo].[Lookup] ([LookupId], [Value], [Description]) VALUES (10, N'Parent', N'Relationship')
INSERT [dbo].[Lookup] ([LookupId], [Value], [Description]) VALUES (11, N'Other', N'Relationship')

-- Inserting additional rows for VehicleType
INSERT [dbo].[Lookup] ([LookupId], [Value], [Description]) VALUES (12, N'Truck', N'VehicleType')
INSERT [dbo].[Lookup] ([LookupId], [Value], [Description]) VALUES (13, N'Van', N'VehicleType')
INSERT [dbo].[Lookup] ([LookupId], [Value], [Description]) VALUES (14, N'Rickshaw', N'VehicleType')
INSERT [dbo].[Lookup] ([LookupId], [Value], [Description]) VALUES (15, N'PickUp', N'VehicleType')
INSERT [dbo].[Lookup] ([LookupId], [Value], [Description]) VALUES (16, N'Car', N'VehicleType')

SET IDENTITY_INSERT [dbo].[Lookup] OFF




--SET IDENTITY_INSERT [dbo].[Lookup] OFF


-- Table: Person
CREATE TABLE Person (
    PersonID INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
    Gender INT FOREIGN KEY REFERENCES [Lookup](LookupId) NOT NULL, -- Corrected foreign key definition
    CNIC NVARCHAR(13) NOT NULL,
    FirstName NVARCHAR(50) NOT NULL,
    LastName NVARCHAR(50),
    Email NVARCHAR(255) NOT NULL,
    PrimaryPhone NVARCHAR(15) NOT NULL,
    AlternatePhone NVARCHAR(15),
    DateOfBirth DATETIME NOT NULL,
    [Address] NVARCHAR(255) NOT NULL,
    CONSTRAINT CHK_FirstName CHECK (LEN(FirstName) <= 50),
    CONSTRAINT CHK_LastName CHECK (LEN(LastName) <= 50),
    CONSTRAINT CHK_DOB CHECK (DateOfBirth <= GETDATE() AND DateOfBirth >= DATEADD(YEAR, -80, GETDATE())),
    CONSTRAINT CHK_Email_Format CHECK (Email LIKE '%@%.%' AND LEN(Email) <= 255),
	
	CONSTRAINT UC_Email UNIQUE (Email),
	CONSTRAINT UC_CNIC UNIQUE (CNIC),

    CONSTRAINT CHK_PrimaryPhone CHECK (PrimaryPhone LIKE '[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]'),
    CONSTRAINT CHK_SecondaryPhone CHECK (AlternatePhone LIKE '[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]'),
    CONSTRAINT CNIC_check CHECK (CNIC LIKE '[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]'),
	CONSTRAINT CHK_Person_Gender CHECK (Gender IN (1,2))
);
GO

-- Table: Applicant
CREATE TABLE Applicant (
    ApplicantID INT PRIMARY KEY NOT NULL,
    FOREIGN KEY (ApplicantID) REFERENCES Person(PersonID),
    DesiredDesignation INT FOREIGN KEY REFERENCES [Lookup](LookupId) NOT NULL,
    isSelected BIT,
    isShortlisted BIT,
    isRejected BIT,
	CVName nvarchar(50),
    PictureName nvarchar(50),
	CONSTRAINT CHK_DesiredDesignation CHECK (DesiredDesignation IN (3,4,5,6)),
    CONSTRAINT CHK_isSelected CHECK (isSelected IN (0, 1)),
    CONSTRAINT CHK_isShortlisted CHECK (isShortlisted IN (0, 1))
);
GO



-- Table: Employee
CREATE TABLE Employee (
    EmployeeID INT PRIMARY KEY NOT NULL,
    FOREIGN KEY (EmployeeID) REFERENCES Applicant(ApplicantID),
	DesignationID INT FOREIGN KEY REFERENCES [Lookup](LookupId) NOT NULL,
    UpdatedBy INT FOREIGN KEY REFERENCES Employee(EmployeeID),
    UpdatedOn DATE,
    EmployeeNumber NVARCHAR(50) NOT NULL, 
    StartDate DATE NOT NULL,
    EndDate DATE,
    IsActive BIT NOT NULL,
	BaseSalary DECIMAL(8,2) NOT NULL,

	CONSTRAINT UC_EmployeeNumber UNIQUE(EmployeeNumber),
    CONSTRAINT CHK_BaseSalary CHECK (BaseSalary >= 0),
	CONSTRAINT CHK_StartDate CHECK (StartDate <= GETDATE()),
    CONSTRAINT CHK_EndDate CHECK (EndDate IS NULL OR EndDate >= StartDate),
    CONSTRAINT CHK_IsActive CHECK (IsActive IN (0, 1))
);
GO

-- Table: EmergencyContact
CREATE TABLE EmergencyContact (
    EmergencyContactID INT NOT NULL,
    FOREIGN KEY (EmergencyContactID) REFERENCES Person(PersonID),
    EmployeeID INT PRIMARY KEY NOT NULL, 
	FOREIGN KEY (EmployeeID) REFERENCES Employee(EmployeeID),
    Relationship INT FOREIGN KEY REFERENCES [Lookup](LookupId) NOT NULL,
    CONSTRAINT CHK_Relationship CHECK (Relationship IN (7,8,9,10,11))
);

GO


-- Table: Salary
CREATE TABLE Salary (
    EmployeeID INT NOT NULL,
    PaidDate DATE,
    IsPaid BIT NOT NULL,
    PaidAmount DECIMAL(8,2),
    Salary DECIMAL(8,2) NOT NULL,
    ExpectedDate DATE,
    UpdatedBy INT FOREIGN KEY REFERENCES Employee(EmployeeID),
    UpdatedOn DATE,
    PRIMARY KEY (EmployeeID, PaidDate),
    FOREIGN KEY (EmployeeID) REFERENCES Employee(EmployeeID),
    
	CONSTRAINT CHK_IsPaid CHECK (IsPaid IN (0, 1)),
    CONSTRAINT CHK_PaidAmount CHECK (PaidAmount >= 0),
	CONSTRAINT CHK_Salary CHECK (Salary >= 0),
    CONSTRAINT CHK_UpdatedOn CHECK (UpdatedOn <= GETDATE())
);

GO

-- Table: Item
CREATE TABLE Item (
    ItemID INT IDENTITY(1,1) PRIMARY KEY NOT NULL, -------Item ID should be auto generated
    ItemName NVARCHAR(100) NOT NULL,
    AvailableQuantity INT NOT NULL,
    Description NVARCHAR(300),
    MeasurementUnit NVARCHAR(50) NOT NULL,
    SalePrice DECIMAL(8,2) NOT NULL,
    UpdatedBy INT FOREIGN KEY REFERENCES Employee(EmployeeID),
    UpdatedOn DATE,
    
	
	CONSTRAINT UC_ItemName UNIQUE (ItemName),
    CONSTRAINT CHK_AvailableQuantity CHECK (AvailableQuantity >= 0),
    CONSTRAINT CHK_SalePrice CHECK (SalePrice >= 0),
    CONSTRAINT CHK_ItemName_Length CHECK (LEN(ItemName) <= 100),
    CONSTRAINT CHK_Description_Length CHECK (LEN(Description) <= 300),
    CONSTRAINT CHK_MeasurementUnit_Length CHECK (LEN(MeasurementUnit) <= 50)
);

GO

-- Table: Credentials
CREATE TABLE Credentials (
  Username NVARCHAR(50) PRIMARY KEY NOT NULL,
  Password NVARCHAR(14) not null,
  EmployeeID INT FOREIGN KEY REFERENCES Employee(EmployeeID) NOT NULL,
  UpdatedBy INT FOREIGN KEY REFERENCES Employee(EmployeeID),
  UpdatedOn DATE
);
GO

-- Table: Client
CREATE TABLE Client (
    ClientID INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
    ClientName NVARCHAR(100) NOT NULL,
    City NVARCHAR(50),
    ClientEmail NVARCHAR(255) NOT NULL,
    PhoneNumber NVARCHAR(15),
    UpdatedBy INT FOREIGN KEY REFERENCES Employee(EmployeeID),
    UpdatedOn DATE,
    

	CONSTRAINT UC_ClientEmail UNIQUE(ClientEmail),
	CONSTRAINT CHK_ClientEmail_Format CHECK (ClientEmail LIKE '%@%.%' AND LEN(ClientEmail) <= 255),
    CONSTRAINT CHK_PhoneNumber_Format CHECK (PhoneNumber LIKE '[0-9]%'),
    CONSTRAINT CHK_City_Length CHECK (LEN(City) <= 50)
);

GO

-- Table: Quotation
CREATE TABLE Quotation (
    QuotationID INT NOT NULL,
    ItemID INT NOT NULL,
    ClientID INT NOT NULL,
    UpdatedBy INT,
    UpdatedOn DATE,
    ItemQuantity INT NOT NULL,
    Discount DECIMAL(3,2),
    TotalAmount DECIMAL(8,2),
    PayableAmount DECIMAL(8,2),
    PRIMARY KEY (QuotationID, ItemID),
    FOREIGN KEY (ItemID) REFERENCES Item(ItemID),
    FOREIGN KEY (ClientID) REFERENCES Client(ClientID),
    FOREIGN KEY (UpdatedBy) REFERENCES Employee(EmployeeID),



    CONSTRAINT CHK_Discount CHECK (Discount BETWEEN 0 AND 100),
	CONSTRAINT CHK_ItemQuantity CHECK (ItemQuantity > 0),
	CONSTRAINT CHK_TotalAmount CHECK (TotalAmount >= 0),
	CONSTRAINT CHK_PayableAmount CHECK (PayableAmount >= 0)
);

GO


-- Table: Driver
CREATE TABLE Driver (
    DriverID INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
    DriverName NVARCHAR(100),
    DriverCNIC NVARCHAR(13) NOT NULL,
    LicenseNumber NVARCHAR(11) NOT NULL,
    UpdatedBy INT, FOREIGN KEY (UpdatedBy) REFERENCES Employee(EmployeeID),
    UpdatedOn DATE,
    LicenseExpiryDate DATE,
    
	
	CONSTRAINT UQ_DriverCNIC UNIQUE (DriverCNIC),
    CONSTRAINT UQ_LicenseNumber UNIQUE (LicenseNumber),
    CONSTRAINT CHK_LicenseExpiryDate CHECK (LicenseExpiryDate >= GETDATE()),
	CONSTRAINT DriverCNIC_check CHECK (DriverCNIC LIKE '[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]')
);

GO

-- Table: Vehicle
CREATE TABLE Vehicle (
  VehicleID INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
  DriverID INT FOREIGN KEY REFERENCES Driver(DriverID) NOT NULL,
  VehicleType INT FOREIGN KEY REFERENCES Lookup(LookupId) NOT NULL,
  PlateNumber NVARCHAR(8) NOT NULL
  
  CONSTRAINT CHK_PlateNumber CHECK (LEN(PlateNumber) <= 8)
);
GO


-- Table: Delivery
CREATE TABLE Delivery (
  DeliveryID INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
  VehicleID INT FOREIGN KEY REFERENCES Vehicle(VehicleID) NOT NULL,
  EmployeeID INT FOREIGN KEY REFERENCES Employee(EmployeeID) NOT NULL,
  DriverID INT FOREIGN KEY REFERENCES Driver(DriverID) NOT NULL,
  ClientID INT FOREIGN KEY REFERENCES Client(ClientID) NOT NULL,
  DeliveryStatus INT FOREIGN KEY REFERENCES Lookup(LookupId) NOT NULL,
  DeliveryDate DATE,
  UpdatedBy INT FOREIGN KEY REFERENCES Employee(EmployeeID), --Status is updateable
  UpdatedOn DATE,
  Payment DECIMAL(8,2)
  
  
  
  CONSTRAINT CHK_Payment CHECK (Payment >= 0)
);
GO

CREATE TABLE Stock (
    StockId INT NOT NULL, 
    ItemId INT FOREIGN KEY REFERENCES Item(ItemId) NOT NULL,
	PRIMARY KEY(StockId, ItemId),
	UpdatedBy INT FOREIGN KEY REFERENCES Employee(EmployeeID),
    UpdatedOn DATE,
    StockArrivalDate DATE NOT NULL,
    CostPrice DECIMAL(8, 2) NOT NULL,
    CurrentStockQuantity INT NOT NULL,
    InitialQuantity INT NOT NULL,
  
  
  CONSTRAINT CHK_StockQuantity CHECK (CurrentStockQuantity >= 0),
    CONSTRAINT CHK_InitialQuantity CHECK (InitialQuantity >= 0)
);
GO


-- Table: Order
CREATE TABLE [Order] (
  OrderID INT IDENTITY(1,1) PRIMARY KEY,
  QuotationID INT NOT NULL,
  ItemID INT,  -- Add ItemID to match the composite key in Quotation
  FOREIGN KEY (QuotationID, ItemID) REFERENCES Quotation(QuotationID, ItemID),
  ClientID INT FOREIGN KEY REFERENCES Client(ClientID) NOT NULL,
  UpdatedBy INT FOREIGN KEY REFERENCES Employee(EmployeeID),
  UpdatedOn DATE,
  RequiredDate DATE,
  OrderDate DATE NOT NULL,
  IsServed BIT NOT NULL,
  
  
  CONSTRAINT CHK_OrderDate CHECK (OrderDate <= GETDATE()), -- OrderDate should not be in the future
  CONSTRAINT CHK_RequiredDate CHECK (RequiredDate >= OrderDate) -- RequiredDate should be on or after OrderDate
);
GO


-- Table: OtherExpenses
CREATE TABLE OtherExpenses (
  ExpenseID INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
  Description NVARCHAR(255) NOT NULL,
  ExpenseTitle NVARCHAR(50) NOT NULL,
  ExpenseDate DATE, 
  ExpenseAmount DECIMAL(8,2) NOT NULL,
  CONSTRAINT CHK_ExpenseDate CHECK (ExpenseDate <= GETDATE()) -- ExpenseDate should not be in the future
);
GO
