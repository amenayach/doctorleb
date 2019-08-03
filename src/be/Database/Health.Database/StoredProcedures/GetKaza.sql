-- =============================================
-- Author: Tarek Haydar
-- Create date: 02/12/2017
-- Description:	Get kaza.
-- =============================================
CREATE PROCEDURE [dbo].[GetKaza]
	-- Add the parameters for the stored procedure here
	@id UniqueIdentifier
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT      Id, ProvinceId, NameAr, NameFo
	FROM         Kaza
	WHERE		Id = @id
END