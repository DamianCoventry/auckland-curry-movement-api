CREATE TABLE [dbo].[Reservation] (
    [ID]                     INT        IDENTITY (1, 1) NOT NULL,
    [Organiser]              INT        NOT NULL,
    [Restaurant]             INT        NOT NULL,
    [Year]                   INT        NOT NULL,
    [Month]                  INT        NOT NULL,
    [ExactDateTime]          DATETIME   NOT NULL,
    [NegotiatedBeerPrice]    FLOAT (53) NULL,
    [NegotiatedBeerDiscount] FLOAT (53) NULL
);
GO

ALTER TABLE [dbo].[Reservation]
    ADD CONSTRAINT [CK_Reservation_2] CHECK ([Month]>=(1) AND [Month]<=(12));
GO

ALTER TABLE [dbo].[Reservation]
    ADD CONSTRAINT [CK_Reservation_1] CHECK ([Year]>=(2010));
GO

ALTER TABLE [dbo].[Reservation]
    ADD CONSTRAINT [DEFAULT_Reservation_Month] DEFAULT ((1)) FOR [Month];
GO

ALTER TABLE [dbo].[Reservation]
    ADD CONSTRAINT [DEFAULT_DinnerReservation_DateTime] DEFAULT (getdate()) FOR [ExactDateTime];
GO

ALTER TABLE [dbo].[Reservation]
    ADD CONSTRAINT [DEFAULT_Reservation_Year] DEFAULT ((2010)) FOR [Year];
GO

ALTER TABLE [dbo].[Reservation]
    ADD CONSTRAINT [FK_DinnerReservation_Organiser] FOREIGN KEY ([Organiser]) REFERENCES [dbo].[Member] ([ID]);
GO

ALTER TABLE [dbo].[Reservation]
    ADD CONSTRAINT [FK_DinnerReservation_Restaurant] FOREIGN KEY ([Restaurant]) REFERENCES [dbo].[Restaurant] ([ID]);
GO

ALTER TABLE [dbo].[Reservation]
    ADD CONSTRAINT [PK_DinnerReservation] PRIMARY KEY CLUSTERED ([ID] ASC);
GO

