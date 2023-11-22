CREATE TABLE [dbo].[RotY] (
    [Year]         INT        NOT NULL,
    [Restaurant]   INT        NOT NULL,
    [NumVotes]     INT        NOT NULL,
    [WinningScore] FLOAT (53) NOT NULL,
    [Presenter]    INT        NULL
);
GO

ALTER TABLE [dbo].[RotY]
    ADD CONSTRAINT [DEFAULT_RotY_Year] DEFAULT ((2010)) FOR [Year];
GO

ALTER TABLE [dbo].[RotY]
    ADD CONSTRAINT [DEFAULT_RotY_VoteCount] DEFAULT ((1)) FOR [NumVotes];
GO

ALTER TABLE [dbo].[RotY]
    ADD CONSTRAINT [DEFAULT_RotY_Rating] DEFAULT ((1.0)) FOR [WinningScore];
GO

ALTER TABLE [dbo].[RotY]
    ADD CONSTRAINT [PK_RotY] PRIMARY KEY CLUSTERED ([Year] ASC);
GO

ALTER TABLE [dbo].[RotY]
    ADD CONSTRAINT [FK_RotY_Member] FOREIGN KEY ([Presenter]) REFERENCES [dbo].[Member] ([ID]);
GO

ALTER TABLE [dbo].[RotY]
    ADD CONSTRAINT [FK_RotY_Restaurant] FOREIGN KEY ([Restaurant]) REFERENCES [dbo].[Restaurant] ([ID]);
GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Restaurant of the Year', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'RotY';
GO

