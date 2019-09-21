-- =============================================
-- Author:		Kanth
-- Create date: Insert transactions
-- Description:	inert account transactions
-- =============================================
CREATE PROCEDURE [dbo].[Insert_Account_Transactions]
	@xml as xml, @minDate date, @accountId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	declare @table table (
		acctNo  varchar(100),
		rowNum int,
		--date datetime,
		--transactdate datetime,
		posteddate datetime,
		description varchar(100),
		debit decimal(10,4),
		credit decimal(10,4),
		total decimal(10,4),
		transactby nvarchar(50),
		grp nvarchar(100),
		subgroup nvarchar(100),
		comments varchar(100)
	)

	insert into @table (acctNo, rowNum, posteddate, description, debit, credit, total, transactby, grp, subgroup, comments)
	select distinct 
	'accountnumber' = x.v.value('acctNo[1]', 'int'),
	'rowNum' = x.v.value('rowNum[1]', 'int'),
	--'transactdate' = x.v.value('transactdate[1]', 'datetime'),
	'posteddate' = x.v.value('posteddate[1]', 'datetime'),
	'description' = x.v.value('description[1]', 'varchar(100)'),
	'debit' = x.v.value('debit[1]', 'decimal(18,4)'),
	'credit' = x.v.value('credit[1]', 'decimal(18,4)'),
	'total' = x.v.value('total[1]', 'decimal(18,4)'),
	'transactby' = x.v.value('transactby[1]', 'nvarchar(50)'),
	'group' = x.v.value('group[1]', 'nvarchar(100)'),
	'subgroup' = x.v.value('subgroup[1]', 'nvarchar(100)'),
	'comments' = x.v.value('comments[1]', 'nvarchar(100)')
	from @xml.nodes('/root/row') x(v)

	--update @table set transactdate = posteddate where posteddate is null
	select * from @table


	delete from HomeTransactions where AccountId = @accountId and PostedDate >= @minDate

	insert into HomeTransactions (RowNumber, PostedDate, [Group], SubGroup, Description, Debit, Credit, Total, AccountId, TransactedBy, Store, Comments, CreatedDate, LastModifiedDate)
	select tbl.rowNum, tbl.posteddate, tbl.grp, tbl.subgroup, tbl.description, tbl.debit, tbl.credit, tbl.total, tbl.acctNo, tbl.transactby, '', tbl.comments, GETDATE(), GETDATE()
	from HomeTransactions t
	 right JOIN @table tbl ON tbl.acctNo = t.AccountId and tbl.posteddate = t.PostedDate and tbl.description = t.Description and tbl.debit = t.Debit and tbl.credit = t.Credit
	 where t.AccountId is null
	 order by tbl.rowNum

    END