CREATE TABLE [dbo].[KotC] (
    [ID]                 INT IDENTITY (1, 1) NOT NULL,
    [Member]             INT NOT NULL,
    [Dinner]             INT NOT NULL,
    [NumChillisConsumed] INT NOT NULL
);
GO

ALTER TABLE [dbo].[KotC]
    ADD CONSTRAINT [FK_KotC_Member] FOREIGN KEY ([Member]) REFERENCES [dbo].[Member] ([ID]);
GO

ALTER TABLE [dbo].[KotC]
    ADD CONSTRAINT [FK_KotC_Dinner] FOREIGN KEY ([Dinner]) REFERENCES [dbo].[Dinner] ([ID]);
GO

ALTER TABLE [dbo].[KotC]
    ADD CONSTRAINT [DEFAULT_KotC_ChlliCount] DEFAULT ((1)) FOR [NumChillisConsumed];
GO

ALTER TABLE [dbo].[KotC]
    ADD CONSTRAINT [PK_KotC] PRIMARY KEY CLUSTERED ([ID] ASC);
GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'King of the Chillis', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'KotC';
GO

