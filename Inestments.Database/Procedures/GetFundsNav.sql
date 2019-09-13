CREATE PROCEDURE [dbo].[GetFundsNav] @NavDate varchar(50)
AS
BEGIN
	SET NOCOUNT ON;

   select h.SchemaCode, h.ISINGrowth, h.ISIN_DIV_Reinvestment, h.NAV, h.RepurchasePrice, h.SalePrice, h.[Date] 
		from FundsNav_History h
		where h.[Date] = @NavDate
	union all
	select n.SchemaCode, n.ISINGrowth, n.ISIN_DIV_Reinvestment, n.NAV, n.RepurchasePrice, n.SalePrice, n.[Date] 
		from FundsNav n
		where n.[Date] = @NavDate

END