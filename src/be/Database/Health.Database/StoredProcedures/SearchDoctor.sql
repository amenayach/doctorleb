CREATE PROCEDURE [dbo].[SearchDoctor]
	@keyword nvarchar(256) = null,
	@provinceId nvarchar(36) = null,
	@kazaId nvarchar(36) = null,
	@regionId nvarchar(36) = null,
	@specialityId nvarchar(36) = null,
	@pageIndex int = 0,
	@itemPerPage int = 10
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	declare @query nvarchar(2048) = N'
	DECLARE @MatchCount int = 0;
	DECLARE @MatchTable TABLE
	(
	  Id uniqueidentifier, 
	  NameAr nvarchar(150),
	  NameFo nvarchar(150),
	  NumberOfReviewer int, 
	  [Rank] decimal(3,2)
	);

	insert into @MatchTable 
	SELECT drfull.* from dbo.Doctor as drfull where 
	       drfull.NameAr LIKE N'''' + @keyword + '''' OR drfull.NameFo LIKE N'''' + @keyword + '''';
	
	select @MatchCount = count(*) from @MatchTable;
	print @MatchCount;
	select * from @MatchTable union all 
	SELECT * FROM (SELECT * FROM (SELECT distinct t.* FROM (
	(SELECT distinct dr.* FROM dbo.Doctor dr ';
	
	if (NOT @keyword IS NULL AND LEN(LTRIM(RTRIM(@keyword))) > 0)
	BEGIN
		SET @query = @query + N'INNER JOIN 
			(SELECT (''%'' +[value] + ''%'') as spl FROM dbo.SplitString(@Keyword, N'' '')) ss
			ON dr.NameAr LIKE ss.spl OR dr.NameFo LIKE ss.spl)
			UNION ALL 
			(SELECT distinct dr.* FROM dbo.Doctor dr INNER JOIN 
				DoctorAddress AS da ON dr.Id = da.DoctorId INNER JOIN 
				Phonebook AS pb ON da.Id = pb.DoctorAddressId AND (pb.Phonenumber LIKE N''%' + @keyword + '%''))
			) as t ';
	END
	ELSE
	BEGIN
		SET @query = @query + N')) as t ';
	END

	if (NOT @regionId IS NULL AND LEN(LTRIM(RTRIM(@regionId))) > 0)
	BEGIN
		SET @query = @query + N' INNER JOIN 
		(SELECT da.DoctorId FROM DoctorAddress AS da 
		 WHERE (da.RegionId = N''' + ltrim(@regionId) + '%'')) as tad ON t.Id = tad.DoctorId';
	END
	ELSE if (NOT @kazaId IS NULL AND LEN(LTRIM(RTRIM(@kazaId))) > 0)
	BEGIN
		SET @query = @query + ' INNER JOIN 
		(SELECT da.DoctorId
		FROM DoctorAddress as da INNER JOIN 
		Region as rg ON rg.Id = da.RegionId INNER JOIN 
		Kaza as ka ON rg.KazaId = ka.Id
		WHERE (ka.Id = N''' + ltrim(@kazaId) + ''')) as tad ON t.Id = tad.DoctorId';
	END
	ELSE if (NOT @provinceId IS NULL AND LEN(LTRIM(RTRIM(@provinceId))) > 0)
	BEGIN
		SET @query = @query + ' INNER JOIN 
		(SELECT da.DoctorId
		FROM DoctorAddress as da INNER JOIN 
		Region as rg ON rg.Id = da.RegionId INNER JOIN 
		Kaza as ka ON rg.KazaId = ka.Id 
		WHERE (ka.ProvinceId = N''' + ltrim(@provinceId) + ''')) as tad ON t.Id = tad.DoctorId';
	END

	if (NOT @specialityId IS NULL AND LEN(LTRIM(RTRIM(@specialityId))) > 0)
	BEGIN
		SET @query = @query + N' INNER JOIN 
		(SELECT ds.DoctorId FROM DoctorSpeciality AS ds 
		 WHERE (ds.SpecialityId = N''' + ltrim(@specialityId) + '%'')) as tds ON t.Id = tds.DoctorId';
	END
	
	SET @query = @query + N') as tall ORDER BY tall.NameAr OFFSET ' + ltrim(@pageIndex * @itemPerPage) + N' ROWS FETCH NEXT (' + LTRIM(@itemPerPage) + ' - @MatchCount) ROWS ONLY) as tfull';
	
	EXECUTE dbo.sp_executesql @query, N'@Keyword nvarchar(256)', @keyword;
END