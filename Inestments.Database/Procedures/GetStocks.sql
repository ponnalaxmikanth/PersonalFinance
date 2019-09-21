CREATE PROCEDURE [dbo].[GetStocks] @details bit = 0
	--exec GetStocks @details = 0
AS
BEGIN
	-- =============================================
	-- Author:		Kanth
	-- Create date: 8/28/2019
	-- Description:	Get Stock details
	-- =============================================
	SET NOCOUNT ON;

	declare @result table (
		StockID nvarchar(50),
		PurchaseDate date,
		Quantity int,
		Price money,
		MarketPrice money,
		Dividend money, 
		Profit money
	)

	if(@details = 0)
	begin
		insert into @result (StockID, Quantity, Price, MarketPrice, Dividend, PurchaseDate)
		select StockID, SUM(Quantity) Quantity, AVG(Price) Price,AVG(MarketPrice) MarketPrice, SUM(Dividend) Dividend, GETDATE() PurchaseDate
			from Stocks
				group by StockID
	end
	else
	begin
		insert into @result (StockID, Quantity, Price, MarketPrice, Dividend, PurchaseDate)
		select StockID, Quantity, Price, MarketPrice, Dividend, PurchaseDate
			from Stocks
	end

	update @result set Profit = (MarketPrice*Quantity) - (Quantity*Price)

	select * from @result order by StockID, PurchaseDate

END
