-- =============================================
-- Author:		Kanth
-- Create date: 10/18/2017
-- Description:	Dump Bench mark data
-- =============================================
CREATE PROCEDURE [dbo].[DumpBenchMarkData]
	@BenchMark nvarchar(100), @Date datetime, @OpenValue decimal(18,4), @HighValue decimal(18,4), @LowValue decimal(18,4), @CloseValue decimal(18,4)
	, @SharesTraded bigint, @TurnOver decimal(18,4)
AS
BEGIN
	
	SET NOCOUNT ON;

	declare @benchmarkId int
	declare @historyId int

	select @benchmarkId = BenchMarkId from BenchMarks where BenchMark = @BenchMark
	if(@benchmarkId is null)
	begin
		insert into BenchMarks (BenchMark)
		select @BenchMark

		select @benchmarkId = BenchMarkId from BenchMarks where BenchMark = @BenchMark
	end
	if(@benchmarkId is not null)
	begin
		select @historyId = id from BenchMark_History where BenchMarkId = @benchmarkId and Date = @Date
		if(@historyId is not null)
		begin
			update BenchMark_History set CloseValue = @CloseValue, HighValue = @HighValue, LowValue = @LowValue, OpenValue = @OpenValue, SharesTraded = @SharesTraded
										, TurnOver = @TurnOver, LastUpdateDate = GETDATE() where id = @historyId --BenchMarkId = @benchmarkId and Date = @Date
		end
		else
		begin
			insert into BenchMark_History (BenchMarkId, CloseValue, HighValue, LowValue, OpenValue, TurnOver, Date, SharesTraded)
			select @benchmarkId, @CloseValue, @HighValue, @LowValue, @OpenValue, @TurnOver, @Date, @SharesTraded
		end
	end
END
