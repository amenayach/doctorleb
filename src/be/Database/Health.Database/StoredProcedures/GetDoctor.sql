CREATE PROCEDURE [dbo].[GetDoctor](
	-- Add the parameters for the stored procedure here
	@doctorId UniqueIdentifier 
	)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT        Doctor.*
	FROM            Doctor
	WHERE        (Id LIKE @doctorId)
END