-- =============================================
-- Author:		Kanth
-- Create date: 07/03/2019
-- Description:	Download Funds NAV History Data
-- =============================================
CREATE PROCEDURE UpdateFundNAVHistory
@xml xml
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	declare @table table (
			schemaCode int,
			nav decimal(10,4),
			fundtype int,
			date datetime
		)

	insert into @table (schemaCode, nav, fundtype, date)
	select
			'code' = x.v.value('code[1]', 'int'),
			'nav' = x.v.value('nav[1]', 'decimal(10, 4)'),
			'type' = x.v.value('type[1]', 'int'),
			'date' = x.v.value('date[1]', 'datetime')
			from @xml.nodes('/root/fund') x(v)
	
	update h set h.NAV = t.nav, LastUpdateDateTime = GETDATE()
		from @table t
		inner join FundsNav_History h on h.SchemaCode = t.schemaCode and h.[Date] = t.date

	insert into FundsNav_History(SchemaCode, NAV, [Date]) --, ISINGrowth, ISIN_DIV_Reinvestment, SchemaName, RepurchasePrice, SalePrice)
	select t.schemaCode, t.nav, t.date--, null, null, null, null, null
	from @table t
	left join FundsNav_History h on t.schemaCode = h.SchemaCode and t.date = h.[Date]
	where h.SchemaCode is null



END