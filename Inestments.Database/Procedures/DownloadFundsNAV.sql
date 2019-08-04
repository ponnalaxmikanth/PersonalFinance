-- =============================================
-- Author:		Kanth
-- Create date: 07/01/2019
-- Description:	Download Funds NAV from amfiindia
-- =============================================
CREATE PROCEDURE DownloadFundsNAV
	@xml as xml, @schemaType nvarchar(100)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	declare @table table (
			fundHouseId int,
			fundHouse nvarchar(100),
			code int,
			ISINGrowth nvarchar(100),
			ISINDivReinv nvarchar(100),
			Name nvarchar(max),
			NAV decimal(10,4),
			RepurchasePrice decimal(10,4),
			SellPrice decimal(10,4),
			Date date,
			FundOption int,
			Fund_Type int,
			FundType nvarchar(100)
		)

	insert into @table (fundHouse, code, ISINGrowth, ISINDivReinv,Name, NAV, RepurchasePrice, SellPrice, [Date], FundOption, Fund_Type, FundType)
		select distinct 
		'fundHouse' = x.v.value('FundHouse[1]', 'nvarchar(100)'),
		'Code' = x.v.value('Code[1]', 'int'),
		'ISINGrowth' = x.v.value('ISINGrowth[1]', 'nvarchar(100)'),
		'ISINDivReinv' = x.v.value('ISINDivReinv[1]', 'nvarchar(100)'),
		'Name' = x.v.value('Name[1]', 'nvarchar(max)'),
		'NAV' = x.v.value('NAV[1]', 'decimal(18,4)'),
		'RepurchasePrice' = x.v.value('RepurchasePrice[1]', 'decimal(18,4)'),
		'SellPrice' = x.v.value('SellPrice[1]', 'decimal(18,4)'),
		'[Date]' = x.v.value('Date[1]', 'date'),
		'FundOption' = x.v.value('FundOption[1]', 'int'),
		'Fund_Type' = x.v.value('Fund_Type[1]', 'int'),
		'FundType' = x.v.value('FundType[1]', 'nvarchar(100)')
		from @xml.nodes('/root/fund') x(v)

	update t set t.fundHouseId = h.FundHouseId
		from @table t
		inner join MF_FundHouses h on h.FundHouseName = t.fundHouse

	if(select count(*) from @table where fundHouseId is null) > 0
	begin
		insert into MF_FundHouses (FundHouseName, DsiplayName, [Site], [Description], CreatedDate)
		select t.fundHouse, t.fundHouse, 'Auto', t.fundHouse, GETDATE() 
		from @table t
		where t.fundHouseId is null

		update t set t.fundHouseId = h.FundHouseId
			from @table t
			inner join MF_FundHouses h on h.FundHouseName = t.fundHouse
		where t.fundHouseId is null
	end

	update n 
		set n.NAV = t.NAV, n.Date = t.Date, n.LastUpdateDateTime = GETDATE()
		--n.ISINGrowth = t.ISINGrowth, n.ISIN_DIV_Reinvestment = t.ISINDivReinv, n.SchemaName = t.Name, n.FundType = t.FundType, n.FundOption = t.FundOption, n.Fund_Type = t.Fund_Type,n.SchemeType = @schemaType, 	
		from FundsNav n
		inner join @table t on n.schemacode = t.code

	insert into FundsNav (SchemaCode, Date, NAV, RepurchasePrice, SalePrice, CreateDateTime, LastUpdateDateTime)
	--, Fund_Type, FundHouseId, FundOption, FundType, ISIN_DIV_Reinvestment, ISINGrowth, SchemaName
	select t.code, t.Date, t.NAV, t.RepurchasePrice, t.SellPrice, GETDATE(), GETDATE()
	--, t.Fund_Type, t.fundHouseId, t.FundOption, t.FundType, t.ISINDivReinv, t.ISINGrowth, t.Name
		from @table t
		left join FundsNav n on t.code = n.SchemaCode
	where n.SchemaCode is null

	select @@ERROR error
    
END
