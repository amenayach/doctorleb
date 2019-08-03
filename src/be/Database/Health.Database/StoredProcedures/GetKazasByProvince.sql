-- =============================================
-- Author: Tarek Haydar
-- Create date: 30/11/2017
-- Description:	Get kazas by province.
-- =============================================
CREATE PROCEDURE [dbo].[GetKazasByProvince]
	-- Add the parameters for the stored procedure here
	@provinceId UniqueIdentifier
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT        Id, NameAr, NameFo, ProvinceId
	FROM            Kaza
	WHERE        (ProvinceId = @provinceId)
END