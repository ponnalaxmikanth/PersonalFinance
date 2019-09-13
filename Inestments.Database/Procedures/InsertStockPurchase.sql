CREATE PROCEDURE InsertStockPurchase
	@StockID [nvarchar](30), @PurchaseDate [date], @Quantity [int], @Price [money]
AS
BEGIN
	-- =============================================
	-- Author:		Kanth
	-- Create date: 8/28/2019
	-- Description:	add sotck purchase transaction
	-- =============================================

	SET NOCOUNT ON;

    insert into Stocks(StockID, PurchaseDate, Quantity, Price)
	select @StockID, @PurchaseDate, @Quantity, @Price

END