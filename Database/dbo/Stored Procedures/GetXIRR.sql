-- =============================================
-- Author:		Kanth
-- Create date: 2/5/2018
-- Description:	Calculate % Growth per Anum or XIRR
-- =============================================
CREATE PROCEDURE GetXIRR
	@currentValue decimal(10,4), @investValue decimal(10,4), @investDate date, @currentDate date
	--exec GetXIRR 6448.63, 3464.7576, '2016-12-12', '2018-02-05'
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    declare @result decimal(10,2)

	select @result = (power((@currentValue/@investValue), (365.00/DATEDIFF(day, @investDate, @currentDate)))-1)*100
	select @result
END