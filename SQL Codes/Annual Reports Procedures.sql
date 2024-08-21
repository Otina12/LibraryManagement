-- authors --
CREATE OR ALTER PROCEDURE GetAuthorAnnualReport
    @Year INT
AS
BEGIN
    DECLARE @SQL NVARCHAR(MAX)
    
    SET @SQL = N'
    SELECT 
        a.Name,
        SUM(CASE WHEN MONTH(r.ReservationDate) = 1 THEN r.Quantity ELSE 0 END) AS January,
        SUM(CASE WHEN MONTH(r.ReservationDate) = 2 THEN r.Quantity ELSE 0 END) AS February,
        SUM(CASE WHEN MONTH(r.ReservationDate) = 3 THEN r.Quantity ELSE 0 END) AS March,
        SUM(CASE WHEN MONTH(r.ReservationDate) = 4 THEN r.Quantity ELSE 0 END) AS April,
        SUM(CASE WHEN MONTH(r.ReservationDate) = 5 THEN r.Quantity ELSE 0 END) AS May,
        SUM(CASE WHEN MONTH(r.ReservationDate) = 6 THEN r.Quantity ELSE 0 END) AS June,
        SUM(CASE WHEN MONTH(r.ReservationDate) = 7 THEN r.Quantity ELSE 0 END) AS July,
        SUM(CASE WHEN MONTH(r.ReservationDate) = 8 THEN r.Quantity ELSE 0 END) AS August,
        SUM(CASE WHEN MONTH(r.ReservationDate) = 9 THEN r.Quantity ELSE 0 END) AS September,
        SUM(CASE WHEN MONTH(r.ReservationDate) = 10 THEN r.Quantity ELSE 0 END) AS October,
        SUM(CASE WHEN MONTH(r.ReservationDate) = 11 THEN r.Quantity ELSE 0 END) AS November,
        SUM(CASE WHEN MONTH(r.ReservationDate) = 12 THEN r.Quantity ELSE 0 END) AS December
    FROM 
        Authors a
	INNER JOIN BookAuthor ba on ba.AuthorId = a.Id
    INNER JOIN Books b ON b.Id = ba.BookId 
    INNER JOIN Reservations r ON r.BookId = b.Id
    WHERE 
        YEAR(r.ReservationDate) = @Year
    GROUP BY 
        a.Name
    '

    EXEC sp_executesql @SQL,
        N'@Year INT',
        @Year
END

EXEC GetAuthorAnnualReport
    @Year = '2024'


-- publishers --
CREATE OR ALTER PROCEDURE GetPublisherAnnualReport
    @Year INT
AS
BEGIN
    DECLARE @SQL NVARCHAR(MAX)
    
    SET @SQL = N'
    SELECT 
        p.Name,
        SUM(CASE WHEN MONTH(r.ReservationDate) = 1 THEN r.Quantity ELSE 0 END) AS January,
        SUM(CASE WHEN MONTH(r.ReservationDate) = 2 THEN r.Quantity ELSE 0 END) AS February,
        SUM(CASE WHEN MONTH(r.ReservationDate) = 3 THEN r.Quantity ELSE 0 END) AS March,
        SUM(CASE WHEN MONTH(r.ReservationDate) = 4 THEN r.Quantity ELSE 0 END) AS April,
        SUM(CASE WHEN MONTH(r.ReservationDate) = 5 THEN r.Quantity ELSE 0 END) AS May,
        SUM(CASE WHEN MONTH(r.ReservationDate) = 6 THEN r.Quantity ELSE 0 END) AS June,
        SUM(CASE WHEN MONTH(r.ReservationDate) = 7 THEN r.Quantity ELSE 0 END) AS July,
        SUM(CASE WHEN MONTH(r.ReservationDate) = 8 THEN r.Quantity ELSE 0 END) AS August,
        SUM(CASE WHEN MONTH(r.ReservationDate) = 9 THEN r.Quantity ELSE 0 END) AS September,
        SUM(CASE WHEN MONTH(r.ReservationDate) = 10 THEN r.Quantity ELSE 0 END) AS October,
        SUM(CASE WHEN MONTH(r.ReservationDate) = 11 THEN r.Quantity ELSE 0 END) AS November,
        SUM(CASE WHEN MONTH(r.ReservationDate) = 12 THEN r.Quantity ELSE 0 END) AS December
    FROM 
        Publishers p
	INNER JOIN Books b on b.PublisherId = p.Id
    INNER JOIN Reservations r ON r.BookId = b.Id
    WHERE 
        YEAR(r.ReservationDate) = @Year
    GROUP BY 
        p.Name
    '

    EXEC sp_executesql @SQL,
        N'@Year INT',
        @Year
END

EXEC GetPublisherAnnualReport
    @Year = '2024'


-- genres --
CREATE OR ALTER PROCEDURE GetGenreAnnualReport
    @Year INT
AS
BEGIN
    DECLARE @SQL NVARCHAR(MAX)
    
    SET @SQL = N'
    SELECT 
        g.Name,
        SUM(CASE WHEN MONTH(r.ReservationDate) = 1 THEN r.Quantity ELSE 0 END) AS January,
        SUM(CASE WHEN MONTH(r.ReservationDate) = 2 THEN r.Quantity ELSE 0 END) AS February,
        SUM(CASE WHEN MONTH(r.ReservationDate) = 3 THEN r.Quantity ELSE 0 END) AS March,
        SUM(CASE WHEN MONTH(r.ReservationDate) = 4 THEN r.Quantity ELSE 0 END) AS April,
        SUM(CASE WHEN MONTH(r.ReservationDate) = 5 THEN r.Quantity ELSE 0 END) AS May,
        SUM(CASE WHEN MONTH(r.ReservationDate) = 6 THEN r.Quantity ELSE 0 END) AS June,
        SUM(CASE WHEN MONTH(r.ReservationDate) = 7 THEN r.Quantity ELSE 0 END) AS July,
        SUM(CASE WHEN MONTH(r.ReservationDate) = 8 THEN r.Quantity ELSE 0 END) AS August,
        SUM(CASE WHEN MONTH(r.ReservationDate) = 9 THEN r.Quantity ELSE 0 END) AS September,
        SUM(CASE WHEN MONTH(r.ReservationDate) = 10 THEN r.Quantity ELSE 0 END) AS October,
        SUM(CASE WHEN MONTH(r.ReservationDate) = 11 THEN r.Quantity ELSE 0 END) AS November,
        SUM(CASE WHEN MONTH(r.ReservationDate) = 12 THEN r.Quantity ELSE 0 END) AS December
    FROM 
        Genres g
	INNER JOIN OriginalBookGenre obg on obg.GenreId = g.Id
	INNER JOIN OriginalBooks ob on ob.Id = obg.OriginalBookId
	INNER JOIN Books b on b.OriginalBookId = ob.Id
    INNER JOIN Reservations r ON r.BookId = b.Id
    WHERE 
        YEAR(r.ReservationDate) = @Year
    GROUP BY 
        g.Name
    '

    EXEC sp_executesql @SQL,
        N'@Year INT',
        @Year
END

EXEC GetGenreAnnualReport
    @Year = '2024'


-- original books --
CREATE OR ALTER PROCEDURE GetOriginalBookAnnualReport
    @Year INT
AS
BEGIN
    DECLARE @SQL NVARCHAR(MAX)
    
    SET @SQL = N'
    SELECT 
        ob.Title,
        SUM(CASE WHEN MONTH(r.ReservationDate) = 1 THEN r.Quantity ELSE 0 END) AS January,
        SUM(CASE WHEN MONTH(r.ReservationDate) = 2 THEN r.Quantity ELSE 0 END) AS February,
        SUM(CASE WHEN MONTH(r.ReservationDate) = 3 THEN r.Quantity ELSE 0 END) AS March,
        SUM(CASE WHEN MONTH(r.ReservationDate) = 4 THEN r.Quantity ELSE 0 END) AS April,
        SUM(CASE WHEN MONTH(r.ReservationDate) = 5 THEN r.Quantity ELSE 0 END) AS May,
        SUM(CASE WHEN MONTH(r.ReservationDate) = 6 THEN r.Quantity ELSE 0 END) AS June,
        SUM(CASE WHEN MONTH(r.ReservationDate) = 7 THEN r.Quantity ELSE 0 END) AS July,
        SUM(CASE WHEN MONTH(r.ReservationDate) = 8 THEN r.Quantity ELSE 0 END) AS August,
        SUM(CASE WHEN MONTH(r.ReservationDate) = 9 THEN r.Quantity ELSE 0 END) AS September,
        SUM(CASE WHEN MONTH(r.ReservationDate) = 10 THEN r.Quantity ELSE 0 END) AS October,
        SUM(CASE WHEN MONTH(r.ReservationDate) = 11 THEN r.Quantity ELSE 0 END) AS November,
        SUM(CASE WHEN MONTH(r.ReservationDate) = 12 THEN r.Quantity ELSE 0 END) AS December
    FROM 
        OriginalBooks ob
	INNER JOIN Books b on b.OriginalBookId = ob.Id
    INNER JOIN Reservations r ON r.BookId = b.Id
    WHERE 
        YEAR(r.ReservationDate) = @Year
    GROUP BY 
        ob.Title
    '

    EXEC sp_executesql @SQL,
        N'@Year INT',
        @Year
END

EXEC GetOriginalBookAnnualReport
    @Year = '2024'


-- customers --
CREATE OR ALTER PROCEDURE GetCustomerAnnualReport
    @Year INT
AS
BEGIN
    DECLARE @SQL NVARCHAR(MAX)
    
    SET @SQL = N'
    SELECT 
        c.Id + '' - '' + c.Name + '' '' + c.Surname AS Customer,
        SUM(CASE WHEN MONTH(r.ReservationDate) = 1 THEN r.Quantity ELSE 0 END) AS January,
        SUM(CASE WHEN MONTH(r.ReservationDate) = 2 THEN r.Quantity ELSE 0 END) AS February,
        SUM(CASE WHEN MONTH(r.ReservationDate) = 3 THEN r.Quantity ELSE 0 END) AS March,
        SUM(CASE WHEN MONTH(r.ReservationDate) = 4 THEN r.Quantity ELSE 0 END) AS April,
        SUM(CASE WHEN MONTH(r.ReservationDate) = 5 THEN r.Quantity ELSE 0 END) AS May,
        SUM(CASE WHEN MONTH(r.ReservationDate) = 6 THEN r.Quantity ELSE 0 END) AS June,
        SUM(CASE WHEN MONTH(r.ReservationDate) = 7 THEN r.Quantity ELSE 0 END) AS July,
        SUM(CASE WHEN MONTH(r.ReservationDate) = 8 THEN r.Quantity ELSE 0 END) AS August,
        SUM(CASE WHEN MONTH(r.ReservationDate) = 9 THEN r.Quantity ELSE 0 END) AS September,
        SUM(CASE WHEN MONTH(r.ReservationDate) = 10 THEN r.Quantity ELSE 0 END) AS October,
        SUM(CASE WHEN MONTH(r.ReservationDate) = 11 THEN r.Quantity ELSE 0 END) AS November,
        SUM(CASE WHEN MONTH(r.ReservationDate) = 12 THEN r.Quantity ELSE 0 END) AS December
    FROM 
        Customers c
    INNER JOIN Reservations r ON r.CustomerId = c.Id
    WHERE 
        YEAR(r.ReservationDate) = @Year
    GROUP BY 
        c.Id, c.Name, c.Surname
    '

    EXEC sp_executesql @SQL,
        N'@Year INT',
        @Year
END

EXEC GetCustomerAnnualReport
    @Year = '2024'


-- employees --
CREATE OR ALTER PROCEDURE GetEmployeeAnnualReport
    @Year INT
AS
BEGIN
    DECLARE @SQL NVARCHAR(MAX)
    
    SET @SQL = N'
    SELECT 
        e.Name + '' '' + e.Surname AS Employee,
        SUM(CASE WHEN MONTH(r.ReservationDate) = 1 THEN r.Quantity ELSE 0 END) AS January,
        SUM(CASE WHEN MONTH(r.ReservationDate) = 2 THEN r.Quantity ELSE 0 END) AS February,
        SUM(CASE WHEN MONTH(r.ReservationDate) = 3 THEN r.Quantity ELSE 0 END) AS March,
        SUM(CASE WHEN MONTH(r.ReservationDate) = 4 THEN r.Quantity ELSE 0 END) AS April,
        SUM(CASE WHEN MONTH(r.ReservationDate) = 5 THEN r.Quantity ELSE 0 END) AS May,
        SUM(CASE WHEN MONTH(r.ReservationDate) = 6 THEN r.Quantity ELSE 0 END) AS June,
        SUM(CASE WHEN MONTH(r.ReservationDate) = 7 THEN r.Quantity ELSE 0 END) AS July,
        SUM(CASE WHEN MONTH(r.ReservationDate) = 8 THEN r.Quantity ELSE 0 END) AS August,
        SUM(CASE WHEN MONTH(r.ReservationDate) = 9 THEN r.Quantity ELSE 0 END) AS September,
        SUM(CASE WHEN MONTH(r.ReservationDate) = 10 THEN r.Quantity ELSE 0 END) AS October,
        SUM(CASE WHEN MONTH(r.ReservationDate) = 11 THEN r.Quantity ELSE 0 END) AS November,
        SUM(CASE WHEN MONTH(r.ReservationDate) = 12 THEN r.Quantity ELSE 0 END) AS December
    FROM 
        AspNetUsers e
    INNER JOIN Reservations r ON r.EmployeeId = e.Id
    WHERE 
        YEAR(r.ReservationDate) = @Year
    GROUP BY 
        e.Id, e.Name, e.Surname
    '

    EXEC sp_executesql @SQL,
        N'@Year INT',
        @Year
END

EXEC GetEmployeeAnnualReport
    @Year = '2024'


-- books --
CREATE OR ALTER PROCEDURE GetBookAnnualReport
    @Year INT
AS
BEGIN
    DECLARE @SQL NVARCHAR(MAX)
    
    SET @SQL = N'
    SELECT 
        b.ISBN + '' - '' + ob.Title AS Book,
        SUM(CASE WHEN MONTH(r.ReservationDate) = 1 THEN r.Quantity ELSE 0 END) AS January,
        SUM(CASE WHEN MONTH(r.ReservationDate) = 2 THEN r.Quantity ELSE 0 END) AS February,
        SUM(CASE WHEN MONTH(r.ReservationDate) = 3 THEN r.Quantity ELSE 0 END) AS March,
        SUM(CASE WHEN MONTH(r.ReservationDate) = 4 THEN r.Quantity ELSE 0 END) AS April,
        SUM(CASE WHEN MONTH(r.ReservationDate) = 5 THEN r.Quantity ELSE 0 END) AS May,
        SUM(CASE WHEN MONTH(r.ReservationDate) = 6 THEN r.Quantity ELSE 0 END) AS June,
        SUM(CASE WHEN MONTH(r.ReservationDate) = 7 THEN r.Quantity ELSE 0 END) AS July,
        SUM(CASE WHEN MONTH(r.ReservationDate) = 8 THEN r.Quantity ELSE 0 END) AS August,
        SUM(CASE WHEN MONTH(r.ReservationDate) = 9 THEN r.Quantity ELSE 0 END) AS September,
        SUM(CASE WHEN MONTH(r.ReservationDate) = 10 THEN r.Quantity ELSE 0 END) AS October,
        SUM(CASE WHEN MONTH(r.ReservationDate) = 11 THEN r.Quantity ELSE 0 END) AS November,
        SUM(CASE WHEN MONTH(r.ReservationDate) = 12 THEN r.Quantity ELSE 0 END) AS December
    FROM 
        Books b
	INNER JOIN OriginalBooks ob on ob.Id = b.OriginalBookId
    INNER JOIN Reservations r ON r.BookId = b.Id
    WHERE 
        YEAR(r.ReservationDate) = @Year
    GROUP BY 
        b.Id, b.ISBN, ob.Title
    '

    EXEC sp_executesql @SQL,
        N'@Year INT',
        @Year
END

EXEC GetBookAnnualReport
    @Year = '2024'


CREATE OR ALTER PROCEDURE GetAnnualReport
    @Model NVARCHAR(50),
    @Year INT
AS
BEGIN
    IF @Model = 'Author'
        EXEC GetAuthorAnnualReport @Year
    ELSE IF @Model = 'Publisher'
        EXEC GetPublisherAnnualReport @Year
    ELSE IF @Model = 'Genre'
        EXEC GetGenreAnnualReport @Year
    ELSE IF @Model = 'OriginalBook'
        EXEC GetOriginalBookAnnualReport @Year
    ELSE IF @Model = 'Customer'
        EXEC GetCustomerAnnualReport @Year
	ELSE IF @Model = 'Employee'
		Exec GetEmployeeAnnualReport @Year
    ELSE IF @Model = 'Book'
        EXEC GetBookAnnualReport @Year
    ELSE
        RAISERROR('Invalid ReportType parameter.', 16, 1)
END

EXEC GetAnnualReport
    @Model = 'Employee',
    @Year = '2024'