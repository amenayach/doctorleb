﻿CREATE TABLE [dbo].[DoctorReview]
(
	[Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY, 
    [DoctorId] UNIQUEIDENTIFIER NOT NULL, 
	[ReviewerName] NVARCHAR(50) NULL, 
    [Description] NVARCHAR(MAX) NULL, 
    [Rank] SMALLINT NOT NULL, 
    [IsApproved] BIT NOT NULL DEFAULT 0, 
    [CreatedOn] DATETIME NOT NULL DEFAULT GETDATE(), 
    [UserId] NVARCHAR(450) NOT NULL, 
    CONSTRAINT [FK_DoctorReview_Doctor] FOREIGN KEY ([DoctorId]) REFERENCES [Doctor]([Id])
)
