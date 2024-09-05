-- 0 - Normal
-- 1 - Damaged
-- 2 - Lost
-- 3 - LostAndReturnedAnotherCopy (still lost)

-- books --
CREATE OR ALTER PROCEDURE GetBookBooksDamagedReport
    @StartDate DATETIME,
    @EndDate DATETIME
AS
BEGIN
    DECLARE @SQL NVARCHAR(MAX)
    
    SET @SQL = N'
    WITH Helper AS (
        SELECT 
            ob.Title AS Name,
            SUM(CASE 
                WHEN (bcl.BookCopyAction = 0 AND bcl.CurrentStatus = 1) OR 
                     (bcl.BookCopyAction = 2 AND bcl.CurrentStatus = 1) 
                THEN 1 
                ELSE 0 
            END) AS BookCopiesDamaged,
            SUM(CASE 
                WHEN bcl.BookCopyAction = 3 
                THEN 1 
                ELSE 0 
            END) AS BookCopiesLost
        FROM 
            Books b
        INNER JOIN OriginalBooks ob ON b.OriginalBookId = ob.Id
        INNER JOIN BookCopies bc ON b.Id = bc.BookId
        INNER JOIN BookCopyLogs bcl ON bc.Id = bcl.BookCopyId
        WHERE 
            bcl.ActionTimeStamp BETWEEN @StartDate AND @EndDate
        GROUP BY 
            ob.Title
    )
    SELECT 
        Name,
        BookCopiesDamaged,
        BookCopiesLost
    FROM 
        Helper
    ORDER BY
        BookCopiesDamaged + BookCopiesLost DESC,
        BookCopiesLost DESC,
		Name ASC
    '
    EXEC sp_executesql @SQL,
        N'@StartDate DATETIME, @EndDate DATETIME',
        @StartDate, @EndDate
END


-- customers --
CREATE OR ALTER PROCEDURE GetCustomerBooksDamagedReport
    @StartDate DATETIME,
    @EndDate DATETIME
AS
BEGIN
    DECLARE @SQL NVARCHAR(MAX)
    
    SET @SQL = N'
    WITH Helper AS (
        SELECT 
            c.Name + '' '' + c.Surname AS Name,
            SUM(CASE 
                WHEN (bcl.BookCopyAction = 0 AND bcl.CurrentStatus = 1) OR 
                     (bcl.BookCopyAction = 2 AND bcl.CurrentStatus = 1) 
                THEN 1 
                ELSE 0 
            END) AS BookCopiesDamaged,
            SUM(CASE 
                WHEN bcl.BookCopyAction = 3 
                THEN 1 
                ELSE 0 
            END) AS BookCopiesLost
        FROM 
            Customers c
        INNER JOIN BookCopyLogs bcl ON c.Id = bcl.CustomerId
        WHERE 
            bcl.ActionTimeStamp BETWEEN @StartDate AND @EndDate
        GROUP BY 
            c.Name, c.Surname
    )
    SELECT 
        Name,
        BookCopiesDamaged,
        BookCopiesLost
    FROM 
        Helper
    ORDER BY
        BookCopiesDamaged + BookCopiesLost DESC,
        BookCopiesLost DESC,
		Name ASC
    '
    EXEC sp_executesql @SQL,
        N'@StartDate DATETIME, @EndDate DATETIME',
        @StartDate, @EndDate
END


-- generic --
CREATE PROCEDURE GetBooksDamagedReport
    @Model NVARCHAR(50),
    @StartDate DATETIME,
    @EndDate DATETIME
AS
BEGIN
    IF @Model = 'Book'
        EXEC GetBookBooksDamagedReport @StartDate, @EndDate
    ELSE IF @Model = 'Customer'
        EXEC GetCustomerBooksDamagedReport @StartDate, @EndDate
    ELSE
        RAISERROR('Invalid ReportType parameter.', 16, 1)
END

EXEC GetBooksDamagedReport
	@Model = 'Customer', 
	@StartDate = '2023-01-01 00:00:00',
    @EndDate = '2024-12-31 23:59:59'