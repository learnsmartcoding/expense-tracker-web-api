-- download links
--https://learn.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms?view=sql-server-ver16
--https://www.microsoft.com/en-us/sql-server/sql-server-downloads
--https://go.microsoft.com/fwlink/p/?linkid=2216019&clcid=0x409&culture=en-us&country=us

-- DB Design for Expense Tracking App --
use master
go

IF NOT EXISTS (SELECT 1 FROM sys.databases WHERE name = 'ExpenseTracker')
BEGIN
    -- Create the database
    CREATE DATABASE ExpenseTracker;
END
ELSE
BEGIN
   DROP DATABASE ExpenseTracker;
END
GO

USE ExpenseTracker;
GO



-- Family Table
CREATE TABLE Family (
    FamilyId INT IDENTITY(1,1),
    FamilyName NVARCHAR(100) NOT NULL,
    CONSTRAINT PK_Family_FamilyId PRIMARY KEY (FamilyId)
);

-- User Profile Table
CREATE TABLE UserProfile (
    UserId INT IDENTITY(1,1),
    DisplayName NVARCHAR(100) NOT NULL CONSTRAINT DF_UserProfile_DisplayName DEFAULT 'Guest',
    FirstName NVARCHAR(50) NOT NULL,
    LastName NVARCHAR(50) NOT NULL,
    Email NVARCHAR(100) NOT NULL,
    AdObjId NVARCHAR(128) NOT NULL,
    FamilyId INT NULL,
    CONSTRAINT PK_UserProfile_UserId PRIMARY KEY (UserId),
    CONSTRAINT FK_UserProfile_Family FOREIGN KEY (FamilyId) REFERENCES Family(FamilyId)
);


-- Expense Type Table (Card, Cash, Cheque)
CREATE TABLE ExpenseType (
    ExpenseTypeId INT IDENTITY(1,1),
    ExpenseTypeName NVARCHAR(50) NOT NULL, --Card, Cash, Cheque
    CONSTRAINT PK_ExpenseType_ExpenseTypeId PRIMARY KEY (ExpenseTypeId)
);

-- Credit Card Table
CREATE TABLE CreditCard (
    CreditCardId INT IDENTITY(1,1),
    CardLastFourDigit NVARCHAR(4) NOT NULL, -- This is to identify which card used so user can pay properly
    CreditCardName NVARCHAR(50) NOT NULL, -- bank name or some identifier    
	UserId INT,
    CONSTRAINT PK_CreditCard_CreditCardId PRIMARY KEY (CreditCardId),	
    CONSTRAINT FK_CreditCard_UserProfile FOREIGN KEY (UserId) REFERENCES UserProfile(UserId)
);

-- Expense Category Table
CREATE TABLE ExpenseCategory (
    ExpenseCategoryId INT IDENTITY(1,1),
    ExpenseCategoryName NVARCHAR(50) NOT NULL, --Food, Fruits, Grocery, Travel, more
    CONSTRAINT PK_ExpenseCategory_ExpenseCategoryId PRIMARY KEY (ExpenseCategoryId)
);

-- Expense Table
CREATE TABLE Expense (
    ExpenseId INT IDENTITY(1,1),
    UserId INT,
    ExpenseAmount DECIMAL(18, 2) NOT NULL,
    ExpenseCategoryId INT,
    ExpenseTypeId INT,
    CreditCardId INT NULL,
    ExpenseDescription NVARCHAR(500) NOT NULL, --have to give details of this purchase
    ExpenseDate DATETIME2 NOT NULL,
    CONSTRAINT PK_Expense_ExpenseId PRIMARY KEY (ExpenseId)
);


-- Expense Table
CREATE TABLE ExpenseItem (
    ExpenseItemId INT IDENTITY(1,1),
	ExpenseId INT,
    UserId INT,
    ExpenseAmount DECIMAL(18, 2) NOT NULL,
    ExpenseCategoryId INT,
    ExpenseTypeId INT,
    CreditCardId INT NULL,
    ExpenseDescription NVARCHAR(500) NOT NULL, --have to give details of this purchase
    ExpenseDate DATETIME2 NOT NULL,
    CONSTRAINT PK_ExpenseItem_ExpenseId PRIMARY KEY (ExpenseItemId),
	CONSTRAINT FK_ExpenseItem_Expense FOREIGN KEY (ExpenseId) REFERENCES Expense(ExpenseId),
	CONSTRAINT FK_ExpenseItem_User FOREIGN KEY (UserId) REFERENCES UserProfile(UserId),
    CONSTRAINT FK_ExpenseItem_Category FOREIGN KEY (ExpenseCategoryId) REFERENCES ExpenseCategory(ExpenseCategoryId),
    CONSTRAINT FK_ExpenseItem_Type FOREIGN KEY (ExpenseTypeId) REFERENCES ExpenseType(ExpenseTypeId),
    CONSTRAINT FK_ExpenseItem_CreditCard FOREIGN KEY (CreditCardId) REFERENCES CreditCard(CreditCardId)
);


CREATE TABLE UserBudget (
    UserBudgetId INT IDENTITY(1,1),
    UserId INT,
    Amount DECIMAL(18, 2) NOT NULL, 
	ItemName NVARCHAR(100) NOT NULL, -- e.g. rent, car insurance, mobile bill, internet, grocery, etc
	ItemDescription NVARCHAR(500) NOT NULL,
	BudgetDate DATETIME2 NOT NULL CONSTRAINT DF_UserBudget_BudgetDate DEFAULT GETDATE(),
	RepeatEveryMonth BIT, -- based on this flag, we will auto populate consucutive months. this logic is based on current month value.
    CONSTRAINT PK_UserBudget_UserBudgetId PRIMARY KEY (UserBudgetId),	
    CONSTRAINT FK_UserBudget_UserProfile FOREIGN KEY (UserId) REFERENCES UserProfile(UserId)
);

CREATE TABLE UserIncome (
    UserIncomeId INT IDENTITY(1,1),
    UserId INT,
    Amount DECIMAL(18, 2) NOT NULL, 	
	IncomeDescription NVARCHAR(500) NOT NULL,
	IncomeDate DATETIME2 NOT NULL CONSTRAINT DF_UserBudget_IncomeDate DEFAULT GETDATE(),
	RepeatEveryMonth BIT, -- based on this flag, we will auto populate consucutive months. this logic is based on current month value.
    CONSTRAINT PK_UserIncome_UserIncomeId PRIMARY KEY (UserIncomeId),	
    CONSTRAINT FK_UserIncome_UserProfile FOREIGN KEY (UserId) REFERENCES UserProfile(UserId)
);


--Family merge request
CREATE TABLE FamilyMemberRequest (
    FamilyMemberRequestId INT IDENTITY(1,1),
	RequestedUserId INT,
	UserMessage NVARCHAR(500) NOT NULL,
    FamilyEmailIds NVARCHAR(100) NOT NULL, -- Initially expected to get only one email id in a request. 
	IsEmailSent BIT,
	IsProcessed BIT,
    CONSTRAINT PK_FamilyMemberRequest_FamilyMemberRequestId PRIMARY KEY (FamilyMemberRequestId),
	CONSTRAINT FK_FamilyMemberRequest_UserProfile FOREIGN KEY (RequestedUserId) REFERENCES UserProfile(UserId)
);

CREATE TABLE EmailCopy (
    EmailCopyId INT IDENTITY(1,1),
    EmailFrom NVARCHAR(100) NOT NULL CONSTRAINT DF_EmailCopy_EmailFrom DEFAULT 'learnsmartcoding@gmail.com',
	EmailTo NVARCHAR(100) NOT NULL, -- we can join this with userprofile table to see what email user received.
    EmailSubject NVARCHAR(100) NOT NULL, 	
	EmailMessage NVARCHAR(2000) NOT NULL, 		
	SentDate DATETIME2 NOT NULL CONSTRAINT DF_EmailCopy_SentDate DEFAULT GETDATE(),	
    CONSTRAINT PK_EmailCopy_EmailCopyId PRIMARY KEY (EmailCopyId)    
);


-- Foreign Key Constraints for Expense Table
ALTER TABLE Expense
ADD CONSTRAINT FK_Expense_User FOREIGN KEY (UserId) REFERENCES UserProfile(UserId),
    CONSTRAINT FK_Expense_Category FOREIGN KEY (ExpenseCategoryId) REFERENCES ExpenseCategory(ExpenseCategoryId),
    CONSTRAINT FK_Expense_Type FOREIGN KEY (ExpenseTypeId) REFERENCES ExpenseType(ExpenseTypeId),
    CONSTRAINT FK_Expense_CreditCard FOREIGN KEY (CreditCardId) REFERENCES CreditCard(CreditCardId);

-- Indexes for faster queries
CREATE INDEX IDX_Expense_UserId ON Expense(UserId);
CREATE INDEX IDX_Expense_FamilyId ON UserProfile(FamilyId);

-- Add some sample data for testing (optional)
INSERT INTO Family (FamilyName) VALUES ('Smith Family'), ('Johnson Family');
INSERT INTO UserProfile (DisplayName, FirstName, LastName, Email, AdObjId, FamilyId) VALUES 
('John Smith', 'John', 'Smith', 'john.smith@example.com', 'adobjid1', 1),
('Jane Smith', 'Jane', 'Smith', 'jane.smith@example.com', 'adobjid2', 1),
('Robert Johnson', 'Robert', 'Johnson', 'robert.johnson@example.com', 'adobjid3', 2);
INSERT INTO ExpenseType (ExpenseTypeName) VALUES ('Card'), ('Cash'), ('Cheque');

-- Insert additional ExpenseCategories
INSERT INTO ExpenseCategory (ExpenseCategoryName) VALUES 
('Housing'),
('Utilities'),
('Transportation'),
('Groceries'),
('Dining Out'),
('Healthcare'),
('Insurance'),
('Entertainment'),
('Education'),
('Personal Care'),
('Clothing'),
('Debt Repayment'),
('Savings'),
('Gifts and Donations'),
('Travel'),
('Subscriptions'),
('Childcare'),
('Pets'),
('Miscellaneous'),
('Purchase'),
('Shopping'),
('Fuel'),
('Misc'),
('Health & wellness'),
('Food & drink');


--insert into [dbo].[CreditCard] (cardLastFourDigit, CreditCardName, UserID)
--values ('4545','chase',4)

--insert into [dbo].[CreditCard] (cardLastFourDigit, CreditCardName, UserID)
--values ('8544','bofa',1)
--insert into [dbo].[CreditCard] (cardLastFourDigit, CreditCardName, UserID)
--values ('8900','chase',2)
--insert into [dbo].[CreditCard] (cardLastFourDigit, CreditCardName, UserID)
--values ('0575','chase',3)