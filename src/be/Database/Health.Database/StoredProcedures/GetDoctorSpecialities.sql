-- =============================================
-- Author: Amen Ayach
-- Create date: 18/03/2017
-- Description:	Gets the doctor specialities.
-- =============================================
CREATE PROCEDURE [dbo].[GetDoctorSpecialities]
	@doctorId UniqueIdentifier
AS
BEGIN
	SET NOCOUNT ON;

	SELECT DoctorSpeciality.DoctorId, Speciality.Id, Speciality.NameAr, Speciality.NameFr, Speciality.NameEn
	FROM DoctorSpeciality INNER JOIN Speciality ON DoctorSpeciality.SpecialityId = Speciality.Id
	WHERE (DoctorSpeciality.DoctorId like  @doctorId)
END