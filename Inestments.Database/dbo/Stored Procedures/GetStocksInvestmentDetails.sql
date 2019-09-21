-- =============================================
-- Author:		Laxmikanth
-- Create date: 9/21/2019
-- Description:	Get Stocks Current & Overall Investment Details 
-- =============================================
CREATE PROCEDURE GetStocksInvestmentDetails
	--exec GetStocksInvestmentDetails
AS
BEGIN
	select SUM(r.Investment) Investment, SUM(r.CurrentValue) CurrentValue, 'C' Type from
	(
		select (s.Quantity * s.Price) - s.Dividend Investment, (s.Quantity * s.MarketPrice) - s.Dividend CurrentValue
			from Stocks s
	) r
END