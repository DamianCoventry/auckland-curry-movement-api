CREATE TABLE [dbo].[Member] (
    [ID]             INT           IDENTITY (1, 1) NOT NULL,
    [Name]           VARCHAR (100) NOT NULL,
    [FoundingFather] BIT           NOT NULL,
    [Sponsor]        INT           NULL,
    [IsArchived]     BIT           NOT NULL,
    [ArchiveReason]  VARCHAR (200) NULL
);
GO

ALTER TABLE [dbo].[Member]
    ADD CONSTRAINT [DEFAULT_Member_Archived] DEFAULT ((0)) FOR [IsArchived];
GO

ALTER TABLE [dbo].[Member]
    ADD CONSTRAINT [DEFAULT_Member_FoundingFather] DEFAULT ((0)) FOR [FoundingFather];
GO

ALTER TABLE [dbo].[Member]
    ADD CONSTRAINT [PK_Member] PRIMARY KEY CLUSTERED ([ID] ASC);
GO

ALTER TABLE [dbo].[Member]
    ADD CONSTRAINT [FK_Member_Sponsor] FOREIGN KEY ([Sponsor]) REFERENCES [dbo].[Member] ([ID]);
GO

