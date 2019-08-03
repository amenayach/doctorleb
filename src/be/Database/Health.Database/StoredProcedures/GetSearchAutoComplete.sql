CREATE PROCEDURE dbo.GetSearchAutoComplete
	@Keyword nvarchar(256)
AS
BEGIN
	SET NOCOUNT ON;
	SELECT top(10) * from (SELECT distinct dr.* FROM dbo.Doctor dr INNER JOIN 
	(SELECT ('%' +[Value] + '%') as spl FROM dbo.SplitString(@Keyword, N' ')) ss
	ON dr.NameAr LIKE ss.spl OR dr.NameFo LIKE ss.spl) as tt
END