-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,01-MAR-2017,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Get_SIP_Details]
AS
BEGIN

	SET NOCOUNT ON;

	select s.SIPDate, s.PortfolioId, p.Portfolio, s.SchemaCode, n.SchemaName, s.Amount SIPAmount, s.FromDate, s.ToDate,
	ISNULL(sum(t.Amount), 0) Amount, ISNULL(SUM(t.Dividend),0) Dividend, ISNULL(sum(t.Units),0) Units, AVG(n.NAV) CurrentNAV, ISNULL(AVG(t.ActualNAV), 0) AvgNAV
	from MF_SIPS s
	inner join FundsNav n on s.SchemaCode = n.SchemaCode
	inner join MF_Portfolios p on p.PortfolioId = s.PortfolioId
	inner join MF_Funds f on f.SchemaCode = s.SchemaCode
	left join MF_Purchases t on s.PortfolioId = t.PortfolioId and s.SchemaCode = f.SchemaCode and t.FundId = f.FundId and t.IsSipInvestment = 'Y' and s.SIPId = t.sipid
	where s.IsActive = 'Y'
	group by s.SchemaCode, s.PortfolioId, s.FromDate, s.ToDate, s.Amount, s.SIPDate, p.Portfolio, n.SchemaName

END
