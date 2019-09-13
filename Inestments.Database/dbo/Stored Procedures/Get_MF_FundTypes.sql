CREATE PROCEDURE [dbo].[Get_MF_FundTypes]
	--exec [Get_MF_FundTypes]
AS
BEGIN
	select * from MF_FundTypes order by FundTypeId
END