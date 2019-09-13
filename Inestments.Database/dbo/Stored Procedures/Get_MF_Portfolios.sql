CREATE PROCEDURE [dbo].[Get_MF_Portfolios]
	--exec [Get_MF_Portfolios]
AS
BEGIN
	select * from MF_Portfolios where IsActive='Y'	order by PortfolioId
END