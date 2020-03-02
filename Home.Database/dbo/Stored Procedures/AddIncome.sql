-- =============================================
-- Author:		Kanth
-- Create date: 2/1/2020
-- Description:	Insert into Income
-- =============================================
CREATE PROCEDURE AddIncome
	@PayDate date, @FromDate date, @ToDate date, @Billing decimal(8,4), @Share decimal(8,4), @Hours decimal(8,4), @Insurance decimal(8,4), @FederalTax decimal(8,4), @SocialTax decimal(8,4), @Medicare decimal(8,4), @StateTax decimal(8,4), @Reimbursment decimal(8,4), @Miscelaneous decimal(8,4), @Comments nvarchar(200)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    insert into Income(PayDate, FromDate, ToDate, Billing, Share, [Hours], Insurance, FederalTax, SocialTax, Medicare, StateTax, Reimbursment, Miscelaneous, Comments)
	select @PayDate, @FromDate, @ToDate, @Billing, @Share, @Hours, @Insurance, @FederalTax, @SocialTax, @Medicare, @StateTax, @Reimbursment, @Miscelaneous, @Comments
END