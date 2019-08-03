-- =============================================
-- Author: Tarek Haydar
-- Create date: 02/12/2017
-- Description:	Get province.
-- =============================================
CREATE PROCEDURE [dbo].[GetProvince]
	-- Add the parameters for the stored procedure here
	@id UniqueIdentifier
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT        Id, NameAr, NameFo
	FROM            Province
	WHERE		Id = @id
END