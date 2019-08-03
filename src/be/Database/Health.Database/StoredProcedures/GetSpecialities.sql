CREATE PROCEDURE [dbo].[GetSpecialities]
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT        Speciality.*
	FROM            Speciality
	ORDER BY NameAr
END