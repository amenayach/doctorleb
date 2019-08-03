-- =============================================
-- Author: Tarek Haydar
-- Create date: 30/11/2017
-- Description:	Get provinces.
-- =============================================
CREATE PROCEDURE [dbo].[GetProvinces]
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT        Id, NameAr, NameFo
	FROM            Province
END