CREATE PROCEDURE [dbo].[AddMFDividend] 
	@PortfolioId int,@folio int, @FundId int, @DividendDate datetime, @Dividend decimal(10,4), @NAV decimal(10,4), @units decimal(10,4), @amount decimal(10,4)
AS
BEGIN
	SET NOCOUNT ON;

declare @funds table (
	transactionId int,
	portfolioId int,
	folioId int,
	fundId int,
	fundOptionId int,
	purchaseDate date,
	amount decimal(10,4),
	units decimal(10,4),
	purchaseNAV decimal(10,4),
	DividendPerNAV decimal(10,4),
	dividend decimal(10,4),
	actualNAV decimal(10,4)
)

--begin transaction

insert into @funds
select t.TransactionId, t.PortfolioId, t.FolioId, t.FundId, o.OptionId, t.PurchaseDate, t.Amount, t.Units, t.PurchaseNAV, t.DividendPerNAV, t.Dividend, t.ActualNAV 
from MF_Purchases t 
inner join MF_Funds f on f.FundId = t.FundId
inner join MF_FundOptions o on o.OptionId = f.FundOptionId
where t.PortfolioId = @PortfolioId and t.FolioId = @folio and t.FundId = @FundId and t.PurchaseDate < @DividendDate

update @funds  set actualNAV = actualNAV - @Dividend, amount = amount - (@Dividend * units), dividend = ISNULL(dividend,0) + (@Dividend * units)
				, DividendPerNAV = ISNULL(DividendPerNAV,0) + @Dividend where fundId = @FundId

insert into @funds (portfolioId, folioId, fundId, fundOptionId, purchaseDate, amount, units, purchaseNAV, dividend, actualNAV, DividendPerNAV)
select portfolioId, folioId, fundId, fundOptionId, @DividendDate, @amount, @units, @NAV, 0, @NAV, 0
from @funds  where fundOptionId = 2
group by portfolioId, folioId, fundId, fundOptionId

update  t set t.ActualNAV = f.actualNAV, t.Amount = f.amount, t.DividendPerNAV = ISNULL(f.DividendPerNAV,0)
, t.Dividend = f.dividend, LastUpdateDateTime = GETDATE()
from MF_Purchases t
inner join @funds f on f.transactionId = t.TransactionId and t.PurchaseDate < @DividendDate

insert into MF_Purchases (portfolioId, folioId, fundId, purchaseDate, amount, purchaseNAV, units, DividendPerNAV, Dividend, actualNAV, CreateDate, LastUpdateDateTime
, Broker, IsSipInvestment)
select portfolioId, folioId, fundId, purchaseDate, sum(amount) amount, purchaseNAV, sum(units) units, 0, 0, actualNAV, GETDATE() CreateDate, GETDATE(), 'Dividend', 'N'
from @funds 
where transactionId is null and fundOptionId = 2
group by portfolioId, folioId, fundId, purchaseDate, purchaseNAV, actualNAV



declare @cnt int

select @cnt = count(*) from MF_Dividends where IsActive='Y' and DividendDate = @DividendDate and FundId = @FundId

if(@cnt <= 0)
begin
	insert into MF_Dividends (FundId, DividendDate, Nav, Dividend, CreateDateTime, LastUpdateDateTime, isactive)
	select @FundId, @DividendDate, @NAV, @Dividend, GETDATE(), GETDATE(), 'Y'
end
--COMMIT TRAN -- Transaction Success!

--select @@ROWCOUNT
END