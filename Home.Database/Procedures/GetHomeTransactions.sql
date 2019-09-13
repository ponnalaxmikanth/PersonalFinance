-- =============================================
-- Author:		Kanth
-- Create date: 12/14/2018
-- Description:	Get Home transactions
-- =============================================
CREATE PROCEDURE [dbo].[GetHomeTransactions]
	-- Add the parameters for the stored procedure here
	@fromDate date, @toDate date, @accountId int
	--exec GetHomeTransactions '2018-12-01', '2018-12-31', 1
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	--select a.AccountId, a.Name, a.DisplayName, a.displayOrder, a.Limit
	--, t.AccountTypeId, t.AccountType
	--, h.TransactionId, h.TransactionDate, h.Item, h.Amount, h.Store, h.TransactedBy
	--, g.GroupId, g.GroupName, sg.Id SubGroupId, sg.SubGroupName
	--from HomeTransactions h
	--inner join Accounts a on h.AccountId = a.AccountId
	--inner join AccountTypes t on a.AccountTypeId = t.AccountTypeId
	--inner join ExpenseGroups g on h.Group = g.gr
	--inner join ExpensesSubGroup sg on sg.Id = h.SubGroupId
	--where a.AccountId = @accountId and h.TransactionDate between @fromDate and @toDate
	--order by a.displayOrder, h.TransactionDate

	select a.AccountId, a.Name, a.DisplayName, a.displayOrder, a.Limit
	, t.AccountTypeId, t.AccountType
	 , h.TransactionId, h.TransactionDate, '' Item, h.Debit, h.Credit, h.Store, h.Total Balance, h.TransactedBy
	, 0 GroupId, h.[Group] GroupName, 0 SubGroupId, h.SubGroup SubGroupName
	from HomeTransactions h
	inner join Accounts a on h.AccountId = a.AccountId
	inner join AccountTypes t on a.AccountTypeId = t.AccountTypeId
	left join ExpenseGroups g on h.[Group] = g.GroupName
	left join ExpensesSubGroup sg on sg.SubGroupName = h.SubGroup
	where a.AccountId = @accountId and h.TransactionDate between @fromDate and @toDate
	order by a.displayOrder, 	h.TransactionDate

END
