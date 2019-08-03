-- =============================================
-- Author:	Tarek Haydar
-- Create date: 29/11/2017
-- Description:	Approve a specific review, after approving it will reflect on the number of viewer and the rating of the doctor.
-- =============================================
CREATE PROCEDURE [dbo].[ApproveReview]
	-- Add the parameters for the stored procedure here
	@reviewId UniqueIdentifier
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	BEGIN TRANSACTION [Trans]

	BEGIN TRY

    -- Set the review as approved.
	UPDATE       DoctorReview
	SET                IsApproved = 1
	WHERE        (Id = @reviewId)

	-- Get doctor id from the review table.
	DECLARE @doctorId UniqueIdentifier
	SELECT @doctorId = DoctorId FROM DoctorReview WHERE (Id like @reviewId)

	-- update the number of reviewer and the rank of the doctor.
	UPDATE	Doctor
	SET	NumberOfReviewer = (SELECT COUNT(*) FROM DoctorReview WHERE (DoctorId like @doctorId)), 
		Rank = (SELECT SUM(Rank) / COUNT(*) FROM DoctorReview WHERE	(DoctorId LIKE @doctorId))
	WHERE (Id = @doctorId)

	COMMIT TRANSACTION [Trans]
	
	SELECT 0

	END TRY
	BEGIN CATCH
	  ROLLBACK TRANSACTION [Trans]
	  SELECT 1000
	END CATCH  

END