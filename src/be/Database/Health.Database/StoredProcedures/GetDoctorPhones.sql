-- =============================================
-- Author: Tarek Haydar
-- Create date: 29/11/2017
-- Description:	Gets the phones for doctor.
-- =============================================
CREATE PROCEDURE dbo.[GetDoctorPhones]
	-- Add the parameters for the stored procedure here
	@doctorId UniqueIdentifier
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT        ph.Phonenumber as Phonenumber, ph.[Type] as [Type], ad.Id as [AddressId]
	FROM            Phonebook ph 
	INNER JOIN DoctorAddress ad ON ph.DoctorAddressId = ad.Id
	WHERE        (ad.DoctorId like  @doctorId)
END