-- =============================================
-- Author:		Kanth
-- Create date: 05/15/2017
-- Description:	Get transactions by portfolio
-- =============================================
CREATE PROCEDURE [dbo].[GetTransactions] @PortfolioId int
--exec GetTransactions 3
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	declare @userId varchar(10)
	
	if(@portfolioId = -1)
		set @userId = '%'
	else
		set @userId = @portfolioId

	--select f.FundName, t.PurchaseDate, t.SellDate, t.Type, SUM(t.Amount) Amount, SUM(t.Redeem) RedeemAmount, SUM(t.CurrentValue) CurrentValue, SUM(t.RedeemValue) RedeemValue
	--from
	--(
	--	select t.PortfolioId, t.PurchaseDate, f.FundId, f.SchemaCode, t.Amount Amount, 0 Redeem, (t.Units * n.NAV) CurrentValue, 0 RedeemValue, 'I' Type, t.SellDate
	--	from MF_Transactions t
	--	inner join MF_Funds f on t.FundId = f.FundId
	--	inner join FundsNav n on f.SchemaCode = n.SchemaCode
	--	where t.PortfolioId like @userId --and t.FundId = 1 and t.SellDate='2013-02-11' and t.PurchaseDate='2013-03-15'
	--	union all
	--	select r.PortfolioId, r.PurchaseDate, r.FundId, f.SchemaCode, r.Amount, r.Amount Redeem, 0 CurrentValue, (r.SellUnits * r.SellNAV) - ISNULL(r.SellSTT,0) RedeemValue
	--	, 'R' Type, r.SellDate
	--	from MF_RedeemTransactions r 
	--	inner join MF_Funds f on r.FundId = f.FundId
	--	where r.PortfolioId like @userId --and r.FundId = 1 and r.SellDate='2013-02-11' and r.PurchaseDate='2013-03-15'
	--) t
	--inner join MF_Funds f on t.FundId = f.FundId
	--inner join MF_Portfolios p on t.PortfolioId = p.PortfolioId
	--group by t.PurchaseDate, t.SellDate, f.FundName, t.Type
	----order by t.PurchaseDate desc

	select f.FundName, t.PurchaseDate, t.SellDate, t.Type, SUM(t.Amount) Amount, SUM(t.Redeem) RedeemAmount, SUM(t.CurrentValue) CurrentValue, SUM(t.RedeemValue) RedeemValue
	from
	(
		select t.PortfolioId, t.PurchaseDate, f.FundId, f.SchemaCode, t.Amount Amount, 0 Redeem, (t.Units * n.NAV) CurrentValue, 0 RedeemValue, 'I' Type, GETDATE() SellDate
		from MF_Purchases t
		inner join MF_Funds f on t.FundId = f.FundId
		inner join FundsNav n on f.SchemaCode = n.SchemaCode
		where t.PortfolioId like @userId
		union all
		select t.PortfolioId, t.PurchaseDate, f.FundId, f.SchemaCode, t.Amount Amount, 0 Redeem, 0 CurrentValue, 0 RedeemValue, 'S' Type, t.SellDate
		from MF_Redeems t
		inner join MF_Funds f on t.FundId = f.FundId
		where t.PortfolioId like @userId
		union all
		select r.PortfolioId, r.SellDate SellDate, r.FundId, f.SchemaCode,0 Amount, r.Amount Redeem,0 CurrentValue, (r.SellUnits * r.SellNAV) - ISNULL(r.SellSTT,0) RedeemValue
					, 'R' Type, r.PurchaseDate
		from MF_Redeems r
		inner join MF_Funds f on r.FundId = f.FundId
		inner join FundsNav n on f.SchemaCode = n.SchemaCode
		where r.PortfolioId like @userId
	) t
	inner join MF_Funds f on t.FundId = f.FundId
	inner join MF_Portfolios p on t.PortfolioId = p.PortfolioId
	group by t.PurchaseDate, t.SellDate, f.FundName, t.Type
	order by t.PurchaseDate desc
END
