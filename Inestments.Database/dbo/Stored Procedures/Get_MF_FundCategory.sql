CREATE PROCEDURE [dbo].[Get_MF_FundCategory]
	--exec [Get_MF_FundCategory]
AS
BEGIN
	select * from MF_FundCategory order by FundClassId
END