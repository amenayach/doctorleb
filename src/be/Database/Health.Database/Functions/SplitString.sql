CREATE  FUNCTION [dbo].[SplitString]
(
    @inputString nvarchar(MAX),
    @delimiter char(1) = ','
)
RETURNS 
@Result TABLE 
(
    Value nvarchar(MAX)
)
AS
BEGIN
    DECLARE @chIndex int
    DECLARE @item nvarchar(100)

    -- While there are more delimiters...
    WHILE CHARINDEX(@delimiter, @inputString, 0) <> 0
        BEGIN
            -- Get the index of the first delimiter.
            SET @chIndex = CHARINDEX(@delimiter, @inputString, 0)

            -- Get all of the characters prior to the delimiter and insert the string into the table.
            SELECT @item = SUBSTRING(@inputString, 1, @chIndex - 1)

            IF LEN(@item) > 0
                BEGIN
                    INSERT INTO @Result(Value)
                    VALUES (ltrim(rtrim(@item)))
                END

            -- Get the remainder of the string.
            SELECT @inputString = SUBSTRING(@inputString, @chIndex + 1, LEN(@inputString))
        END

    RETURN 
END