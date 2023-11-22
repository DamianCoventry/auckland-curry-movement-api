CREATE TABLE [dbo].[MemberLevel] (
    [Dinner] INT NOT NULL,
    [Member] INT NOT NULL,
    [Level]  INT NOT NULL
);
GO

ALTER TABLE [dbo].[MemberLevel]
    ADD CONSTRAINT [FK_MemberLevel_Member] FOREIGN KEY ([Member]) REFERENCES [dbo].[Member] ([ID]);
GO

ALTER TABLE [dbo].[MemberLevel]
    ADD CONSTRAINT [FK_MemberLevel_Dinner] FOREIGN KEY ([Dinner]) REFERENCES [dbo].[Dinner] ([ID]);
GO

ALTER TABLE [dbo].[MemberLevel]
    ADD CONSTRAINT [FK_MemberLevel_Level] FOREIGN KEY ([Level]) REFERENCES [dbo].[Level] ([ID]);
GO

ALTER TABLE [dbo].[MemberLevel]
    ADD CONSTRAINT [PK_MemberLevel] PRIMARY KEY CLUSTERED ([Member] ASC, [Level] ASC, [Dinner] ASC);
GO

