-- =============================================
-- Author:		Kanth
-- Create date: 2/1/2020
-- Description:	Get Income Details
-- =============================================
CREATE PROCEDURE [dbo].[GetIncomeDetails]
	-- Add the parameters for the stored procedure here
	@fromDate date, @toDate date
	--exec GetIncomeDetails @fromDate='1/1/2019', @toDate = '12/31/2019'
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    select r.*
	, (r.Billing * r.Hours)  Vendor
	, (r.Billing * r.Hours * (100 - r.Share)) / 100 Employer 
	, (r.Billing * r.Hours * r.Share) / 100  Gross
	, ((r.Billing * r.Hours * r.Share) / 100) - (r.FederalTax + r.SocialTax + r.Medicare + r.StateTax + r.Miscelaneous) + r.Reimbursment NetIncome
	, (r.FederalTax + r.SocialTax + r.Medicare + r.StateTax + r.Miscelaneous) Tax
	, (r.FederalTax + r.SocialTax + r.Medicare + r.StateTax + r.Miscelaneous) * 100 / ((r.Billing * r.Hours * r.Share) / 100) TaxPercent
	from  
		(select PayDate, FromDate, ToDate, Billing, Share, [Hours], Insurance, FederalTax, SocialTax, Medicare, StateTax, Reimbursment, Miscelaneous
				from Income 
			where PayDate between @fromDate and @toDate
		) r
	order by r.PayDate

END