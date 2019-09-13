CREATE PROCEDURE [dbo].[GetMyFunds]
@transaction varchar(50), @portfolioId int

--exec [GetMyFunds] 'add', 1
--exec [GetMyFunds] 'redeem', 1
AS
BEGIN
	SET NOCOUNT ON;

	if(@transaction = 'add')
	begin
		select distinct -1 PortfolioId, f.FundId, f.FundName , f.FundHouseId, h.FundHouseName, f.FundTypeId, t.FundType, f.FundClassId, c.FundClass,
		 f.FundOptionId, o.FundOption, f.SchemaCode, f.GrowthSchemaCode, g.SchemaName
		from MF_Funds f
		inner join MF_FundHouses h on f.FundHouseId = h.FundHouseId
		inner join MF_FundTypes t on f.FundTypeId = t.FundTypeId
		inner join MF_FundCategory c on f.FundClassId = c.FundClassId
		inner join MF_FundOptions o on f.FundOptionId = o.OptionId
		inner join FundsNav g on f.GrowthSchemaCode = g.SchemaCode
		order by f.FundName	
	end
	else
	begin
		select distinct p.PortfolioId, f.FundId, f.FundName , f.FundHouseId, h.FundHouseName, f.FundTypeId, t.FundType, f.FundClassId, c.FundClass,
		 f.FundOptionId, o.FundOption, f.SchemaCode, f.GrowthSchemaCode, g.SchemaName
		from MF_Funds f
		inner join MF_Purchases p on f.FundId = p.FundId
		inner join MF_FundHouses h on f.FundHouseId = h.FundHouseId
		inner join MF_FundTypes t on f.FundTypeId = t.FundTypeId
		inner join MF_FundCategory c on f.FundClassId = c.FundClassId
		inner join MF_FundOptions o on f.FundOptionId = o.OptionId
		inner join FundsNav g on f.GrowthSchemaCode = g.SchemaCode
		--where p.PortfolioId = @portfolioId
		order by p.PortfolioId, f.FundHouseId, f.FundId
	end
	
END