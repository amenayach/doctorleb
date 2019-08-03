-- =============================================
-- Author:		Amen Ayach
-- Create date: 2018-05-03
-- Description:	Update user activation
-- =============================================
CREATE PROCEDURE UpdateUserActivation
	@Id	uniqueidentifier,
	@Expires	datetime
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE UserActivation SET Expires = @Expires
	WHERE (Id = @Id);
	
	SELECT 0;    
END