-- =============================================
-- Author:		Kanth
-- Create date: 1/26/2020
-- Description:	Get Funds Current Investment Value by Portfolio and Fund
-- =============================================
CREATE PROCEDURE GetFundsValue
	 @portfolio int
	 --exec GetFundsValue @portfolio = 1
AS
BEGIN

	SET NOCOUNT ON;

	select p.FundId, n.SchemaCode, n.SchemaName, fd.Category, t.FundType, o.FundOption
	, AVG(DATEDIFF(day, p.PurchaseDate, GETDATE())) avgDays, SUM(p.Units) Units, SUM(p.Dividend) Dividend, AVG(p.PurchaseNAV) PurchaseNAV, AVG(p.ActualNAV) ActualNAV
	, AVG(n.NAV) NAV, sum(p.Amount) Amount, sum(n.NAV * p.Units) currentValue
		from MF_Purchases p
		inner join MF_Funds f on f.FundId = p.FundId
		inner join FundsNav n on n.SchemaCode = f.SchemaCode
		inner join MF_FundDetails fd on f.SchemaCode = fd.SchemaCode
		inner join MF_FundTypes t on t.FundTypeId =f.FundTypeId
		inner join MF_FundOptions o on o.OptionId = f.FundOptionId
	where p.PortfolioId = @portfolio --and n.SchemaCode = 119028
	group by p.FundId, n.SchemaCode, n.SchemaName, fd.Category, t.FundType, o.FundOption
	order by n.SchemaName

END