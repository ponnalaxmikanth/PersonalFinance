
-- =============================================
-- Author:		Kanth
-- Create date: 1/20/2018
-- Description:	Get ULIP Value
-- =============================================
CREATE PROCEDURE [dbo].[GetULIPValue]
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    DECLARE @FromDate DATETIME = '2008-11-18', @ToDate DATETIME = getdate()
	declare @invest int
	declare @units numeric(10,4)
	declare @nav numeric(10,4)
	declare @months int = (DATEDIFF(MONTH, @FromDate, @ToDate) - (CASE WHEN DATEADD(MONTH, DATEDIFF(MONTH, @FromDate, @ToDate), @FromDate) > @ToDate THEN 1 ELSE 0 END))
	SELECT @invest = @months * 2000 

	set @nav = (select top 1 NAV from SBI_ULIP order by PurchaseDate desc)
	set @units = (select sum(Units) from SBI_ULIP)

	select @months, @invest Invest, @nav NAV, @units Units, @units * @nav CurrentValue

END