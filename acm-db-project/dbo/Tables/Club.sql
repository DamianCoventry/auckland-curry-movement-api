CREATE TABLE [dbo].[Club] (
    [ID]            INT           IDENTITY (1, 1) NOT NULL,
    [Name]          VARCHAR (100) NOT NULL,
    [IsArchived]    BIT           NOT NULL,
    [ArchiveReason] VARCHAR (200) NULL
);
GO

ALTER TABLE [dbo].[Club]
    ADD CONSTRAINT [DEFAULT_Club_Archived] DEFAULT ((0)) FOR [IsArchived];
GO

ALTER TABLE [dbo].[Club]
    ADD CONSTRAINT [PK_Club] PRIMARY KEY CLUSTERED ([ID] ASC);
GO

