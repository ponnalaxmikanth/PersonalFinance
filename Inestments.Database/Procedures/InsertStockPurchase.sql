CREATE PROCEDURE [dbo].[InsertStockPurchase]
	@StockID [nvarchar](30), @PurchaseDate [date], @Quantity [int], @Price [money]
AS
BEGIN
	-- =============================================
	-- Author:		Kanth
	-- Create date: 8/28/2019
	-- Description:	add sotck purchase transaction
	-- =============================================

	SET NOCOUNT ON;

    insert into Stocks(StockID, PurchaseDate, Quantity, Price, MarketPrice, Dividend, DividendPerStock)
	select @StockID, @PurchaseDate, @Quantity, @Price, 0, 0, 0

	select * from Stocks where StockID = @StockID order by PurchaseDate desc

END