CREATE PROCEDURE [dbo].[CalculateAccountTotal] @acocuntId int
--exec CalculateAccountTotal @acocuntId = 11
AS
-- =============================================
-- Author:		Kanth
-- Create date: 09/29/2019
-- Description:	Calculate Account total
-- =============================================

BEGIN
	declare @rowNumber int, @TransactionId int, @PostedDate date, @AccountId int, @Credit money, @Debit money, @Total money, @new_total money = 0--, @count int = 0

	DECLARE hcurr CURSOR FOR 
		select RowNumber, TransactionId, PostedDate, AccountId, Credit, Debit, Total 
			from HomeTransactions 
				where AccountId = @acocuntId
			order by RowNumber;

	open hcurr

	FETCH NEXT FROM hcurr INTO @rowNumber, @TransactionId, @PostedDate, @AccountId, @Credit, @Debit, @Total

	WHILE @@FETCH_STATUS = 0
		BEGIN
			set @new_total = @new_total + @Credit - @Debit
			update HomeTransactions set Total = @new_total where TransactionId = @TransactionId and RowNumber = @rowNumber and PostedDate = @PostedDate
			FETCH NEXT FROM hcurr INTO @rowNumber, @TransactionId, @PostedDate, @AccountId, @Credit, @Debit, @Total
		END;

	close hcurr
	DEALLOCATE hcurr

END