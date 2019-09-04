CREATE PROCEDURE [dbo].[calculate_mf_daily_value]
	-- Add the parameters for the stored procedure here
	@date date = '8/1/2019', @portfolioid int = null --@days int = 10
	--exec calculate_mf_daily_value @date = '8/3/2019', @portfolioid = 1

	-- =============================================
	-- Author:		Kanth
	-- Create date: 7/31/2019
	-- Description:	calculate daily mf value
	-- =============================================
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	declare @futuredate date = DATEADD(year, 1, GETDATE()), @today date = getdate()

	declare @table table (
		portfolioid int,
		schemacode int,
		purchaseDate date,
		sellDate date,
		purchasenav [decimal](10, 4),
		actualnav [decimal](10, 4),
		nav [decimal](10, 4),
		navdate date,
		units [decimal](10, 4),
		dividendpernav [decimal](10, 4)
	)


	declare @result table (
		portfolioId int,
		[date] date,
		period int,
		investment money,
		currentvalue money,
		profit money
	)

	insert into @table (portfolioid, schemacode, purchaseDate, sellDate, purchasenav, actualnav, units, dividendpernav, navdate)
	select r.PortfolioId, r.SchemaCode, r.PurchaseDate, r.SellDate, r.PurchaseNAV, r.ActualNAV, r.Units, r.DividendPerNAV, @date from
	(
		select p.PortfolioId, f.SchemaCode, p.PurchaseDate, DATEADD(year, 1, GETDATE()) SellDate, p.PurchaseNAV, p.ActualNAV, p.Units, p.DividendPerNAV
			from MF_Purchases p
			inner join MF_Funds f on p.FundId = f.FundId
		where p.PortfolioId = IIF(@portfolioid IS NULL, PortfolioId, @portfolioid)
		union all
		select r.PortfolioId, f.SchemaCode, r.PurchaseDate, r.SellDate, r.PurchaseNAV, r.ActualNAV, r.SellUnits, r.DividendPerNAV
			from MF_Redeems r
			inner join MF_Funds f on r.FundId = f.FundId
		where r.PortfolioId = IIF(@portfolioid IS NULL, PortfolioId, @portfolioid)
	) r
	order by r.PurchaseDate desc

	--select @date , DATEADD(day, -@days, GETDATE())
	--while(@date >= DATEADD(day, -@days, GETDATE()))
	--begin
	--	update @table set navdate = null
	--	update @table set navdate = @date
		
		if(@today = @date)
		begin
			UPDATE t SET t.nav = n.NAV
			FROM @table AS t
			INNER JOIN FundsNav AS n ON n.SchemaCode = t.schemacode --and n.[Date] = t.navdate 
		end
		else
		begin
			select 'update from backup table', @today today, @date [date]
		end

		insert into @result(portfolioId, period, [date], investment, currentvalue)
		select t.portfolioid, -1, t.navdate, sum(t.actualnav * t.units), sum(t.units * t.nav)--, t.purchaseDate, t.sellDate
			from @table t
		where t.sellDate > @date and t.purchaseDate < @date and t.nav is not null --and t.schemacode = 100915 and t.portfolioid = 1
		group by t.portfolioid, t.navdate

		insert into @result(portfolioId, period, [date], investment, currentvalue)
		select t.portfolioid, 1, t.navdate, sum(t.actualnav * t.units), sum(t.units * t.nav)
			from @table t
		where t.sellDate > @date and t.purchaseDate < @date and t.purchaseDate > DATEADD(mm, -1, @date) and t.nav is not null
		group by t.portfolioid, t.navdate

		insert into @result(portfolioId, period, [date], investment, currentvalue)
		select t.portfolioid, 3, t.navdate, sum(t.actualnav * t.units), sum(t.units * t.nav)
			from @table t
		where t.sellDate > @date and t.purchaseDate < @date and t.purchaseDate > DATEADD(mm, -3, @date) and t.nav is not null
		group by t.portfolioid, t.navdate

		insert into @result(portfolioId, period, [date], investment, currentvalue)
		select t.portfolioid, 12, t.navdate, sum(t.actualnav * t.units), sum(t.units * t.nav)
			from @table t
		where t.sellDate > @date and t.purchaseDate < @date and t.purchaseDate > DATEADD(yy, -1, @date) and t.nav is not null
		group by t.portfolioid, t.navdate







		insert into @result(portfolioId, period, [date], investment, currentvalue)
		select -1, -1, t.navdate, sum(t.actualnav * t.units), sum(t.units * t.nav)--, t.purchaseDate, t.sellDate
			from @table t
		where t.sellDate > @date and t.purchaseDate < @date and t.nav is not null --and t.schemacode = 100915 and t.portfolioid = 1
		group by t.navdate

		insert into @result(portfolioId, period, [date], investment, currentvalue)
		select -1, 1, t.navdate, sum(t.actualnav * t.units), sum(t.units * t.nav)
			from @table t
		where t.sellDate > @date and t.purchaseDate < @date and t.purchaseDate > DATEADD(mm, -1, @date) and t.nav is not null
		group by t.navdate

		insert into @result(portfolioId, period, [date], investment, currentvalue)
		select -1, 3, t.navdate, sum(t.actualnav * t.units), sum(t.units * t.nav)
			from @table t
		where t.sellDate > @date and t.purchaseDate < @date and t.purchaseDate > DATEADD(mm, -3, @date) and t.nav is not null
		group by t.navdate

		insert into @result(portfolioId, period, [date], investment, currentvalue)
		select -1, 12, t.navdate, sum(t.actualnav * t.units), sum(t.units * t.nav)
			from @table t
		where t.sellDate > @date and t.purchaseDate < @date and t.purchaseDate > DATEADD(yy, -1, @date) and t.nav is not null
		group by t.navdate



		set @date = DATEADD(day, -1, @date)
	--end

	
	update @result set profit = currentvalue - investment

	select * from @result

END
