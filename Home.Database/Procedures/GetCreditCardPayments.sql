-- =============================================
-- Author:		Kanth
-- Create date: 9/7/2019
-- Description:	Get Credit Card Payments by Date Range
-- =============================================
CREATE PROCEDURE GetCreditCardPayments
	@fromDate date, @toDate date
	--exec GetCreditCardPayments @fromDate = '2019-09-01', @toDate = '2019-09-30'
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    select h.AccountId, h.TransactionDate, h.PostedDate, h.[Description], h.Debit, h.Credit, h.Total, h.[Group], h.[SubGroup], h.TransactedBy, h.Comments
	, a.DisplayName, at.AccountType, at.AccountTypeId
	from HomeTransactions h
	inner join Accounts a on a.AccountId = h.AccountId
	inner join AccountTypes at on at.AccountTypeId = a.AccountTypeId
		where at.AccountTypeId in (1, 3) and h.TransactionDate between @fromDate and @toDate and h.[Group] = 'Credit Card'
	order by h.[Description]
END
GO
