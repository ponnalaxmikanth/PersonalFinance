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
			case when h.[Group] = 'Car' then 'Home' 
				 when h.[Group] ='Home' and h.[SubGroup] = 'Rewards' then 'Home' 
			else h.[Group] end AS [Group],
			case when h.[Group] = 'Car' then 'Transport' 
				 when h.[Group] ='Home' and h.[SubGroup] = 'Rewards' then 'Income' 
			else h.SubGroup end AS SubGroup
		from HomeTransactions h
		where h.PostedDate between @fromDate and @toDate
				and h.[Group] <> 'Credit Card' and h.SubGroup <> 'Payment' and h.SubGroup <> 'Transfer' 
				and h.[SubGroup] <> 'Balance Transfer'
				and h.[Group] <> 'Friends' and h.[SubGroup] <> 'Withdraw'
		union all
		select i.Debit, i.Credit, i.[Group], i.SubGroup 
		from HealthInsurance i
		where i.PostedDate between @fromDate and @toDate
	) h
	group by h.[Group], h.SubGroup

	update r set r.Budget = b.Amount
	from @result r
	inner join (select b.[Group], SUM(b.Amount) Amount from
	Budget b 
		where b.FromDate >= @fromDate and b.ToDate <= @toDate
	group by b.[Group]) b  on r.SubGroup = b.[Group]

	insert into @result([Group], SubGroup, debit, credit, [level], Budget)
	select 'Home', b.[Group], 0, 0, 0, sum(b.Amount)
		from Budget b
		left join @result r on b.[Group] = r.SubGroup
			where b.FromDate >= @fromDate and b.ToDate <= @toDate and r.SubGroup is null
		group by b.[Group]
	
	if exists (select top 1 * from @result)
	BEGIN
		insert into @result([Group], SubGroup, debit, credit, Budget, [level])
		select 'Total', 'Total', sum(debit), sum(credit), sum(Budget), 1 from @result
	END

	update @result set Balance = [Budget] - [debit], fromDate = @fromDate, toDate = @toDate
	update @result set [Group] ='Unknown', [SubGroup] ='Unknown' where [Group] = ''

	--select @income = SUM(credit) from @result where [SubGroup] = 'Income'
	
	--update @result set [Budget] = [Budget] - @income, [Balance] = [Balance] - @income where level = 1

	select * from @result order by [level], Budget desc

END