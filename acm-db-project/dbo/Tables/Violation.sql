CREATE TABLE [dbo].[Violation] (
    [ID]              INT           IDENTITY (1, 1) NOT NULL,
    [Dinner]          INT           NOT NULL,
    [FoundingFather]  INT           NOT NULL,
    [Member]          INT           NOT NULL,
    [Description]     VARCHAR (200) NOT NULL,
    [IndianHotCurry]  BIT           NOT NULL,
    [Reinduction]     BIT           NOT NULL,
    [OtherPunishment] VARCHAR (100) NULL
);
GO

ALTER TABLE [dbo].[Violation]
    ADD CONSTRAINT [DEFAULT_Violation_Reinduction] DEFAULT ((0)) FOR [Reinduction];
GO

ALTER TABLE [dbo].[Violation]
    ADD CONSTRAINT [DEFAULT_Violation_IndianHotCurry] DEFAULT ((0)) FOR [IndianHotCurry];
GO

ALTER TABLE [dbo].[Violation]
    ADD CONSTRAINT [FK_Violation_FoundingFather] FOREIGN KEY ([FoundingFather]) REFERENCES [dbo].[Member] ([ID]);
GO

ALTER TABLE [dbo].[Violation]
    ADD CONSTRAINT [FK_Violation_Member] FOREIGN KEY ([Member]) REFERENCES [dbo].[Member] ([ID]);
GO

ALTER TABLE [dbo].[Violation]
    ADD CONSTRAINT [FK_Violation_Dinner] FOREIGN KEY ([Dinner]) REFERENCES [dbo].[Dinner] ([ID]);
GO

ALTER TABLE [dbo].[Violation]
    ADD CONSTRAINT [PK_Violation] PRIMARY KEY CLUSTERED ([ID] ASC);
GO

