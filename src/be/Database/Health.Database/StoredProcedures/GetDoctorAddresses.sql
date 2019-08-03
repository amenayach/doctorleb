-- =============================================
-- Author: Tarek Haydar
-- Create date: 29/11/2017
-- Description:	Gets the addresses for doctor.
-- =============================================
CREATE PROCEDURE GetDoctorAddresses
	-- Add the parameters for the stored procedure here
	@doctorId UniqueIdentifier
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT        Id,RegionId, Address, Type
	FROM            DoctorAddress
	WHERE DoctorId = @doctorId
END