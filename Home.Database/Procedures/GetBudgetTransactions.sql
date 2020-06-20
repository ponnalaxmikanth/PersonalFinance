CREATE PROCEDURE [dbo].[GetBudgetTransactions]
	-- Add the parameters for the stored procedure here
	@fromDate date, @toDate date
	--exec GetBudgetTransactions @fromDate = '11/01/2019', @toDate = '11/30/2019'
AS
BEGIN

	SET NOCOUNT ON;

	-- =============================================
	-- Author:		Kanth
	-- Create date: 6/29/2019
	-- Description:	Get Home Transactions
	-- =============================================

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

	declare @income money
	
	insert into @result([Group], SubGroup, debit, credit, [level])
	select h.[Group], h.SubGroup, sum(h.Debit) debit, sum(h.Credit) credit, 0 AS [level]
	from (
		select h.Debit, h.Credit,
			case when h.[Group] in ('Transport', 'Income') then 'Home' 
				 when h.[Group] = 'India' and h.SubGroup = 'Ticket' then 'Home'
				 when h.[Group] = 'India' and h.SubGroup = 'Gifts' then 'Home'
			else h.[Group] end AS [Group],
			case when h.[Group] = 'Transport' then 'Transport' 
				 when h.[Group] ='Income' then 'Income'
				 when h.[Group] = 'India' and h.SubGroup = 'Ticket' then 'Transport'
				 when h.[Group] = 'India' and h.SubGroup = 'Gifts' then 'shopping'
				 when h.[Group] = 'Kanth' and h.SubGroup in('Hair Cut', 'Kanth') then 'Kanth'
			else h.SubGroup end AS SubGroup
		from HomeTransactions h
		where h.PostedDate between @fromDate and @toDate
				and (h.[Group] <> 'Account' and h.SubGroup <> 'Transfer') and (h.[Group] <> 'Credit Card' and h.SubGroup <> 'Payment')
				and (h.[Group] <> 'Credit Card' and h.SubGroup <> 'Reversal')
				and h.SubGroup not in ('Transfer' , 'Withdraw')
				and h.[Group] <> 'Friends'
		union all
		select i.Debit, i.Credit, i.[Group], i.SubGroup 
		from HealthInsurance i
		where i.PostedDate between @fromDate and @toDate
		union all
		select 0 Debit, i.Credit, 'Home', 'Income' 
		from HealthInsurance i
		where i.PostedDate between @fromDate and @toDate
	) h
	group by h.[Group], h.SubGroup

	--if not exists (select 1 from @result where [Group] = 'Home' and  SubGroup = 'Income')
	--BEGIN
	--	insert into @result([Group], SubGroup, debit, credit, [level])
	--	select 'Home', 'Income', 0, 0, 0
	--END

	update r set r.Budget = b.Amount
	from @result r
	inner join (
		select b.[Group], SUM(b.Amount) Amount 
			from Budget b 
		where b.FromDate >= @fromDate and b.ToDate <= @toDate
		group by b.[Group]
	) b on r.SubGroup = b.[Group]

	update r set r.debit = r.debit + b.Debit, r.credit = r.credit + b.Credit
	from @result r
	inner join (
		select 0  Debit, SUM(i.Debit) Credit, 'Home' [Group], 'Income' SubGroup 
		from HealthInsurance i
		where i.PostedDate between @fromDate and @toDate
		group by i.[Group], i.SubGroup 
	) b on r.[Group] = b.[Group] and r.SubGroup = b.SubGroup

	insert into @result([Group], SubGroup, debit, credit, [level], Budget)
	select 'Home', b.[Group], 0, 0, 0, sum(b.Amount)
		from Budget b
		left join @result r on b.[Group] = r.SubGroup
			where b.FromDate >= @fromDate and b.ToDate <= @toDate and r.SubGroup is null
		group by b.[Group]
	
	if exists (select top 1 * from @result)
	BEGIN
		insert into @result([Group], SubGroup, debit, credit, Budget, [level])
		select 'Total', 'Total', sum(debit), sum(credit), sum(Budget), 1 
		from @result
		where [Group] <> 'Credit Card' and SubGroup <> 'Payment'
	END

	update @result set Balance = [Budget] - [debit], fromDate = @fromDate, toDate = @toDate
	update @result set [Group] ='Unknown', [SubGroup] ='Unknown' where [Group] = ''
	update @result set [Group] ='Home', [SubGroup] ='Utilities' where [Group] = 'Utilities'
	update @result set [Group] ='Home', [SubGroup] ='Utilities' where [Group] = 'Home' and SubGroup = 'Electricity'

	select fromDate, toDate, [Group], SubGroup, SUM(debit) debit, SUM(credit) credit, sum(Budget) Budget, sum(Balance) Balance, [level]
	from @result
	group by fromDate, toDate, [Group], SubGroup, [level]
	order by [level], Budget desc

END