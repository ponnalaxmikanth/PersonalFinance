-- =============================================
-- Author:		Kanth
-- Create date: 10/31/2019
-- Description:	Get Investment performance
-- =============================================
CREATE PROCEDURE GetInvestmentPerformance
	@portfolioId int
	--exec GetInvestmentPerformance @portfolioId = 1
AS
BEGIN
	
	SET NOCOUNT ON;

	select * 
	from mf_daily_tracker where portfolioId = @portfolioId and period = -1
	order by date
END