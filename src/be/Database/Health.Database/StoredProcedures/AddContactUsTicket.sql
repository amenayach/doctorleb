-- =============================================
-- Author: Tarek Haydar
-- Create date: 29/11/2017
-- Description:	Add new contact us ticket.
-- =============================================
CREATE PROCEDURE [dbo].[AddContactUsTicket]
	-- Add the parameters for the stored procedure here
	@name nvarchar(100),
	@phoneNumber nvarchar(50),
	@email nvarchar(50),
	@description nvarchar(MAX)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	INSERT INTO ContactUsTicket
                         (Id, Name, PhoneNumber, Email, [Description])
	VALUES        (NEWID(),@name,@phoneNumber,@email,@description)

	SELECT 0

END