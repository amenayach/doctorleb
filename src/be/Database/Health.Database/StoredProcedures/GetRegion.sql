-- =============================================
-- Author: Tarek Haydar
-- Create date: 03/12/2017
-- Description:	Get region.
-- =============================================
CREATE PROCEDURE [dbo].[GetRegion]
	-- Add the parameters for the stored procedure here
	@id UniqueIdentifier
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT        Region.*
	FROM            Region
	WHERE Id = @id
END