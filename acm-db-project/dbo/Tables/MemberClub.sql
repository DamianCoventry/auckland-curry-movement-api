CREATE TABLE [dbo].[MemberClub] (
    [Member] INT NOT NULL,
    [Club]   INT NOT NULL
);
GO

ALTER TABLE [dbo].[MemberClub]
    ADD CONSTRAINT [PK_ClubMember] PRIMARY KEY CLUSTERED ([Member] ASC, [Club] ASC);
GO

ALTER TABLE [dbo].[MemberClub]
    ADD CONSTRAINT [FK_ClubMember_Club] FOREIGN KEY ([Club]) REFERENCES [dbo].[Club] ([ID]);
GO

ALTER TABLE [dbo].[MemberClub]
    ADD CONSTRAINT [FK_ClubMember_Member] FOREIGN KEY ([Member]) REFERENCES [dbo].[Member] ([ID]);
GO

