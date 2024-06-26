IF EXISTS (
    SELECT TOP 1 1 FROM sys.databases WHERE name = N'EclipseWorks'
)
BEGIN
    ALTER DATABASE EclipseWorks SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE EclipseWorks;
END
GO

CREATE DATABASE EclipseWorks;
GO

USE EclipseWorks;
GO

CREATE TABLE UserAccount (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(255) NOT NULL,
	Role INT NOT NULL CHECK (Role IN (1,2)) -- 1: User, 2: Manager
);

CREATE TABLE Project (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    UserAccountId INT NOT NULL,
    Name NVARCHAR(255) NOT NULL,
    CONSTRAINT FK_Project_UserAccount FOREIGN KEY (UserAccountId) REFERENCES UserAccount(Id)
);

CREATE TABLE ProjectTask (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    ProjectId INT NOT NULL,
    Title NVARCHAR(255) NOT NULL,
    Description NVARCHAR(MAX) NULL,
    DueDate DATETIME NULL,
    Status INT NOT NULL CHECK (Status IN (1, 2, 3)), -- 1: Pending, 2: In Progress, 3: Done
	Priority INT NOT NULL CHECK (Priority IN (1, 2, 3)), -- 1: Low, 2: Medium, 3: High
    CONSTRAINT FK_Task_Project FOREIGN KEY (ProjectId) REFERENCES Project(Id)
);

CREATE TABLE ProjectTaskComment (
	Id INT IDENTITY(1,1) PRIMARY KEY,
	ProjectTaskId INT NOT NULL,
	UserAccountId INT NOT NULL,
	Comment NVARCHAR(255) NOT NULL,
	SentAt DATETIME NOT NULL,
	CONSTRAINT FK_Comment_ProjectTask FOREIGN KEY (ProjectTaskId) REFERENCES ProjectTask(Id),
	CONSTRAINT FK_Comment_UserAccount FOREIGN KEY (UserAccountId) REFERENCES UserAccount(Id),
);

CREATE TABLE ProjectTaskHistory (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    ProjectTaskId INT NOT NULL,
    PropertyName NVARCHAR(255) NOT NULL,
    OriginalValue NVARCHAR(MAX) NULL,
    CurrentValue NVARCHAR(MAX) NULL,
    ChangedAt DATETIME NOT NULL,
    ChangedByUserId INT NOT NULL,
    ProjectTaskCommentId INT NULL,
    CONSTRAINT FK_History_ProjectTask FOREIGN KEY (ProjectTaskId) REFERENCES ProjectTask(Id),
    CONSTRAINT FK_History_UserAccount FOREIGN KEY (ChangedByUserId) REFERENCES UserAccount(Id),
    CONSTRAINT FK_History_ProjectTaskComment FOREIGN KEY (ProjectTaskCommentId) REFERENCES ProjectTaskComment(Id)
);


CREATE INDEX IDX_Project_UserAccountId ON Project(UserAccountId);
CREATE INDEX IDX_Tasks_ProjectId ON ProjectTask(ProjectId);
CREATE INDEX IDX_ProjectTaskComment_ProjectTask ON ProjectTaskComment(ProjectTaskId);
CREATE INDEX IDX_ProjectTaskComment_UserAccount ON ProjectTaskComment(UserAccountId);
CREATE INDEX IDX_ProjectTaskHistory_ProjectTaskId ON ProjectTaskHistory(ProjectTaskId);
CREATE INDEX IDX_ProjectTaskHistory_ChangedByUserId ON ProjectTaskHistory(ChangedByUserId);
CREATE INDEX IDX_ProjectTaskHistory_ProjectTaskCommentId ON ProjectTaskHistory(ProjectTaskCommentId);
GO

ALTER TABLE ProjectTaskHistory ALTER COLUMN PropertyName NVARCHAR(MAX) NULL
GO

ALTER TABLE ProjectTaskHistory ALTER COLUMN OriginalValue NVARCHAR(MAX) NULL;
GO

ALTER TABLE ProjectTaskHistory ALTER COLUMN CurrentValue NVARCHAR(MAX) NULL;
GO

ALTER TABLE ProjectTask ADD CompletionDate DATETIME NULL;
GO

ALTER TABLE ProjectTask ADD AssignedToUserAccountId INT NOT NULL;
GO

ALTER TABLE ProjectTask
ADD CONSTRAINT FK_ProjectTask_UserAccount
FOREIGN KEY (AssignedToUserAccountId)
REFERENCES UserAccount(Id)
GO

CREATE INDEX IDX_ProjectTask_UserAccount ON ProjectTask(AssignedToUserAccountId)
GO

INSERT INTO UserAccount VALUES ('Jane Doe', 1)
INSERT INTO UserAccount VALUES ('John Smith', 2)