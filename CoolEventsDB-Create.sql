CREATE DATABASE [CoolEventsDB];
GO;

USE [CoolEventsDB];


-- Create Role table
CREATE TABLE [Role] (
    [Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [Name] NVARCHAR(50) NOT NULL UNIQUE
);

-- Create User table
CREATE TABLE [User] (
    [Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [Username] NVARCHAR(50) NOT NULL UNIQUE,
    [Password] NVARCHAR(100) NOT NULL,
    [FirstName] NVARCHAR(50) NOT NULL,
    [LastName] NVARCHAR(50) NOT NULL,
    [RoleId] INT NOT NULL,
    CONSTRAINT [FK_User_Role] FOREIGN KEY ([RoleId]) REFERENCES [Role]([Id])
);

-- Create Event table
CREATE TABLE [Event] (
    [Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [Name] NVARCHAR(64) NOT NULL,
    [Description] NVARCHAR(255) NOT NULL,
    [Image] VARBINARY(MAX) NOT NULL,
    [Date] DATE NOT NULL
);

-- Create Ticket table
CREATE TABLE [Ticket] (
    [Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [UserId] INT NOT NULL,
    [EventId] INT NOT NULL,
    CONSTRAINT [FK_Ticket_User] FOREIGN KEY ([UserId]) REFERENCES [User]([Id]),
    CONSTRAINT [FK_Ticket_Event] FOREIGN KEY ([EventId]) REFERENCES [Event]([Id])
);


-- Add default roles to Role table
INSERT INTO [Role] ([Name])
VALUES ('Admin'), ('User');


INSERT INTO [User] ([Username], [Password], [FirstName], [LastName], [RoleId])
VALUES ('admin', 'admin', 'admin', 'admin', 1);