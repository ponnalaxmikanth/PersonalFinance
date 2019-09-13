CREATE PROCEDURE [dbo].[Get_MF_Funds]
	--exec [Get_MF_Funds]
AS
BEGIN
	select * from MF_Funds order by FundName
END