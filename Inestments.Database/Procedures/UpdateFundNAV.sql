CREATE PROCEDURE [dbo].[UpdateFundNAV]
	@fundhouse varchar(500),
	@schemaCode int, 
	@isinGrowth varchar(100), 
	@isinDivReInv varchar(100),
	@fundType int,
	@SchemaName nvarchar(max),
	@nav decimal(10,4),
	--@repurchasePrice decimal(10,4),
	--@salePrice decimal(10,4),
	@fundOption int,
	@fund_type int,
	@date date
AS
BEGIN
	SET NOCOUNT ON;

	--if(@repurchasePrice = -99999999)
	--	set @repurchasePrice = null
	--if(@salePrice = -99999999)
	--	set @salePrice = null

	declare @fundhouseId int = -1;
	
	if(select count(*) from MF_FundHouses where FundHouseName = @fundhouse) > 0
	begin
		select @fundhouseId = FundHouseId from MF_FundHouses where FundHouseName = @fundhouse;
	end
	else
	begin
		insert into MF_FundHouses(FundHouseName, Site, Description, CreatedDate)
		select @fundhouse, 'Me', @fundhouse, getdate()

		select @fundhouseId = FundHouseId from MF_FundHouses where FundHouseName = @fundhouse;
	end

	if((select count(*) from FundsNav where SchemaCode = @schemaCode) > 0)
	begin
		update FundsNav set NAV = @nav, SchemaName=@SchemaName,
							--RepurchasePrice=case when @repurchasePrice = -99999999 then null else @repurchasePrice end, 
							--SalePrice=case when @salePrice=-99999999 then null else @salePrice end, 
							--RepurchasePrice= @repurchasePrice, 
							--SalePrice= @salePrice, 
							[Date] = @date, 
							FundOption = @fundOption,
							Fund_Type = @fund_type,
							FundHouseId = @fundhouseId,
							LastUpdateDateTime = GETDATE() 
		where SchemaCode = @schemaCode --and [Date] = @date
	end
	else
	begin
		insert into FundsNav (SchemaCode, ISINGrowth, ISIN_DIV_Reinvestment, SchemaName, FundType, FundOption, Fund_Type, NAV, RepurchasePrice, SalePrice
					, FundHouseId, [Date], CreateDateTime, LastUpdateDateTime)
		select @schemaCode, @isinGrowth, @isinDivReInv, @SchemaName, @fundType, @fundOption, @fund_type, @nav, null, null
					, @fundhouseId , @date, GETDATE(), GETDATE()
	end

	--if((select count(*) from FundsNav where SchemaCode = @schemaCode and [Date] = @date) > 0)
	--begin
	--	update FundsNav set NAV = @nav, [Date] = @date, LastUpdateDateTime = GETDATE() where SchemaCode = @schemaCode and [Date] = @date
	--end
	--else
	--begin
	--	insert into FundsNav (SchemaCode, ISINGrowth, ISIN_DIV_Reinvestment, SchemaName, FundType, NAV, RepurchasePrice, SalePrice, [Date], CreateDateTime, LastUpdateDateTime)
	--	select @schemaCode, @isinGrowth, @isinDivReInv, @SchemaName, @fundType, @nav, null, null, @date, GETDATE(), GETDATE()
	--end
END
