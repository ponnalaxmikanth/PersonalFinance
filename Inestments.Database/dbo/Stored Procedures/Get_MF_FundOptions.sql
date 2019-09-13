CREATE PROCEDURE [dbo].[Get_MF_FundOptions]
	--exec [Get_MF_FundOptions]
AS
BEGIN
	select * from MF_FundOptions order by OptionId
END