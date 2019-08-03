-- =============================================
-- Author: amen ayach
-- Create date: 24/02/2018
-- Description:	Get a paged approved reviews for doctor.
-- =============================================
CREATE PROCEDURE [dbo].[GetPagedReviewsByDoctor]
	@userId nvarchar(50) = NULL,
	@doctorId UniqueIdentifier,
	@pageIndex int =  0,
	@pageSize int =  10
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT Id, ReviewerName, Description, Rank, UserId, CreatedOn
	FROM            DoctorReview
	WHERE        ((IsApproved = 1 or UserId = @userId) and DoctorId = @doctorId)
	ORDER BY CreatedOn Desc
	OFFSET @PageSize * @PageIndex ROWS
	FETCH NEXT @PageSize ROWS ONLY;
END