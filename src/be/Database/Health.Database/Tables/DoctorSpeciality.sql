CREATE TABLE [dbo].[DoctorSpeciality]
(
    [DoctorId] UNIQUEIDENTIFIER NOT NULL, 
    [SpecialityId] UNIQUEIDENTIFIER NOT NULL,
	CONSTRAINT [FK_DoctorSpeciality_Doctor] FOREIGN KEY ([DoctorId]) REFERENCES [Doctor]([Id]),
	CONSTRAINT [FK_DoctorSpeciality_Speciality] FOREIGN KEY ([SpecialityId]) REFERENCES [Speciality]([Id]), 
    CONSTRAINT [PK_DoctorSpeciality] PRIMARY KEY ([DoctorId], [SpecialityId])
)
