
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, and Azure
-- --------------------------------------------------
-- Date Created: 10/06/2014 20:22:44
-- Generated from EDMX file: C:\Users\MaddMike\Desktop\GiraffeSpotter(Broken)\GiraffeSpotter\Models\GCF_Models.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [GiraffeSpotterFinal];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_Game_ReserveLocations]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Game_Reserve] DROP CONSTRAINT [FK_Game_ReserveLocations];
GO
IF OBJECT_ID(N'[dbo].[FK_ObservationGallery]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Galleries] DROP CONSTRAINT [FK_ObservationGallery];
GO
IF OBJECT_ID(N'[dbo].[FK_Game_ReserveGiraffe]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Giraffes] DROP CONSTRAINT [FK_Game_ReserveGiraffe];
GO
IF OBJECT_ID(N'[dbo].[FK_ObservationLocations]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Observations] DROP CONSTRAINT [FK_ObservationLocations];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Profile_Pictures]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Profile_Pictures];
GO
IF OBJECT_ID(N'[dbo].[Game_Reserve]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Game_Reserve];
GO
IF OBJECT_ID(N'[dbo].[Giraffes]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Giraffes];
GO
IF OBJECT_ID(N'[dbo].[Locations]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Locations];
GO
IF OBJECT_ID(N'[dbo].[Galleries]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Galleries];
GO
IF OBJECT_ID(N'[dbo].[Observations]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Observations];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Profile_Pictures'
CREATE TABLE [dbo].[Profile_Pictures] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [ImageByte] varbinary(max)  NOT NULL,
    [Extension] nvarchar(max)  NOT NULL,
    [Username] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Game_Reserve'
CREATE TABLE [dbo].[Game_Reserve] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [Type] nvarchar(max)  NOT NULL,
    [Country] nvarchar(max)  NOT NULL,
    [Region] nvarchar(max)  NOT NULL,
    [Category] nvarchar(max)  NOT NULL,
    [Username] nvarchar(max)  NOT NULL,
    [Status] nvarchar(max)  NOT NULL,
    [Location_Id] int  NOT NULL
);
GO

-- Creating table 'Giraffes'
CREATE TABLE [dbo].[Giraffes] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Date_of_Recieval] datetime  NOT NULL,
    [Sub_Species] nvarchar(max)  NOT NULL,
    [Gender] nvarchar(max)  NOT NULL,
    [Age] int  NOT NULL,
    [Place_of_Origin] nvarchar(max)  NOT NULL,
    [Status] nvarchar(max)  NOT NULL,
    [Game_ReserveId] int  NOT NULL
);
GO

-- Creating table 'Locations'
CREATE TABLE [dbo].[Locations] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Latitude] decimal(18,8)  NOT NULL,
    [Longitude] decimal(18,8)  NOT NULL
);
GO

-- Creating table 'Galleries'
CREATE TABLE [dbo].[Galleries] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [ImageBytes] varbinary(max)  NULL,
    [Caption] nvarchar(max)  NOT NULL,
    [ObservationId] int  NULL
);
GO

-- Creating table 'Observations'
CREATE TABLE [dbo].[Observations] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Date_of_Observation] datetime  NOT NULL,
    [Sub_Species] nvarchar(max)  NOT NULL,
    [No_of_Male] int  NOT NULL,
    [No_of_Female] int  NOT NULL,
    [Group_Size] int  NOT NULL,
    [Date_of_Submission] datetime  NOT NULL,
    [Status] nvarchar(max)  NOT NULL,
    [Username] nvarchar(max)  NOT NULL,
    [Comment] nvarchar(max)  NULL,
    [Location_Id] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'Profile_Pictures'
ALTER TABLE [dbo].[Profile_Pictures]
ADD CONSTRAINT [PK_Profile_Pictures]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Game_Reserve'
ALTER TABLE [dbo].[Game_Reserve]
ADD CONSTRAINT [PK_Game_Reserve]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Giraffes'
ALTER TABLE [dbo].[Giraffes]
ADD CONSTRAINT [PK_Giraffes]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Locations'
ALTER TABLE [dbo].[Locations]
ADD CONSTRAINT [PK_Locations]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Galleries'
ALTER TABLE [dbo].[Galleries]
ADD CONSTRAINT [PK_Galleries]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Observations'
ALTER TABLE [dbo].[Observations]
ADD CONSTRAINT [PK_Observations]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [Location_Id] in table 'Game_Reserve'
ALTER TABLE [dbo].[Game_Reserve]
ADD CONSTRAINT [FK_Game_ReserveLocations]
    FOREIGN KEY ([Location_Id])
    REFERENCES [dbo].[Locations]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_Game_ReserveLocations'
CREATE INDEX [IX_FK_Game_ReserveLocations]
ON [dbo].[Game_Reserve]
    ([Location_Id]);
GO

-- Creating foreign key on [ObservationId] in table 'Galleries'
ALTER TABLE [dbo].[Galleries]
ADD CONSTRAINT [FK_ObservationGallery]
    FOREIGN KEY ([ObservationId])
    REFERENCES [dbo].[Observations]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ObservationGallery'
CREATE INDEX [IX_FK_ObservationGallery]
ON [dbo].[Galleries]
    ([ObservationId]);
GO

-- Creating foreign key on [Game_ReserveId] in table 'Giraffes'
ALTER TABLE [dbo].[Giraffes]
ADD CONSTRAINT [FK_Game_ReserveGiraffe]
    FOREIGN KEY ([Game_ReserveId])
    REFERENCES [dbo].[Game_Reserve]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_Game_ReserveGiraffe'
CREATE INDEX [IX_FK_Game_ReserveGiraffe]
ON [dbo].[Giraffes]
    ([Game_ReserveId]);
GO

-- Creating foreign key on [Location_Id] in table 'Observations'
ALTER TABLE [dbo].[Observations]
ADD CONSTRAINT [FK_ObservationLocations]
    FOREIGN KEY ([Location_Id])
    REFERENCES [dbo].[Locations]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ObservationLocations'
CREATE INDEX [IX_FK_ObservationLocations]
ON [dbo].[Observations]
    ([Location_Id]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------