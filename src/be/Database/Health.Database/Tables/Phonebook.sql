CREATE TABLE [dbo].[Phonebook]
(
	[Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY, 
    [DoctorAddressId] UNIQUEIDENTIFIER NOT NULL, 
	[Phonenumber] NVARCHAR(100) NOT NULL, 
    [Type] SMALLINT NOT NULL,
	CONSTRAINT [FK_Phonebook_Doctor] FOREIGN KEY ([DoctorAddressId]) REFERENCES [DoctorAddress]([Id])
)
