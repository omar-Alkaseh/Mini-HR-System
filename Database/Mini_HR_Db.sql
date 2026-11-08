use master;
create database Mini_HR_System
GO
USE Mini_HR_System;
GO


    CREATE TABLE dbo.[User]
    (
        UserId   INT IDENTITY(1,1) NOT NULL,
        FullName  NVARCHAR(200) NOT NULL,
        Email     NVARCHAR(254) NOT NULL,
        [PasswordHash] NVARCHAR(512) NOT NULL,
        IsActive  BIT NOT NULL CONSTRAINT DF_UserAccount_IsActive DEFAULT (1),
        CreatedAt DATETIME2(3) NOT NULL CONSTRAINT DF_UserAccount_CreatedAt DEFAULT (SYSDATETIME()),
        UpdatedAt DATETIME2(3) NOT NULL CONSTRAINT DF_UserAccount_UpdatedAt DEFAULT (SYSDATETIME()),
        CONSTRAINT PK_UserAccount PRIMARY KEY (UserId),
        CONSTRAINT UQ_UserAccount_Email UNIQUE (Email),
        CONSTRAINT CK_UserAccount_FullName CHECK (LEN(LTRIM(RTRIM(FullName))) > 0),
        CONSTRAINT CK_UserAccount_Email CHECK (LEN(LTRIM(RTRIM(Email))) > 0),
        CONSTRAINT CK_UserAccount_PasswordHash CHECK (LEN(LTRIM(RTRIM([PasswordHash]))) > 0)
    );

    CREATE TABLE dbo.Employee
    (
        EmployeeId INT IDENTITY(1,1) NOT NULL,
        FullName   NVARCHAR(200) NOT NULL,
        HireDate   DATE NOT NULL,
        IsActive   BIT NOT NULL CONSTRAINT DF_Employee_IsActive DEFAULT (1),
        CreatedAt  DATETIME2(3) NOT NULL CONSTRAINT DF_Employee_CreatedAt DEFAULT (SYSDATETIME()),
        UpdatedAt  DATETIME2(3) NOT NULL CONSTRAINT DF_Employee_UpdatedAt DEFAULT (SYSDATETIME()),
        CreatedByUserId INT NOT NULL,
        CONSTRAINT PK_Employee PRIMARY KEY (EmployeeId),
        CONSTRAINT CK_Employee_FullName CHECK (LEN(LTRIM(RTRIM(FullName))) > 0),
        CONSTRAINT FK_Employee_CreatedByAdmin FOREIGN KEY (CreatedByUserId)
            REFERENCES dbo.[User] (UserId) ON DELETE NO ACTION ON UPDATE NO ACTION
    );

    CREATE TABLE dbo.EmployeeLeaveBalanced
    (
        BalanceId    INT IDENTITY(1,1) NOT NULL,
        EmployeeId   INT NOT NULL,
        LeaveYear    SMALLINT NOT NULL CONSTRAINT DF_EmployeeLeaveBalanced_LeaveYear DEFAULT (YEAR(SYSDATETIME())),
        EntitledDays DECIMAL(4,1) NOT NULL,
        UsedDays     INT NOT NULL CONSTRAINT DF_EmployeeLeaveBalanced_UsedDays DEFAULT (0),
        CreatedAt    DATETIME2(3) NOT NULL CONSTRAINT DF_EmployeeLeaveBalanced_CreatedAt DEFAULT (SYSDATETIME()),
        UpdatedAt    DATETIME2(3) NOT NULL CONSTRAINT DF_EmployeeLeaveBalanced_UpdatedAt DEFAULT (SYSDATETIME()),
        CONSTRAINT PK_EmployeeLeaveBalanced PRIMARY KEY (BalanceId),
        CONSTRAINT FK_EmployeeLeaveBalanced_Employee FOREIGN KEY (EmployeeId)
            REFERENCES dbo.Employee (EmployeeId) ON DELETE NO ACTION ON UPDATE NO ACTION,
        CONSTRAINT UQ_EmployeeLeaveBalanced_EmployeeId_LeaveYear UNIQUE (EmployeeId, LeaveYear),
        CONSTRAINT CK_EmployeeLeaveBalanced_LeaveYear CHECK (LeaveYear BETWEEN 1 AND 9999),
        CONSTRAINT CK_EmployeeLeaveBalanced_EntitledDays CHECK (EntitledDays BETWEEN 0 AND 30),
        CONSTRAINT CK_EmployeeLeaveBalanced_UsedDays CHECK (UsedDays BETWEEN 0 AND 30)
    );

    CREATE TABLE dbo.EmployeeLeaves
    (
        EmployeeLeaveId INT IDENTITY(1,1) NOT NULL,
        EmployeeId      INT NOT NULL,
        StartDate       DATE NOT NULL,
        EndDate         DATE NOT NULL,
        Days            INT NOT NULL,
        CreatedAt       DATETIME2(3) NOT NULL CONSTRAINT DF_EmployeeLeaves_CreatedAt DEFAULT (SYSDATETIME()),
        RecordedByUserId INT NOT NULL,
        CONSTRAINT PK_EmployeeLeaves PRIMARY KEY (EmployeeLeaveId),
        CONSTRAINT FK_EmployeeLeaves_Employee FOREIGN KEY (EmployeeId)
            REFERENCES dbo.Employee (EmployeeId) ON DELETE NO ACTION ON UPDATE NO ACTION,
        CONSTRAINT FK_EmployeeLeaves_RecordedByUser FOREIGN KEY (RecordedByUserId)
            REFERENCES dbo.[User] (UserId) ON DELETE NO ACTION ON UPDATE NO ACTION,
        CONSTRAINT CK_EmployeeLeaves_DateRange CHECK (StartDate <= EndDate),
        CONSTRAINT CK_EmployeeLeaves_Days CHECK (Days BETWEEN 0 AND 30)
    );

    CREATE INDEX IX_Employee_CreatedByUserId ON dbo.Employee (CreatedByUserId);
    CREATE INDEX IX_EmployeeLeaves_EmployeeId ON dbo.EmployeeLeaves (EmployeeId);
    CREATE INDEX IX_EmployeeLeaves_RecordedByUserId ON dbo.EmployeeLeaves (RecordedByUserId);

