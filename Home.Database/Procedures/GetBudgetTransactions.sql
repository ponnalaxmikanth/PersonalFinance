-- =============================================
-- Author:		Kanth
-- Create date: 6/29/2019
-- Description:	Get Home Transactions
-- =============================================
CREATE PROCEDURE [dbo].[GetBudgetTransactions]
	-- Add the parameters for the stored procedure here
	@fromDate date, @toDate date
	--exec GetBudgetTransactions '06/01/2019', '06/30/2019'
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
		select h.[Group], h.SubGroup, sum(h.Debit) debit, sum(h.Credit) credit
		from HomeTransactions h 
		where h.TransactionDate between @fromDate and @toDate
			and h.[Group] <> 'Credit Card' and h.SubGroup <> 'Payment'
			and h.[Group] <> 'Car'
		group by h.[Group], h.SubGroup
		Union all
		select h.[Group], 'Car' SubGroup, sum(h.Debit) debit, sum(h.Credit) credit
			from HomeTransactions h 
			where h.TransactionDate between @fromDate and @toDate
				and h.[Group] = 'Car'
		group by h.[Group]
		order by h.[Group], h.SubGroup

END
