CREATE PROCEDURE [dbo].[GetBudgetTransactions]
	-- Add the parameters for the stored procedure here
	@fromDate date, @toDate date
	--exec GetBudgetTransactions @fromDate = '06/01/2019', @toDate = '06/30/2019'
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
	
	insert into @result([Group], SubGroup, debit, credit, [level])
	select h.[Group], h.SubGroup, sum(h.Debit) debit, sum(h.Credit) credit, 0
	from HomeTransactions h 
	where h.PostedDate between @fromDate and @toDate
		and h.[Group] <> 'Credit Card' and h.SubGroup <> 'Payment' and h.SubGroup <> 'Transfer' and h.[Group] <> 'Friends'
		and h.[Group] <> 'Car'
		and h.[SubGroup] <> 'Withdraw'
	group by h.[Group], h.SubGroup
	Union all
	select h.[Group], 'Car' SubGroup, sum(h.Debit) debit, sum(h.Credit) credit, 0
		from HomeTransactions h 
		where h.PostedDate between @fromDate and @toDate
			and h.[Group] = 'Car'
	group by h.[Group]
	order by h.[Group], h.SubGroup


	update r set r.Budget = b.Amount
	from @result r
	inner join (select b.[Group], SUM(b.Amount) Amount from
	Budget b 
		where b.FromDate >= @fromDate and b.ToDate <= @toDate
	group by b.[Group]) b  on r.SubGroup = b.[Group]

	--update r set r.Budget = SUM(b.Amount)
	--(select b.[Group], SUM(b.Amount) Amount from
	--Budget b 
	--	where b.FromDate >= @fromDate and b.ToDate <= @toDate
	--group by b.[Group]) b
	--JOIN @result r on r.SubGroup = b.[Group]

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
	select * from @result order by [level], [Group]

END