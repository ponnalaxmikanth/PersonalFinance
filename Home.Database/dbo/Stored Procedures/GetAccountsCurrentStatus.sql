-- =============================================
-- Author:		Kanth
-- Create date: 9/21/2019
-- Description:	Get Accounts Current Status
-- =============================================
CREATE PROCEDURE [dbo].[GetAccountsCurrentStatus] @detail int = 0
	--exec GetAccountsCurrentStatus @detail = 1
AS
BEGIN
		declare @result table (
		AccountId int, 
		Debit money,
		Credit money,
		Total money, 
		DisplayName varchar(100),
		AccountType varchar(100)
	)

	insert into @result
	SELECT h.AccountId, h.Debit, h.Credit, h.Total, a.DisplayName, CASE WHEN t.AccountType = 'Credit' THEN 'Credit' ELSE 'Savings' END AS AcctType
		FROM (
		 SELECT h.*, RANK () OVER (PARTITION BY AccountId ORDER BY RowNumber DESC ) price_rank 
			FROM HomeTransactions h --where AccountId = 2
		) h
		inner join Accounts a on h.AccountId = a.AccountId
		inner join AccountTypes t on t.AccountTypeId = a.AccountTypeId
		WHERE h.price_rank = 1
		order by t.AccountType

	if(@detail = 1)
	BEGIN
		select * from @result
	END
	ELSE
	BEGIN
		select 0 AccountId, AccountType, SUM(Debit) Debit, SUM(Credit) Credit, SUM(Total) Total, ' ' DisplayName
			from @result 
		group by AccountType
	END

END