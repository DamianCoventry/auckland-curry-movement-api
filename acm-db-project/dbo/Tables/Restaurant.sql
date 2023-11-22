CREATE TABLE [dbo].[Restaurant] (
    [ID]             INT           IDENTITY (1, 1) NOT NULL,
    [Name]           VARCHAR (100) NOT NULL,
    [Street Address] VARCHAR (200) NULL,
    [Suburb]         VARCHAR (100) NOT NULL,
    [Phone Number]   VARCHAR (50)  NULL,
    [IsArchived]     BIT           NOT NULL,
    [ArchiveReason]  VARCHAR (200) NULL
);
GO

ALTER TABLE [dbo].[Restaurant]
    ADD CONSTRAINT [DEFAULT_Restaurant_Archived] DEFAULT ((0)) FOR [IsArchived];
GO

ALTER TABLE [dbo].[Restaurant]
    ADD CONSTRAINT [PK_Restaurant] PRIMARY KEY CLUSTERED ([ID] ASC);
GO

