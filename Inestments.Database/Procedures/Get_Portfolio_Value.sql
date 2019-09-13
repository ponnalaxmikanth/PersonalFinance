CREATE PROCEDURE [dbo].[Get_Portfolio_Value]
	@PortfolioId varchar(50), @FromDate date, @ToDate date
	--exec [Get_Portfolio_Value] '-1', '01/01/2017', '05/12/2017'
AS
BEGIN
	SET NOCOUNT ON;

	declare @userId varchar(10)
	
	if(@portfolioId = -1)
		set @userId = '%'
	else
		set @userId = @portfolioId

		select x.PortfolioId, x.FundId, f.FundName, sum(x.Amount) Amount,  sum(x.Dividend) Dividend, sum(x.Units) Units
		, 0 SellUnits, 0 sellAmount , sum(x.Amount)/sum(x.Units) PurchaseNAV --, avg(x.ActualNAV) PurchaseNAV
		, AVG(x.NAV) NAV, x.[Date] LatestNAVDate
		 from (
				select  t.FundId, t.PortfolioId, t.Amount, t.Units, t.Dividend, t.ActualNAV,
				f.schemacode, n.NAV, n.[Date], n.NAV * t.Units CurrentValue
				from MF_Purchases t
				inner join MF_Funds f on t.FundId = f.FundId
				inner join FundsNav n on n.SchemaCode = f.SchemaCode
				where t.PortfolioId like @userId
				and t.PurchaseDate between @FromDate and @ToDate
			)x
			inner join MF_Funds f on x.FundId = f.FundId
			group by x.FundId, x.PortfolioId, f.FundName, x.[Date]
			order by x.PortfolioId, sum(x.Amount) desc

END
