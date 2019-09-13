-- =============================================
-- Author:		Kanth
-- Create date: 05/14/2017
-- Description:	Get Investment details by fund category
-- =============================================
CREATE PROCEDURE [dbo].[GetInvestmentsByFundCategory]  @portfolioId int
-- exec GetInvestmentsByFundCategory 1
AS
BEGIN
	SET NOCOUNT ON;

	declare @userId varchar(5)

	declare @result table
	(
		FundClass varchar(max),
		Amount numeric(10,2),
		CurrentValue numeric(10,2),
		Profit numeric(10,2)
	)

	if(@portfolioId = -1)
		set @userId = '%'
	else
		set @userId = @portfolioId

	insert into @result(FundClass, Amount, CurrentValue, Profit)
    select c.FundClass, sum(t.Amount) Amount, sum((t.Units * n.NAV)) currentValue, sum((t.Units * n.NAV)) - sum(t.Amount) Profit
	from MF_Purchases t
	inner join MF_Funds f on t.FundId = f.FundId
	inner join FundsNav n on f.SchemaCode = n.SchemaCode
	inner join MF_FundCategory c on f.FundClassId = c.FundClassId
	where t.PortfolioId like @userId
	group by c.FundClass
	
	select * from @result
END