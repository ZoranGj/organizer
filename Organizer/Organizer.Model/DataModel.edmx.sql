
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 06/02/2017 14:49:33
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
IF OBJECT_ID(N'[dbo].[FK_TodoItemTag_TodoItem]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[TodoItemTag] DROP CONSTRAINT [FK_TodoItemTag_TodoItem];
GO
IF OBJECT_ID(N'[dbo].[FK_TodoItemTag_Tag]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[TodoItemTag] DROP CONSTRAINT [FK_TodoItemTag_Tag];
GO
IF OBJECT_ID(N'[dbo].[FK_UserGoals]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Goals] DROP CONSTRAINT [FK_UserGoals];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Goals]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Goals];
GO
IF OBJECT_ID(N'[dbo].[Activities]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Activities];
GO
IF OBJECT_ID(N'[dbo].[TodoItems]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TodoItems];
GO
IF OBJECT_ID(N'[dbo].[Tags]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Tags];
GO
IF OBJECT_ID(N'[dbo].[Users]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Users];
GO
IF OBJECT_ID(N'[dbo].[TodoItemTag]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TodoItemTag];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Goals'
CREATE TABLE [dbo].[Goals] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [Notes] nvarchar(max)  NULL,
    [Priority] int  NOT NULL,
    [MinHoursPerWeek] smallint  NOT NULL,
    [MaxHoursPerWeek] smallint  NOT NULL,
    [Color] nvarchar(max)  NOT NULL,
    [Start] datetime  NULL,
    [End] datetime  NULL,
    [User_Id] int  NULL
);
GO

-- Creating table 'Activities'
CREATE TABLE [dbo].[Activities] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [Priority] smallint  NOT NULL,
    [Description] nvarchar(max)  NULL,
    [GoalId] int  NOT NULL,
    [Completed] bit  NOT NULL,
    [PlannedCompletionDate] datetime  NULL,
    [CompletionDate] datetime  NULL,
    [StartDate] datetime  NULL
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
    [Resolved] bit  NOT NULL,
    [Duration] int  NOT NULL,
    [Notes] nvarchar(max)  NULL
);
GO

-- Creating table 'Tags'
CREATE TABLE [dbo].[Tags] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Users'
CREATE TABLE [dbo].[Users] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Username] nvarchar(max)  NOT NULL,
    [Email] nvarchar(max)  NOT NULL,
    [Password] nvarchar(max)  NOT NULL,
    [IsAdmin] bit  NOT NULL,
    [DateJoined] datetime  NOT NULL
);
GO

-- Creating table 'TodoItemTag'
CREATE TABLE [dbo].[TodoItemTag] (
    [TodoItems_Id] int  NOT NULL,
    [Tags_Id] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'Goals'
ALTER TABLE [dbo].[Goals]
ADD CONSTRAINT [PK_Goals]
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

-- Creating primary key on [Id] in table 'Tags'
ALTER TABLE [dbo].[Tags]
ADD CONSTRAINT [PK_Tags]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Users'
ALTER TABLE [dbo].[Users]
ADD CONSTRAINT [PK_Users]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [TodoItems_Id], [Tags_Id] in table 'TodoItemTag'
ALTER TABLE [dbo].[TodoItemTag]
ADD CONSTRAINT [PK_TodoItemTag]
    PRIMARY KEY CLUSTERED ([TodoItems_Id], [Tags_Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [GoalId] in table 'Activities'
ALTER TABLE [dbo].[Activities]
ADD CONSTRAINT [FK_CategoryActivity]
    FOREIGN KEY ([GoalId])
    REFERENCES [dbo].[Goals]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CategoryActivity'
CREATE INDEX [IX_FK_CategoryActivity]
ON [dbo].[Activities]
    ([GoalId]);
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

-- Creating foreign key on [TodoItems_Id] in table 'TodoItemTag'
ALTER TABLE [dbo].[TodoItemTag]
ADD CONSTRAINT [FK_TodoItemTag_TodoItem]
    FOREIGN KEY ([TodoItems_Id])
    REFERENCES [dbo].[TodoItems]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Tags_Id] in table 'TodoItemTag'
ALTER TABLE [dbo].[TodoItemTag]
ADD CONSTRAINT [FK_TodoItemTag_Tag]
    FOREIGN KEY ([Tags_Id])
    REFERENCES [dbo].[Tags]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_TodoItemTag_Tag'
CREATE INDEX [IX_FK_TodoItemTag_Tag]
ON [dbo].[TodoItemTag]
    ([Tags_Id]);
GO

-- Creating foreign key on [User_Id] in table 'Goals'
ALTER TABLE [dbo].[Goals]
ADD CONSTRAINT [FK_UserGoals]
    FOREIGN KEY ([User_Id])
    REFERENCES [dbo].[Users]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_UserGoals'
CREATE INDEX [IX_FK_UserGoals]
ON [dbo].[Goals]
    ([User_Id]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------