-- =============================================
-- Author:		Kanth
-- Create date: 3/21/2020
-- Description:	Add Dividend for US Share
-- =============================================
CREATE PROCEDURE [dbo].[AddDividendForUSStock]
	@stock varchar(50), @divdate date, @dividend decimal(18,4)
	--exec AddDividendForUSStock @stock='MSFT', @divdate='14-Jun-2018', @dividend=0.42
AS
BEGIN
	SET NOCOUNT ON;
	declare @qty decimal(18,0), @divpershare decimal(18,4), @retvalue int = 0

	select @qty = SUM(Quantity) from Stocks where StockID = @stock and PurchaseDate <= @divdate

	set @divpershare = @dividend / @qty

	IF NOT EXISTS (select * from Stock_Dividends where StockID = @stock and DividendDate = @divdate and ISNULL(@divpershare, 0.0) > 0.0)
	BEGIN
		update Stocks 
			set Dividend = ISNULL(Dividend, 0) + (@divpershare * Quantity), DividendPerStock = ISNULL(DividendPerStock, 0) + @divpershare
		where  StockID = @stock and PurchaseDate <= @divdate
	
		insert into Stock_Dividends (StockID, DividendDate, DividendPerStock, Dividend, CreateDateTime, LastUpdateDateTime)
		select @stock, @divdate, @divpershare, @dividend, GETDATE(), GETDATE()

	END
	ELSE
	BEGIN
		set @retvalue = 1
	END

	select @retvalue, @qty, @divpershare
END