-- =============================================
-- Author:		Kanth
-- Create date: 05/15/2017
-- updated: 12/20/2018 sip details added
-- Description:	Get transactions by portfolio
-- =============================================
CREATE PROCEDURE [dbo].[GetIndividualTransactions] @PortfolioId int, @Type varchar(50)
--exec GetIndividualTransactions 1, ''
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	declare @userId varchar(10)

	declare @result table(
		TransactionId int,
		PortfolioId int,
		FolioId int, 
		FundId int, 
		SchemaCode int, 
		PurchaseDate date,
		Amount decimal(10,4), 
		Units decimal(10,4), 
		DividendPerNAV decimal(10,4),
		Dividend decimal(10,4),
		CurrentValue decimal(10,4), 
		SellDate date, 
		Type nvarchar(1), 
		IsSipInvestment nvarchar(1), 
		SIPID int
	)
	
	if(@portfolioId = -1)
		set @userId = '%'
	else
		set @userId = @portfolioId

	if(@Type = 'Invest' or @Type = 'All' or @Type = '')
	begin
		insert into @result
		select t.TransactionId, t.PortfolioId, t.FolioId, f.FundId, f.SchemaCode, t.PurchaseDate, t.Amount Amount, t.Units, t.DividendPerNAV, t.Dividend
		, (t.Units * n.NAV) CurrentValue, '1/1/1900' SellDate, 'I' Type, t.IsSipInvestment, t.SIPID
		from MF_Purchases t
		inner join MF_Funds f on t.FundId = f.FundId
		inner join FundsNav n on f.SchemaCode = n.SchemaCode
			where t.PortfolioId  like @userId
	end
	if(@Type = 'Redeem' or @Type = 'All' or @Type = '')
	begin
		insert into @result
		select r.TransactionId, r.PortfolioId, r.FolioId, r.FundId,f.SchemaCode, r.PurchaseDate, r.Amount Amount, r.SellUnits, r.DividendPerNAV, r.Dividend
		, 0 CurrentValue, r.SellDate, 'S' Type, r.IsSipInvestment, r.SIPID
		from MF_Redeems r
		inner join MF_Funds f on r.FundId = f.FundId
			where r.PortfolioId  like @userId
	end
	if(@Type = 'RedeemedInvest' or @Type = 'All' or @Type = '')
	begin
		insert into @result
		select r.TransactionId, r.PortfolioId, r.FolioId, r.FundId,f.SchemaCode, r.PurchaseDate, r.Amount Amount, r.SellUnits, r.DividendPerNAV, r.Dividend
		, (r.SellUnits * r.SellNAV) - ISNULL(r.SellSTT,0) CurrentValue, r.SellDate, 'R' Type, r.IsSipInvestment, r.SIPID --, r.Amount Amount
		from MF_Redeems r
		inner join MF_Funds f on r.FundId = f.FundId
			where r.PortfolioId like @userId
	end
	

	select t.TransactionId, t.PortfolioId, p.Portfolio
	, t.FolioId, fs.FolioNumber
	, h.FundHouseId, h.DsiplayName
	, t.FundId, t.SchemaCode, f.FundName
	, c.FundClassId, c.FundClass, c.IsSectorCategory
	, t.PurchaseDate, t.Amount, t.CurrentValue, t.Units, t.DividendPerNAV, t.Dividend, t.SellDate, t.Type, t.IsSipInvestment, t.SIPID 
	from @result t
	inner join MF_Portfolios p  on t.PortfolioId = p.PortfolioId
	inner join MF_Folios fs on t.FolioId = fs.FolioId
	inner join MF_Funds f on f.FundId = t.FundId
	inner join MF_FundHouses h on h.FundHouseId = f.FundHouseId
	inner join MF_FundCategory c on c.FundClassId = f.FundClassId
	order by t.PortfolioId, h.DsiplayName, f.FundName, t.PurchaseDate desc


	--declare @userId varchar(10)

	--declare @result table(
	--	TransactionId int,
	--	PortfolioId int,
	--	Portfolio nvarchar(100),
	--	FolioId int, 
	--	FolioNumber nvarchar(100),
	--	FundHouseId int, 
	--	DsiplayName nvarchar(100),
	--	FundId int, 
	--	SchemaCode int, 
	--	FundName nvarchar(500),
	--	FundClassId int, 
	--	FundClass nvarchar(100),
	--	IsSectorCategory int,
	--	PurchaseDate date,
	--	Amount decimal(10,4), 
	--	CurrentValue decimal(10,4), 
	--	Units decimal(10,4), 
	--	DividendPerNAV decimal(10,4),
	--	Dividend decimal(10,4), 
	--	SellDate date, 
	--	Type nvarchar(1), 
	--	IsSipInvestment nvarchar(1), 
	--	SIPID int
	--)
	
	--if(@portfolioId = -1)
	--	set @userId = '%'
	--else
	--	set @userId = @portfolioId

	--insert into @result
	--select t.TransactionId, t.PortfolioId, p.Portfolio
	--, t.FolioId, fs.FolioNumber
	--, h.FundHouseId, h.DsiplayName
	--, t.FundId, t.SchemaCode, f.FundName
	--, c.FundClassId, c.FundClass, c.IsSectorCategory
	--, t.PurchaseDate, t.Amount, t.CurrentValue, t.Units, t.DividendPerNAV, t.Dividend, t.SellDate, t.Type, t.IsSipInvestment, t.SIPID
	--from
	--(
	--	select t.TransactionId, t.PortfolioId, t.FolioId, f.FundId, f.SchemaCode, t.PurchaseDate, t.Amount Amount, t.Units, t.DividendPerNAV, t.Dividend
	--	, (t.Units * n.NAV) CurrentValue, '1/1/1900' SellDate, 'I' Type, t.IsSipInvestment, t.SIPID
	--	from MF_Purchases t
	--	inner join MF_Funds f on t.FundId = f.FundId
	--	inner join FundsNav n on f.SchemaCode = n.SchemaCode
	--		where t.PortfolioId  like @userId

	--	union all
	--	select r.TransactionId, r.PortfolioId, r.FolioId, r.FundId,f.SchemaCode, r.PurchaseDate, r.Amount Amount, r.SellUnits, r.DividendPerNAV, r.Dividend
	--	, 0 CurrentValue, r.SellDate, 'S' Type, r.IsSipInvestment, r.SIPID
	--	from MF_Redeems r
	--	inner join MF_Funds f on r.FundId = f.FundId
	--		where r.PortfolioId  like @userId

	--	union all
	--	select r.TransactionId, r.PortfolioId, r.FolioId, r.FundId,f.SchemaCode, r.PurchaseDate, r.Amount Amount, r.SellUnits, r.DividendPerNAV, r.Dividend
	--	, (r.SellUnits * r.SellNAV) - ISNULL(r.SellSTT,0) CurrentValue, r.SellDate, 'R' Type, r.IsSipInvestment, r.SIPID --, r.Amount Amount
	--	from MF_Redeems r
	--	inner join MF_Funds f on r.FundId = f.FundId
	--		where r.PortfolioId like @userId
	--) t
	--inner join MF_Funds f on t.FundId = f.FundId
	--inner join MF_Portfolios p on t.PortfolioId = p.PortfolioId
	--inner join MF_Folios fs on t.FolioId = fs.FolioId
	--inner join MF_FundHouses h on h.FundHouseId = f.FundHouseId
	--inner join MF_FundCategory c on c.FundClassId = f.FundClassId
	--order by t.PortfolioId, h.DsiplayName, f.FundName, t.PurchaseDate desc--, t.FundId

	--select * from @result

	----select f.FundName, t.PurchaseDate, t.SellDate, t.Type, SUM(t.Amount) Amount, SUM(t.CurrentValue) CurrentValue
	----from
	----(
	----	select t.PortfolioId, t.PurchaseDate, GETDATE() SellDate, f.FundId, f.SchemaCode, t.Amount Amount, (t.Units * n.NAV) CurrentValue, 'I' Type
	----	from MF_Purchases t
	----	inner join MF_Funds f on t.FundId = f.FundId
	----	inner join FundsNav n on f.SchemaCode = n.SchemaCode
	----	where t.PortfolioId like @userId
	----	union all
	----	select t.PortfolioId, t.PurchaseDate, t.SellDate, f.FundId, f.SchemaCode, t.Amount Amount, 0 CurrentValue, 'S' Type
	----	from MF_Redeems t
	----	inner join MF_Funds f on t.FundId = f.FundId
	----	where t.PortfolioId like @userId
	----	union all
	----	select r.PortfolioId, r.PurchaseDate, r.SellDate, r.FundId, f.SchemaCode, r.Amount Amount, (r.SellUnits * r.SellNAV) - ISNULL(r.SellSTT,0) CurrentValue
	----				, 'R' Type
	----	from MF_Redeems r
	----	inner join MF_Funds f on r.FundId = f.FundId
	----	inner join FundsNav n on f.SchemaCode = n.SchemaCode
	----	where r.PortfolioId like @userId
	----) t
	----inner join MF_Funds f on t.FundId = f.FundId
	----inner join MF_Portfolios p on t.PortfolioId = p.PortfolioId
	----group by t.PurchaseDate, t.SellDate, f.FundName, t.Type
	----order by t.PurchaseDate desc
END