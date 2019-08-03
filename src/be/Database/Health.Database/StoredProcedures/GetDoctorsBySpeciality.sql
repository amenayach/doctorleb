CREATE PROCEDURE [dbo].[GetDoctorsBySpeciality]
	-- Add the parameters for the stored procedure here
	@specialityId UniqueIdentifier
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT        Doctor.Id, Doctor.NameAr, Doctor.NameFo, Doctor.NumberOfReviewer, Doctor.Rank
	FROM            Doctor INNER JOIN
							 DoctorSpeciality ON Doctor.Id = DoctorSpeciality.DoctorId
	WHERE        (DoctorSpeciality.SpecialityId = @specialityId)
END