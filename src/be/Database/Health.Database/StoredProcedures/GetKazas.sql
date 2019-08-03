-- =============================================
-- Author: Tarek Haydar
-- Create date: 30/11/2017
-- Description:	Get kazas.
-- =============================================
CREATE PROCEDURE [dbo].[GetKazas]
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT        Id, ProvinceId, NameAr, NameFo
	FROM            Kaza
END