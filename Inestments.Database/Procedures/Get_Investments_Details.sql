CREATE PROCEDURE [dbo].[Get_Investments_Details]
	-- Add the parameters for the stored procedure here
	@FromDate varchar(50),
	@ToDate varchar(50),
	@portfolioId int

	--exec [Get_Investments_Details] '01/01/2008', '02/03/2017', -1
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
declare @userId varchar(10)
	
if(@portfolioId = -1)
	set @userId = '%'
else
	set @userId = @portfolioId

select DATEFROMPARTS (Year(t.PurchaseDate), MONTH(t.PurchaseDate), 1) [Date] , @portfolioId PortfolioId, sum(t.Amount) Amount, sum(t.Dividend) Dividend, sum(t.CurrentValue) CurrentValue,
sum(t.RedeemAmount) RedeemAmount, sum(t.RedeemDividend) RedeemDividend, sum(t.RedeemValue) RedeemValue
from
(	select t.PurchaseDate, t.PortfolioId, t.Amount Amount, t.Units, t.Dividend, f.FundId, f.SchemaCode
	, n.NAV, t.Units * n.NAV CurrentValue, 0 [RedeemAmount], 0 [RedeemDividend], 0 [RedeemValue]
	from MF_Purchases t
	inner join MF_Funds f on t.FundId = f.FundId
	inner join FundsNav n on f.SchemaCode = n.SchemaCode
	where t.PortfolioId like @userId and t.PurchaseDate between @FromDate and @ToDate
	union all
	select t.SellDate PurchaseDate, t.PortfolioId, 0 Amount, t.SellUnits,0  Dividend, f.FundId, f.SchemaCode
	, n.NAV, (t.SellUnits - isnull(t.SellUnits,0)) * n.NAV CurrentValue, t.Amount + t.Dividend [RedeemAmount], t.Dividend [RedeemDividend], isnull(t.SellUnits * t.SellNAV, 0) - ISNULL(t.SellSTT,0) [RedeemValue]
	from MF_Redeems t
	inner join MF_Funds f on t.FundId = f.FundId
	inner join FundsNav n on f.SchemaCode = n.SchemaCode
	where t.PortfolioId like @userId and t.SellDate between @FromDate and @ToDate
) t

group by Year(t.PurchaseDate), MONTH(t.PurchaseDate), t.PortfolioId
order by DATEFROMPARTS (Year(t.PurchaseDate), MONTH(t.PurchaseDate), 1)

END
