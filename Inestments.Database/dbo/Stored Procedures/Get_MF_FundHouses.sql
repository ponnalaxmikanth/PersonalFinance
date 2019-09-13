CREATE PROCEDURE [dbo].[Get_MF_FundHouses]
	--exec [Get_MF_FundHouses]
AS
BEGIN
	select * from MF_FundHouses order by FundHouseId
END