CREATE PROCEDURE [dbo].[AddMFPurchase]
	@PortfolioId int, @houseId int, @typeId int, @categoryId int,  @optionsId int,
	@schemaCode int, @growthschemaCode int, @fundName varchar(100), @PurchaseDate datetime,@FolioId int, 
	@Amount decimal(10,4), @PurchaseNAV decimal(10,4), @Units decimal(10,4),
	@SIP varchar(2)

	 --exec [AddMFPurchase] -1, 4, 13, 12, '4/3/2017', 2000, 48.36, 0, 'Y'
AS
BEGIN
	SET NOCOUNT ON;
	
	declare @fId int = -1

	if(select count(*) from MF_Funds where SchemaCode = @schemaCode and FundOptionId = @optionsId) <= 0
	begin
		insert into MF_Funds(FundHouseId, FundTypeId, FundClassId, FundOptionId, SchemaCode, GrowthSchemaCode, FundName, CreatedDate)
		select @houseId, @typeId, @categoryId, @optionsId, @schemaCode, @growthschemaCode, @fundName, GETDATE()
	end

	select @fId = FundId from MF_Funds where SchemaCode = @schemaCode and FundOptionId = @optionsId

	insert into MF_Purchases(PortfolioId, FolioId, FundId, PurchaseDate, Amount, PurchaseNAV, Units, DividendPerNAV, Dividend, ActualNAV, Broker, CreateDate, LastUpdateDateTime, IsSipInvestment, SIPID)
	select @PortfolioId, @FolioId, @fId, @PurchaseDate, @Amount, @PurchaseNAV, @Units, 0.00, 0.00, @PurchaseNAV, 'Direct', GETDATE(), GETDATE(), @SIP, null


	select @@error

END