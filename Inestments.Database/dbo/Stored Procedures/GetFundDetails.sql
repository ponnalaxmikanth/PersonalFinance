-- =============================================
-- Author:		Kanth
-- Create date: 2/20/2020
-- Description:	Get Funds Investment Details
-- =============================================
CREATE PROCEDURE GetFundDetails
	-- Add the parameters for the stored procedure here
	@portfolioId int
AS
	--exec GetFundDetails @portfolioId = 1
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

   select fs.FundName, SUM(r.Amount) InvestedAmount, sum(r.CurrentAmount) CurrentAmount, AVG(r.XIRR) XIRR
	, AVG(r.NAV) currentNAV, AVG(r.PurchaseNAV) PurchaseNAV, AVG(r.noOfDays) AvgDays
	, AVG(r.DividendPerNAV) DividendPerNAV, SUM(r.Dividend) Dividend, SUM(r.Units) Units
	, r.PortfolioId, h.DsiplayName, t.FundType, c.FundClass, c.IsSectorCategory IsSectorFund, o.FundOption
	from (select p.PortfolioId, f.FundId, p.Amount, n.NAV * p.Units CurrentAmount, p.PurchaseNAV, p.Units
		,p.DividendPerNAV, p.Dividend, p.ActualNAV, n.NAV, DATEDIFF(day, p.PurchaseDate,GETDATE()) noOfDays
		, dbo.GetXIRR(n.NAV * p.Units, p.Amount, p.PurchaseDate) XIRR
			from MF_Purchases p
			inner join MF_Funds f on p.FundId = f.FundId
			inner join FundsNav n on f.SchemaCode = n.SchemaCode
		where p.PortfolioId = @portfolioId
	)r
	inner join MF_Funds fs on r.FundId = fs.FundId
	inner join MF_FundHouses h on h.FundHouseId = fs.FundHouseId
	inner join MF_FundTypes t on t.FundTypeId = fs.FundTypeId
	inner join MF_FundCategory c on c.FundClassId = fs.FundClassId
	inner join MF_FundOptions o on o.OptionId = fs.FundOptionId
	group by r.PortfolioId, fs.FundName, h.DsiplayName, t.FundType, c.FundClass, c.IsSectorCategory, o.FundOption
	order by AVG(r.XIRR)

END