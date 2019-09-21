-- =============================================
-- Author:		Laxmikanth
-- Create date: 9/21/2019
-- Description:	Get Mutual Funds Overall & Current Investment details
-- =============================================
CREATE PROCEDURE [dbo].[GetFundsInvestmentDetails]
	--exec GetFundsInvestmentDetails
AS
BEGIN
	select SUM(r.Investment) Investment, SUM(r.currentValue) CurrentValue, 'C' Type
	from (
		select p.Amount Investment, (p.Units * n.NAV) CurrentValue
		from MF_Purchases p
		inner join MF_Funds f on f.FundId = p.FundId
		inner join FundsNav n on f.SchemaCode = n.SchemaCode
	) r
	UNION ALL
	select SUM(r.Investment) Investment, SUM(r.currentValue) CurrentValue, 'R' Type
	from (
		select r.Amount Investment, (r.SellNAV * r.SellUnits) - r.SellSTT CurrentValue
			from MF_Redeems r
		) r

END