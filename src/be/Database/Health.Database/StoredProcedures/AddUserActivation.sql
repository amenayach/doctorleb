-- =============================================
-- Author:		Amen Ayach
-- Create date: 2018-05-03
-- Description:	Creates user activation
-- =============================================
CREATE PROCEDURE AddUserActivation
	@Id	uniqueidentifier,
	@ShortCode	nvarchar(5),
	@IdentityCode	nvarchar(1024),
	@Created	datetime,
	@Expires	datetime,
	@UserId	nvarchar(450)	
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO UserActivation (Id, ShortCode, IdentityCode, Created, Expires, UserId)
	VALUES        (@Id, @ShortCode, @IdentityCode, @Created, @Expires, @UserId);
	
	SELECT 0;    
END