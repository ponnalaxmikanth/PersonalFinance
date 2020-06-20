-- =============================================
-- Author:		Kanth
-- Create date: 3/20/2020
-- Description:	Sell Stock in US
-- =============================================
CREATE PROCEDURE [dbo].[SellUSAStock]
	@stockId varchar(50), @SelllDate Date, @price decimal(18,4), @Quantity int, @PurchaseDate date
	--exec SellUSAStock @stockId='ANET', @SelllDate='12/23/2019', @price=208.43, @Quantity=1, @PurchaseDate='11/1/2019'
AS
BEGIN
	declare @qty int
	BEGIN TRY
		BEGIN TRANSACTION
			
			select @qty = Quantity from Stocks where StockID = @stockId and PurchaseDate = @PurchaseDate

			IF(@qty >= @Quantity)
			BEGIN
				insert into Stock_Redeems(StockID, PurchaseDate, Quantity, Price, Dividend, DividendPerStock, SellDate, SellPrice, CreateDateTime, LastUpdateDateTime)
				select StockID, PurchaseDate, @Quantity, Price, Dividend, DividendPerStock, @SelllDate, @price, GETDATE(), GETDATE()
					from Stocks where StockID = @stockId and PurchaseDate = @PurchaseDate
			END
			IF(@qty = @Quantity)
			BEGIN
				delete from Stocks where StockID = @stockId and PurchaseDate = @PurchaseDate
			END
			ELSE IF(@qty > @Quantity)
			BEGIN
				update Stocks set Quantity = Quantity - @Quantity, LastUpdateDateTime = GETDATE() where StockID = @stockId and PurchaseDate = @PurchaseDate
			END
			delete from Stocks where Quantity <= 0
		COMMIT TRAN -- Transaction Success!
	END TRY
	BEGIN CATCH
		rollback TRAN
		DECLARE @ErMessage NVARCHAR(2048), @ErSeverity INT, @ErState INT
		SELECT @ErMessage = ERROR_MESSAGE(), @ErSeverity = ERROR_SEVERITY(), @ErState = ERROR_STATE()
		RAISERROR (@ErMessage, @ErSeverity, @ErState)
	END CATCH
	
	select @qty

END