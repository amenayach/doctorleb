-- =============================================
-- Author: Tarek Haydar
-- Create date: 30/11/2017
-- Description:	Get regions by kaza.
-- =============================================
CREATE PROCEDURE [dbo].[GetRegionsByKaza]
	-- Add the parameters for the stored procedure here
	@kazaId UniqueIdentifier
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT        NameAr, NameFo, Id, KazaId
	FROM            Region
	WHERE        (KazaId = @kazaId)
END