CREATE TABLE [dbo].[Exemption] (
    [ID]             INT            IDENTITY (1, 1) NOT NULL,
    [FoundingFather] INT            NOT NULL,
    [Member]         INT            NOT NULL,
    [Date]           DATE           NOT NULL,
    [ShortReason]    VARCHAR (100)  NOT NULL,
    [LongReason]     VARCHAR (1024) NULL,
    [IsArchived]     BIT            NOT NULL,
    [ArchiveReason]  VARCHAR (200)  NULL
);
GO

ALTER TABLE [dbo].[Exemption]
    ADD CONSTRAINT [FK_Exemption_FoundingFather] FOREIGN KEY ([FoundingFather]) REFERENCES [dbo].[Member] ([ID]);
GO

ALTER TABLE [dbo].[Exemption]
    ADD CONSTRAINT [FK_Exemption_Member] FOREIGN KEY ([Member]) REFERENCES [dbo].[Member] ([ID]);
GO

ALTER TABLE [dbo].[Exemption]
    ADD CONSTRAINT [DEFAULT_Exemption_Date] DEFAULT (getdate()) FOR [Date];
GO

ALTER TABLE [dbo].[Exemption]
    ADD CONSTRAINT [DEFAULT_Exemption_Archived] DEFAULT ((0)) FOR [IsArchived];
GO

ALTER TABLE [dbo].[Exemption]
    ADD CONSTRAINT [PK_Exemption] PRIMARY KEY CLUSTERED ([ID] ASC);
GO

