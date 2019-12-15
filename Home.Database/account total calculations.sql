declare @TransactionId int, @RowNumber int, @PostedDate date, @AccountId int, @Debit money, @Credit money, @total money
DECLARE account_transactions CURSOR for
select h.TransactionId, h.RowNumber, h.PostedDate, h.AccountId, h.Debit, h.Credit 
from HomeTransactions h
inner join Accounts a on h.AccountId = a.AccountId
inner join AccountTypes t on t.AccountTypeId = a.AccountTypeId
where t.AccountTypeId = 4 and h.AccountId = 11
order by h.RowNumber

open account_transactions
DECLARE @done bit = 0

--WHILE (@done = 0)  
--BEGIN 
--  -- Get the next author.  
--  FETCH NEXT FROM account_transactions INTO @TransactionId, @RowNumber, @PostedDate, @AccountId, @Debit, @Credit;
--  select 1, @TransactionId, @RowNumber, @PostedDate, @AccountId, @Debit, @Credit, @total
--  IF (@@FETCH_STATUS <> 0) 
--  BEGIN 
--    SET @done = 1 
--    CONTINUE 
--  END 
--END
FETCH NEXT FROM account_transactions INTO @TransactionId, @RowNumber, @PostedDate, @AccountId, @Debit, @Credit;
set @total = @Credit - @Debit
select 1, @TransactionId, @RowNumber, @PostedDate, @AccountId, @Debit, @Credit, @total
WHILE @@FETCH_STATUS = 0
BEGIN
    FETCH NEXT FROM account_transactions INTO @TransactionId, @RowNumber, @PostedDate, @AccountId, @Debit, @Credit;
	set @total = @total + @Credit - @Debit
	select 2, @TransactionId, @RowNumber, @PostedDate, @AccountId, @Debit, @Credit, @total
END;

CLOSE account_transactions
DEALLOCATE account_transactions