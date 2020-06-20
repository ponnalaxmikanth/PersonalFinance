-- =============================================
-- Author:		Kanth
-- Create date: 3/13/2020
-- Description:	Get Digital Currency Avg Prices
-- =============================================
CREATE PROCEDURE GetDCAvgPRices
--exec GetDCAvgPRices
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	select r.Currency, sum(r.Size) Size, SUM(r.investPrice) investAmount, SUM(r.investPrice)/sum(r.Size) avgPrice, avg(r.CurrentPrice) CurrentPrice
	from (
		select dc.Currency, dc.Size, (dc.Size * dc.Price) + dc.Fees investPrice, dc.CurrentPrice
		from DigitalCurrency dc
	) r
	group by r.Currency

END