CREATE TABLE [dbo].[Level] (
    [ID]                  INT           IDENTITY (1, 1) NOT NULL,
    [RequiredAttendances] INT           NOT NULL,
    [Name]                VARCHAR (50)  NOT NULL,
    [Description]         VARCHAR (200) NOT NULL,
    [IsArchived]          BIT           NOT NULL,
    [ArchiveReason]       VARCHAR (50)  NULL
);
GO

ALTER TABLE [dbo].[Level]
    ADD CONSTRAINT [PK_Level] PRIMARY KEY CLUSTERED ([ID] ASC);
GO

ALTER TABLE [dbo].[Level]
    ADD CONSTRAINT [DEFAULT_Level_IsArchived] DEFAULT ((0)) FOR [IsArchived];
GO

ALTER TABLE [dbo].[Level]
    ADD CONSTRAINT [DEFAULT_Level_Description] DEFAULT ('Wear the Golden Turban to a club dinner.') FOR [Description];
GO

ALTER TABLE [dbo].[Level]
    ADD CONSTRAINT [DEFAULT_Level_MinimumAttendances] DEFAULT ((50)) FOR [RequiredAttendances];
GO

ALTER TABLE [dbo].[Level]
    ADD CONSTRAINT [DEFAULT_Level_Name] DEFAULT ('Guru') FOR [Name];
GO

