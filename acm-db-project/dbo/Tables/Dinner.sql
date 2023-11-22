CREATE TABLE [dbo].[Dinner] (
    [ID]               INT        IDENTITY (1, 1) NOT NULL,
    [Reservation]      INT        NOT NULL,
    [CostPerPerson]    FLOAT (53) NULL,
    [NumBeersConsumed] INT        NULL
);
GO

ALTER TABLE [dbo].[Dinner]
    ADD CONSTRAINT [FK_Dinner_Reservation] FOREIGN KEY ([Reservation]) REFERENCES [dbo].[Reservation] ([ID]);
GO

ALTER TABLE [dbo].[Dinner]
    ADD CONSTRAINT [PK_Dinner] PRIMARY KEY CLUSTERED ([ID] ASC);
GO

