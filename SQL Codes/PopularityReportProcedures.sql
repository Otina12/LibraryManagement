-- author --
CREATE PROCEDURE GetAuthorPopularityReport
    @StartDate DATETIME,
    @EndDate DATETIME
AS
BEGIN
    DECLARE @SQL NVARCHAR(MAX)
    
    SET @SQL = N'
    WITH Helper AS (
        SELECT
            a.Id AS AuthorId,
            a.Name AS AuthorName,
            a.Surname AS AuthorSurname,
            SUM(r.Quantity) AS TotalBookCopiesReserved
        FROM
            Authors a
            INNER JOIN BookAuthor ba ON a.Id = ba.AuthorId
            INNER JOIN Books b ON b.Id = ba.BookId
            INNER JOIN Reservations r ON b.Id = r.BookId
        WHERE
            r.ReservationDate BETWEEN @StartDate AND @EndDate
        GROUP BY
            a.Id, a.Name, a.Surname
    )
    SELECT
        AuthorName + '' '' + AuthorSurname AS Name,
        TotalBookCopiesReserved
    FROM
        Helper
	ORDER BY
		TotalBookCopiesReserved DESC'

    EXEC sp_executesql @SQL,
        N'@StartDate DATETIME, @EndDate DATETIME',
        @StartDate, @EndDate
END

EXEC GetAuthorPopularityReport
    @StartDate = '2023-01-01 00:00:00',
    @EndDate = '2024-12-31 23:59:59'


-- publisher --
CREATE PROCEDURE GetPublisherPopularityReport
    @StartDate DATETIME,
    @EndDate DATETIME
AS
BEGIN
    DECLARE @SQL NVARCHAR(MAX)
    
    SET @SQL = N'
    WITH Helper AS (
        SELECT
            p.Id AS PublisherId,
            p.Name AS PublisherName,
            SUM(r.Quantity) AS TotalBookCopiesReserved
        FROM
            Publishers p
            INNER JOIN Books b ON p.Id = b.PublisherId
            INNER JOIN Reservations r ON b.Id = r.BookId
        WHERE
            r.ReservationDate BETWEEN @StartDate AND @EndDate
        GROUP BY
            p.Id, p.Name
    )
    SELECT
        PublisherName AS Name,
        TotalBookCopiesReserved
    FROM
        Helper
	ORDER BY
		TotalBookCopiesReserved DESC'

    EXEC sp_executesql @SQL,
        N'@StartDate DATETIME, @EndDate DATETIME',
        @StartDate, @EndDate
END

EXEC GetPublisherPopularityReport
    @StartDate = '2023-01-01 00:00:00',
    @EndDate = '2024-12-31 23:59:59'


-- genres --
CREATE PROCEDURE GetGenrePopularityReport
    @StartDate DATETIME,
    @EndDate DATETIME
AS
BEGIN
    DECLARE @SQL NVARCHAR(MAX)
    
    SET @SQL = N'
    WITH Helper AS (
        SELECT
            g.Id AS GenreId,
            g.Name AS GenreName,
            SUM(r.Quantity) AS TotalBookCopiesReserved
        FROM
            Genres g
            INNER JOIN OriginalBookGenre obg ON g.Id = obg.GenreId
			INNER JOIN OriginalBooks ob on obg.OriginalBookId = ob.Id
            INNER JOIN Books b ON b.OriginalBookId = ob.Id
            INNER JOIN Reservations r ON b.Id = r.BookId
        WHERE
            r.ReservationDate BETWEEN @StartDate AND @EndDate
        GROUP BY
            g.Id, g.Name
    )
    SELECT
        GenreName AS Name,
        TotalBookCopiesReserved
    FROM
        Helper
	ORDER BY
		TotalBookCopiesReserved DESC'

    EXEC sp_executesql @SQL,
        N'@StartDate DATETIME, @EndDate DATETIME',
        @StartDate, @EndDate
END

EXEC GetGenrePopularityReport
    @StartDate = '2023-01-01 00:00:00',
    @EndDate = '2024-12-31 23:59:59'


-- original book --
CREATE PROCEDURE GetOriginalBookPopularityReport
    @StartDate DATETIME,
    @EndDate DATETIME
AS
BEGIN
    DECLARE @SQL NVARCHAR(MAX)
    
    SET @SQL = N'
    WITH Helper AS (
        SELECT
            ob.Id AS OriginalBookId,
            ob.Title AS OriginalBookTitle,
            SUM(r.Quantity) AS TotalBookCopiesReserved
        FROM
            OriginalBooks ob
            INNER JOIN Books b ON ob.Id = b.OriginalBookId
            INNER JOIN Reservations r ON b.Id = r.BookId
        WHERE
            r.ReservationDate BETWEEN @StartDate AND @EndDate
        GROUP BY
            ob.Id, ob.Title
    )
    SELECT
        OriginalBookTitle AS Name,
        TotalBookCopiesReserved
    FROM
        Helper
	ORDER BY
		TotalBookCopiesReserved DESC'

    EXEC sp_executesql @SQL,
        N'@StartDate DATETIME, @EndDate DATETIME',
        @StartDate, @EndDate
END

EXEC GetOriginalBookPopularityReport
    @StartDate = '2023-01-01 00:00:00',
    @EndDate = '2024-12-31 23:59:59'


-- customers --
CREATE PROCEDURE GetCustomerPopularityReport
    @StartDate DATETIME,
    @EndDate DATETIME
AS
BEGIN
    DECLARE @SQL NVARCHAR(MAX)
    
    SET @SQL = N'
    WITH Helper AS (
        SELECT
            c.Id AS CustomerId,
            c.Name AS CustomerName,
            c.Surname AS CustomerSurname,
            SUM(r.Quantity) AS TotalBookCopiesReserved
        FROM
            Customers c
            INNER JOIN Reservations r ON c.Id = r.CustomerId
        WHERE
            r.ReservationDate BETWEEN @StartDate AND @EndDate
        GROUP BY
            c.Id, c.Name, c.Surname
    )
    SELECT
        CustomerName + '' '' + CustomerSurname AS Name,
        TotalBookCopiesReserved
    FROM
        Helper
	ORDER BY
		TotalBookCopiesReserved DESC'

    EXEC sp_executesql @SQL,
        N'@StartDate DATETIME, @EndDate DATETIME',
        @StartDate, @EndDate
END

EXEC GetCustomerPopularityReport
    @StartDate = '2023-01-01 00:00:00',
    @EndDate = '2024-12-31 23:59:59'


-- employees --
CREATE OR ALTER PROCEDURE GetEmployeePopularityReport
    @StartDate DATETIME,
    @EndDate DATETIME
AS
BEGIN
    DECLARE @SQL NVARCHAR(MAX)
    
    SET @SQL = N'
    WITH Helper AS (
        SELECT
            e.Id AS EmployeeId,
            e.Name AS EmployeeName,
            e.Surname AS EmployeeSurname,
            SUM(r.Quantity) AS TotalBookCopiesReserved
        FROM
            AspNetUsers e
            INNER JOIN Reservations r ON r.EmployeeId = e.Id
        WHERE
            r.ReservationDate BETWEEN @StartDate AND @EndDate
        GROUP BY
            e.Id, e.Name, e.Surname
    )
    SELECT
        EmployeeName + '' '' + EmployeeSurname AS Name,
        TotalBookCopiesReserved
    FROM
        Helper
	ORDER BY
		TotalBookCopiesReserved DESC'

    EXEC sp_executesql @SQL,
        N'@StartDate DATETIME, @EndDate DATETIME',
        @StartDate, @EndDate
END

EXEC GetEmployeePopularityReport
    @StartDate = '2023-01-01 00:00:00',
    @EndDate = '2024-12-31 23:59:59'

-- books --
CREATE PROCEDURE GetBookPopularityReport
    @StartDate DATETIME,
    @EndDate DATETIME
AS
BEGIN
    DECLARE @SQL NVARCHAR(MAX)
    
    SET @SQL = N'
    WITH Helper AS (
        SELECT
            b.Id AS BookId,
			b.ISBN AS BookISBN,
            ob.Title AS BookTitle,
            SUM(r.Quantity) AS TotalBookCopiesReserved
        FROM
            Books b
			INNER JOIN OriginalBooks ob on b.OriginalBookId = ob.Id
            INNER JOIN Reservations r ON b.Id = r.BookId
        WHERE
            r.ReservationDate BETWEEN @StartDate AND @EndDate
        GROUP BY
            b.Id, b.ISBN, ob.Title
    )
    SELECT
        BookTitle + '' - '' + BookISBN AS Name,
        TotalBookCopiesReserved
    FROM
        Helper
	ORDER BY
		TotalBookCopiesReserved DESC'

    EXEC sp_executesql @SQL,
        N'@StartDate DATETIME, @EndDate DATETIME',
        @StartDate, @EndDate
END

EXEC GetBookPopularityReport
    @StartDate = '2023-01-01 00:00:00',
    @EndDate = '2024-12-31 23:59:59'


-- generic --
CREATE PROCEDURE GetPopularityReport
    @Model NVARCHAR(50),
    @StartDate DATETIME,
    @EndDate DATETIME
AS
BEGIN
    IF @Model = 'Author'
        EXEC GetAuthorPopularityReport @StartDate, @EndDate
    ELSE IF @Model = 'Publisher'
        EXEC GetPublisherPopularityReport @StartDate, @EndDate
    ELSE IF @Model = 'Genre'
        EXEC GetGenrePopularityReport @StartDate, @EndDate
    ELSE IF @Model = 'OriginalBook'
        EXEC GetOriginalBookPopularityReport @StartDate, @EndDate
    ELSE IF @Model = 'Customer'
        EXEC GetCustomerPopularityReport @StartDate, @EndDate
    ELSE IF @Model = 'Employee'
        EXEC GetEmployeePopularityReport @StartDate, @EndDate
    ELSE IF @Model = 'Book'
        EXEC GetBookPopularityReport @StartDate, @EndDate
    ELSE
        RAISERROR('Invalid ReportType parameter.', 16, 1)
END

EXEC GetPopularityReport
	@Model = 'Customer', 
	@StartDate = '2023-01-01 00:00:00',
    @EndDate = '2024-12-31 23:59:59'