-- =============================================
-- Author:		Amen Ayach
-- Create date: 2018-05-03
-- Description:	Creates user activation
-- =============================================
CREATE PROCEDURE [dbo].[GetUserActivation]
	@ShortCode	nvarchar(5) = NULL,
	@UserId	nvarchar(450)	
AS
BEGIN
	SET NOCOUNT ON;

	SELECT  Id, ShortCode, IdentityCode, Created, Expires, UserId
	FROM    UserActivation
	WHERE UserId = @UserId and (@ShortCode	= '' or @ShortCode	= NULL or ShortCode = @ShortCode);
END