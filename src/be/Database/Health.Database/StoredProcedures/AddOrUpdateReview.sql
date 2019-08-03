-- =============================================
-- Author:	Tarek Haydar
-- Create date: 27/11/2017
-- Description:	Add review on a specific doctor.
-- =============================================
CREATE PROCEDURE [dbo].[AddOrUpdateReview]
	-- Add the parameters for the stored procedure here
	@doctorId UniqueIdentifier,
	@reviewerName nvarchar(50),
	@description nvarchar(MAX),
	@rank smallint,
	@userId nvarchar(450)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	if exists (SELECT * FROM DoctorReview WHERE DoctorId = @doctorId AND UserId = @userId)
	BEGIN
		UPDATE DoctorReview SET 
			[ReviewerName] = @reviewerName,
			[Description] = @description,
			[Rank] = @rank,
			[IsApproved] = 0
		WHERE DoctorId = @doctorId AND UserId = @userId
	END
	ELSE
	BEGIN
		-- Insert statements for procedure here
		INSERT INTO DoctorReview
							 (Id,DoctorId, ReviewerName, [Description], [Rank], [UserId])
		VALUES        (NEWID(),@doctorId,@reviewerName,@description,@rank, @userId)
	END
	SELECT 0
END