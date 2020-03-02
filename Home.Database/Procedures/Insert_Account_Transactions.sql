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
		posteddate datetime,
		description varchar(100),
		debit decimal(18,4),
		credit decimal(18,4),
		total decimal(18,4),
		transactby nvarchar(50),
		grp nvarchar(100),
		subgroup nvarchar(100),
		comments varchar(100)
	)

	insert into @table (acctNo, rowNum, posteddate, description, debit, credit, total, transactby, grp, subgroup, comments)
	select distinct 
	'accountnumber' = x.v.value('acctNo[1]', 'int'),
	'rowNum' = x.v.value('rowNum[1]', 'int'),
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

	-- update the records which exists in data base and changed
	update h set h.Description = t.description, h.Debit = t.debit, h.Credit = t.credit, h.Total = t.total, h.[Group] = t.grp
		, h.SubGroup = t.subgroup, h.TransactedBy = t.transactby, h.Comments = t.comments, h.PostedDate = t.posteddate, h.LastModifiedDate = GETDATE()
	from @table t
	inner join HomeTransactions h on h.AccountId = t.acctNo and h.RowNumber = t.rowNum

	insert into HomeTransactions (RowNumber, PostedDate, [Group], SubGroup, Description, Debit, Credit, Total, AccountId, TransactedBy, Store, Comments, CreatedDate
									,LastModifiedDate)
	select tbl.rowNum, tbl.posteddate, tbl.grp, tbl.subgroup, tbl.description, tbl.debit, tbl.credit, tbl.total, tbl.acctNo, tbl.transactby, '', tbl.comments, GETDATE()
								,GETDATE()
	from HomeTransactions t
	 right JOIN @table tbl ON tbl.acctNo = t.AccountId  and tbl.rowNum = t.RowNumber 
	 where t.AccountId is null
	 order by tbl.rowNum


		--delete t
		--	from HomeTransactions t
		--	LEFT JOIN @table tbl ON tbl.acctNo = t.AccountId  and tbl.rowNum = t.RowNumber 
		--where tbl.acctNo is null

		select * from @table

    END