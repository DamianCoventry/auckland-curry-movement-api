CREATE TABLE [dbo].[Notification] (
    [ID]               INT            IDENTITY (1, 1) NOT NULL,
    [Date]             DATE           NOT NULL,
    [ShortDescription] VARCHAR (100)  NOT NULL,
    [LongDescription]  VARCHAR (1024) NULL,
    [Attendee]         INT            NULL,
    [Club]             INT            NULL,
    [Dinner]           INT            NULL,
    [Exemption]        INT            NULL,
    [KotC]             INT            NULL,
    [Level]            INT            NULL,
    [Member]           INT            NULL,
    [Reservation]      INT            NULL,
    [Restaurant]       INT            NULL,
    [RotY]             INT            NULL,
    [Violation]        INT            NULL
);
GO

ALTER TABLE [dbo].[Notification]
    ADD CONSTRAINT [PK_NotableMemberDate] PRIMARY KEY CLUSTERED ([ID] ASC);
GO

ALTER TABLE [dbo].[Notification]
    ADD CONSTRAINT [FK_Notification_Violation] FOREIGN KEY ([Violation]) REFERENCES [dbo].[Violation] ([ID]);
GO

ALTER TABLE [dbo].[Notification]
    ADD CONSTRAINT [FK_Notification_Dinner] FOREIGN KEY ([Dinner]) REFERENCES [dbo].[Dinner] ([ID]);
GO

ALTER TABLE [dbo].[Notification]
    ADD CONSTRAINT [FK_Notification_Exemption] FOREIGN KEY ([Exemption]) REFERENCES [dbo].[Exemption] ([ID]);
GO

ALTER TABLE [dbo].[Notification]
    ADD CONSTRAINT [FK_Notification_KotC] FOREIGN KEY ([KotC]) REFERENCES [dbo].[KotC] ([ID]);
GO

ALTER TABLE [dbo].[Notification]
    ADD CONSTRAINT [FK_Notification_Level] FOREIGN KEY ([Level]) REFERENCES [dbo].[Level] ([ID]);
GO

ALTER TABLE [dbo].[Notification]
    ADD CONSTRAINT [FK_Notification_Member] FOREIGN KEY ([Member]) REFERENCES [dbo].[Member] ([ID]);
GO

ALTER TABLE [dbo].[Notification]
    ADD CONSTRAINT [FK_Notification_Reservation] FOREIGN KEY ([Reservation]) REFERENCES [dbo].[Reservation] ([ID]);
GO

ALTER TABLE [dbo].[Notification]
    ADD CONSTRAINT [FK_Notification_Attendee] FOREIGN KEY ([Attendee]) REFERENCES [dbo].[Attendee] ([ID]);
GO

ALTER TABLE [dbo].[Notification]
    ADD CONSTRAINT [FK_Notification_Club] FOREIGN KEY ([Club]) REFERENCES [dbo].[Club] ([ID]);
GO

ALTER TABLE [dbo].[Notification]
    ADD CONSTRAINT [FK_Notification_RotY] FOREIGN KEY ([RotY]) REFERENCES [dbo].[RotY] ([Year]);
GO

ALTER TABLE [dbo].[Notification]
    ADD CONSTRAINT [FK_Notification_Restaurant] FOREIGN KEY ([Restaurant]) REFERENCES [dbo].[Restaurant] ([ID]);
GO

ALTER TABLE [dbo].[Notification]
    ADD CONSTRAINT [DEFAULT_NotableMemberDate_Date] DEFAULT (getdate()) FOR [Date];
GO

