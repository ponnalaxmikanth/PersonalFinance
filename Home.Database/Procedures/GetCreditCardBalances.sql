CREATE PROCEDURE GetCreditCardBalances
	--exec GetCreditCardBalances
AS
-- =============================================
-- Author:		Kanth
-- Create date: 9/14/2019
-- Description:	Get Credit Card Current Balances
-- =============================================

BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SELECT a.[DisplayName], h.AccountId, h.[Description], h.[Debit], h.[Credit], h.[Total]
	 FROM
	HomeTransactions h 
	INNER JOIN
	(
		SELECT h.AccountId, MAX(PostedDate) PostedDate
		  FROM HomeTransactions h
			INNER JOIN Accounts a on a.AccountId = h.AccountId
			WHERE a.AccountTypeId = 2 and h.[Group]= 'Credit Card' and h.[SubGroup] ='Payment'
			  GROUP BY h.AccountId
	) as max_date on h.PostedDate = max_date.PostedDate and h.AccountId = max_date.AccountId
	INNER JOIN Accounts a on a.AccountId = h.AccountId

END
GO
