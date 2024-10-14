CREATE DATABASE ArticlesManagementDB;
GO

USE ArticlesManagementDB;
GO

CREATE TABLE Users (
    Id INT IDENTITY(1,1) PRIMARY KEY, 
    CreatedAt DATETIME NOT NULL DEFAULT GETDATE(),
    UpdatedAt DATETIME NOT NULL DEFAULT GETDATE(),
    Email NVARCHAR(255) NOT NULL,                 
    PasswordHash NVARCHAR(255) NOT NULL,              
    DisplayName NVARCHAR(255) NULL,               
    ImageUrl NVARCHAR(MAX) NULL,                   
    IsVerified BIT NOT NULL DEFAULT 0,             
    CONSTRAINT UQ_Email UNIQUE (Email)             
);

CREATE TABLE VerificationCodes (
    UserId INT NOT NULL,                           
    Code NVARCHAR(10) NOT NULL,        
    CreatedAt DATETIME NOT NULL DEFAULT GETDATE(), 
    ExpiresAt DATETIME NOT NULL,                   
    IsUsed BIT NOT NULL DEFAULT 0,                 
    CONSTRAINT PK_VerificationCodes PRIMARY KEY (UserId, Code), 
    CONSTRAINT FK_User_Verification FOREIGN KEY (UserId) REFERENCES Users(Id)
);

CREATE TABLE Authors (
    UserId INT PRIMARY KEY,
    FirstName NVARCHAR(255) NOT NULL,
    LastName NVARCHAR(255) NOT NULL,
    Biography NVARCHAR(MAX) NULL,
    SocialMediaLinks NVARCHAR(MAX) NULL,  
    ImageUrl NVARCHAR(255) NULL,
    Website NVARCHAR(255) NULL,
    PublishedArticlesCount INT DEFAULT 0,
    CreatedAt DATETIME2 DEFAULT GETDATE(),
    UpdatedAt DATETIME2 DEFAULT GETDATE(),
    CONSTRAINT FK_Authors_Users FOREIGN KEY (UserId) REFERENCES Users(Id) ON DELETE CASCADE
);

CREATE TABLE Articles (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Title NVARCHAR(255) NOT NULL,
    Subtitle NVARCHAR(255) NULL,  
    Content TEXT NOT NULL,
    PublishDate DATETIME NOT NULL,  
    AuthorId INT NOT NULL,           
    ReadCount INT DEFAULT 0,
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME DEFAULT GETDATE(),
    ArticleType INT NOT NULL,     
    FOREIGN KEY (AuthorId) REFERENCES Authors(UserId)
);

CREATE TABLE ArticleLikes (
    ArticleId INT NOT NULL,
    UserId INT NOT NULL,
    CreatedAt DATETIME DEFAULT GETDATE(),
    PRIMARY KEY (ArticleId, UserId),
    FOREIGN KEY (ArticleId) REFERENCES Articles(Id),
    FOREIGN KEY (UserId) REFERENCES Users(Id)
);

CREATE TABLE ArticleComments (
    Id INT PRIMARY KEY IDENTITY(1,1),
    ArticleId INT NOT NULL,
    UserId INT NOT NULL,
    ParentCommentId INT NULL,
    Content TEXT NOT NULL,
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (ArticleId) REFERENCES Articles(Id),
    FOREIGN KEY (UserId) REFERENCES Users(Id),
    FOREIGN KEY (ParentCommentId) REFERENCES ArticleComments(Id)
);


CREATE TABLE ArticleOwners (
    ArticleId INT NOT NULL,
    UserId INT NOT NULL,
    PRIMARY KEY (ArticleId, UserId),  
    FOREIGN KEY (ArticleId) REFERENCES Articles(Id) ON DELETE CASCADE,  
    FOREIGN KEY (UserId) REFERENCES Users(Id) ON DELETE CASCADE  
);


CREATE TABLE Notifications (
    Id INT PRIMARY KEY IDENTITY(1,1),
    UserId INT NOT NULL,
    NotificationType NVARCHAR(50) NOT NULL, 
    Link NVARCHAR(255), 
    CreatedAt DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (UserId) REFERENCES Users(Id)
);
