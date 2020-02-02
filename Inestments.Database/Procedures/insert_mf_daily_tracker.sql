CREATE PROCEDURE [dbo].[insert_mf_daily_tracker]
	@portfolioId int, @trackdate date, @period int, @investValue decimal(18,4), @currentvalue decimal(18,4), @profit decimal(10,4)
AS
BEGIN
	-- =============================================
	-- Author:		Kanth
	-- Create date: 9/5/2018
	-- Description:	track daily mf transactionsvalue
	-- =============================================
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	if(select count(*) from mf_daily_tracker where portfolioId = @portfolioId and date = @trackdate and period = @period) > 0
	begin
		update mf_daily_tracker set investment = @investValue, currentvalue = @currentvalue, profit = @profit, lastupdatedate = GETDATE()
		where portfolioId = @portfolioId and date = @trackdate and period = @period
	end
	else
	begin
		insert into mf_daily_tracker (portfolioId ,date, period, investment, currentvalue, profit)
		values (@portfolioId, @trackdate, @period, @investValue, @currentvalue, @profit)
	end
END

