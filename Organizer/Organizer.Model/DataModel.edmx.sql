
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 03/16/2017 22:37:46
-- Generated from EDMX file: D:\Projects\Prototypes\organizer\organizer\Organizer\Organizer.Model\DataModel.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [Organizer];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_CategoryActivity]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Activities] DROP CONSTRAINT [FK_CategoryActivity];
GO
IF OBJECT_ID(N'[dbo].[FK_ActivityTodoItem]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[TodoItems] DROP CONSTRAINT [FK_ActivityTodoItem];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Categories]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Categories];
GO
IF OBJECT_ID(N'[dbo].[Activities]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Activities];
GO
IF OBJECT_ID(N'[dbo].[TodoItems]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TodoItems];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Categories'
CREATE TABLE [dbo].[Categories] (
    [Id] int  NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [Notes] nvarchar(max)  NULL,
    [Priority] int  NOT NULL,
    [HoursPerWeek] smallint  NOT NULL
);
GO

-- Creating table 'Activities'
CREATE TABLE [dbo].[Activities] (
    [Id] int  NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [Priority] smallint  NOT NULL,
    [Description] nvarchar(max)  NULL,
    [CategoryId] int  NOT NULL,
    [Completed] bit  NOT NULL
);
GO

-- Creating table 'TodoItems'
CREATE TABLE [dbo].[TodoItems] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Description] nvarchar(max)  NOT NULL,
    [AddedOn] datetime  NOT NULL,
    [Deadline] datetime  NOT NULL,
    [ResolvesActivity] bit  NOT NULL,
    [ActivityId] int  NOT NULL,
    [Resolved] bit  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'Categories'
ALTER TABLE [dbo].[Categories]
ADD CONSTRAINT [PK_Categories]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Activities'
ALTER TABLE [dbo].[Activities]
ADD CONSTRAINT [PK_Activities]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'TodoItems'
ALTER TABLE [dbo].[TodoItems]
ADD CONSTRAINT [PK_TodoItems]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [CategoryId] in table 'Activities'
ALTER TABLE [dbo].[Activities]
ADD CONSTRAINT [FK_CategoryActivity]
    FOREIGN KEY ([CategoryId])
    REFERENCES [dbo].[Categories]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CategoryActivity'
CREATE INDEX [IX_FK_CategoryActivity]
ON [dbo].[Activities]
    ([CategoryId]);
GO

-- Creating foreign key on [ActivityId] in table 'TodoItems'
ALTER TABLE [dbo].[TodoItems]
ADD CONSTRAINT [FK_ActivityTodoItem]
    FOREIGN KEY ([ActivityId])
    REFERENCES [dbo].[Activities]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ActivityTodoItem'
CREATE INDEX [IX_FK_ActivityTodoItem]
ON [dbo].[TodoItems]
    ([ActivityId]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------