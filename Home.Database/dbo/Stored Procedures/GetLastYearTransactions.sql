CREATE PROCEDURE [dbo].[GetLastYearTransactions]
	--exec GetLastYearTransactions
AS
BEGIN
-- =============================================
-- Author:		Kanth
-- Create date: 09/29/2019
-- Description:	
-- =============================================
	DECLARE @Today DATETIME, @nMonths TINYINT, @fromDate date, @toDate date
	SET @nMonths = 11
	--SET @Today = DATEADD(month, (-1) * @nMonths, GETDATE())
	SET @Today = GETDATE()

	declare @dates table (
		dates date,
		fromDate date,
		toDate date
	)

	declare @result table (
			fromDate date,
			toDate date,
			[Group] nvarchar(100),
			SubGroup nvarchar(100),
			debit decimal(18,4), 
			credit decimal(18,4),
			[Budget] money default(0),
			[Balance] money default(0),
			[level] int
		)

	--;WITH q AS
	--(
	--	SELECT  @Today AS datum
	--	UNION ALL
	--	SELECT  DATEADD(month, 1, datum) 
	--	FROM q WHERE datum + 1 < GETDATE()
	--)
	--insert into @dates(dates)
	--SELECT datum --SUBSTRING(DATENAME(MONTH, datum), 1, 3) + CAST(YEAR(datum) AS VARCHAR(4))
	--FROM q

	insert into @dates(dates)
	SELECT	DATEADD(MM, -1*number, @Today) --RIGHT(CONVERT(VARCHAR,DATEADD(MM, -1*number, @Today),106),8)
	FROM	master.dbo.spt_values
	WHERE TYPE = 'P' and number between 0 and @nMonths

	update @dates set fromDate = DATEADD(mm, DATEDIFF(mm, 0, dates), 0),  toDate = DATEADD(s,-1,DATEADD(mm, DATEDIFF(m,0,dates)+1,0))

	--select * from @dates

	DECLARE contacts_cursor CURSOR FOR
	SELECT fromDate, toDate
	FROM @dates;

	OPEN contacts_cursor;
	FETCH NEXT FROM contacts_cursor INTO @fromDate, @toDate;

	WHILE @@FETCH_STATUS = 0
	   BEGIN
			--select  @fromDate, @toDate;
			insert into @result
		  exec GetBudgetTransactions @fromDate = @fromDate, @toDate = @toDate
		  FETCH NEXT FROM contacts_cursor INTO @fromDate, @toDate;
	   END;

	CLOSE contacts_cursor;
	DEALLOCATE contacts_cursor;

	--delete from @result where level = 1
	--select fromDate, toDate, SUM(debit) debit, SUM(credit) credit, sum(Budget) Budget from @result group by fromDate, toDate
	select * from @result where level = 1
	order by fromDate


END