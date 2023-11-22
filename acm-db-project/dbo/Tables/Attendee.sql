CREATE TABLE [dbo].[Attendee] (
    [ID]         INT IDENTITY (1, 1) NOT NULL,
    [Dinner]     INT NOT NULL,
    [Member]     INT NOT NULL,
    [IsSponsor]  BIT NOT NULL,
    [IsInductee] BIT NOT NULL
);
GO

ALTER TABLE [dbo].[Attendee]
    ADD CONSTRAINT [FK_Attendee_Member] FOREIGN KEY ([Member]) REFERENCES [dbo].[Member] ([ID]);
GO

ALTER TABLE [dbo].[Attendee]
    ADD CONSTRAINT [FK_Attendee_Dinner] FOREIGN KEY ([Dinner]) REFERENCES [dbo].[Dinner] ([ID]);
GO

ALTER TABLE [dbo].[Attendee]
    ADD CONSTRAINT [DEFAULT_Attendee_IsInductee] DEFAULT ((0)) FOR [IsInductee];
GO

ALTER TABLE [dbo].[Attendee]
    ADD CONSTRAINT [DEFAULT_Attendee_IsSponsor] DEFAULT ((0)) FOR [IsSponsor];
GO

ALTER TABLE [dbo].[Attendee]
    ADD CONSTRAINT [PK_Attendee] PRIMARY KEY CLUSTERED ([ID] ASC);
GO

