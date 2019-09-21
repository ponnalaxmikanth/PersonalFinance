-- =============================================
-- Author:		Kanth
-- Create date: 9/21/2019
-- Description:	Get Accounts Current Status
-- =============================================
CREATE PROCEDURE GetAccountsCurrentStatus
	--exec GetAccountsCurrentStatus
AS
BEGIN
	SELECT h.AccountId, h.Debit, h.Credit, h.Total, a.DisplayName, t.AccountType
	FROM (
	 SELECT h.*, RANK () OVER (PARTITION BY AccountId ORDER BY PostedDate DESC ) price_rank 
		FROM HomeTransactions h
	) h
	inner join Accounts a on h.AccountId = a.AccountId
	inner join AccountTypes t on t.AccountTypeId = a.AccountTypeId
	WHERE h.price_rank = 1
	order by t.AccountType
END