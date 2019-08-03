
-- =============================================
-- Author: Tarek Haydar
-- Create date: 29/11/2017
-- Description:	Get the approved reviews for doctor.
-- =============================================
CREATE PROCEDURE [dbo].[GetReviewsByDoctor]
	@userId nvarchar(50) = NULL,
	@doctorId UniqueIdentifier
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT top(10) Id, ReviewerName, [Description], [Rank], UserId, CreatedOn
	FROM            DoctorReview
	WHERE        ((IsApproved = 1 or UserId = @userId) and DoctorId = @doctorId)
	ORDER BY CreatedOn Desc
END