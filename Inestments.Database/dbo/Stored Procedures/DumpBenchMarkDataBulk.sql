-- =============================================
-- Author:		Kanth
-- Create date: 01/26/2020
-- Description:	Download BSE/NSE sesex data
-- =============================================
CREATE PROCEDURE [dbo].[DumpBenchMarkDataBulk]
@benchmark varchar(50), @xmldata xml
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	declare @benchmarkId int

    -- Insert statements for procedure here
	declare @table table (
			[open]  decimal(10,4),
			high decimal(10,4),
			low  decimal(10,4),
			[close] decimal(10,4),
			[date] date,
			benchmarkid int
		)
	select @benchmarkId = BenchMarkId from BenchMarks where BenchMark = @benchmark
	if(@benchmarkId is null)
	begin
		insert into BenchMarks (BenchMark, CreatedDate, LastUpdatedDate)
		select @BenchMark, GETDATE(), GETDATE()

		select @benchmarkId = BenchMarkId from BenchMarks where BenchMark = @BenchMark
	end

	insert into @table (benchmarkid, [open], high, low, [close], [date])
	select @benchmarkId,
			'open' = x.v.value('open[1]', 'decimal(10,4)'),
			'high' = x.v.value('high[1]', 'decimal(10, 4)'),
			'low' = x.v.value('low[1]', 'decimal(10,4)'),
			'close' = x.v.value('close[1]', 'decimal(10,4)'),
			'date' = x.v.value('date[1]', 'date')
			from @xmldata.nodes('/root/data') x(v)
	
	insert into @table (benchmarkid, [open], high, low, [close], [date])
	select @benchmarkId,
			'open' = x.v.value('open[1]', 'decimal(10,4)'),
			'high' = x.v.value('high[1]', 'decimal(10, 4)'),
			'low' = x.v.value('low[1]', 'decimal(10,4)'),
			'close' = x.v.value('close[1]', 'decimal(10,4)'),
			'date' = x.v.value('date[1]', 'datetime')
			from @xmldata.nodes('/root/data') x(v)

			update bh set bh.OpenValue = t.[open], bh.[HighValue] = t.[high], bh.[LowValue] = t.[low], bh.[CloseValue] =t.[close], bh.LastUpdateDate = GETDATE()
			from @table t
			inner join BenchMark_History bh on t.benchmarkid = bh.BenchMarkId and t.date = bh.Date

			insert into BenchMark_History (BenchMarkId, CloseValue, HighValue, LowValue, OpenValue, Date, LastUpdateDate, CreateDate)
			select t.benchmarkid, t.[close], t.[high], t.[low], t.[open], t.[date], GETDATE(), GETDATE()
			from @table t
			left join BenchMark_History bh on t.benchmarkid = bh.BenchMarkId and t.date = bh.Date
			where t.benchmarkid is null

	select * from @table

END