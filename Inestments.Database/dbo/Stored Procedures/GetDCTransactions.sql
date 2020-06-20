-- =============================================
-- Author:		Kanth
-- Create date: 3/13/2020
-- Description:	Get Digital Currency Transactions
-- =============================================
CREATE PROCEDURE GetDCTransactions
--exec GetDCTransactions	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    select dc.Currency, dc.PurchaseDate, dc.Size, dc.Price, dc.Fees, dc.CurrentPrice
	from DigitalCurrency dc
	order by dc.Currency, dc.PurchaseDate

	
END