-- =============================================
-- Author:		Kanth
-- Create date: 2/20/2020
-- Description:	Calculate % Growth per Anum or XIRR
-- =============================================
CREATE FUNCTION [GetXIRR]
(
	@currentValue decimal(10,4), @investValue decimal(10,4), @investDate date
)
--exec GetXIRR 6448.63, 3464.7576, '2016-12-12'
RETURNS decimal(10,4)
AS
BEGIN
	---- Declare the return variable here
	--DECLARE <@ResultVar, sysname, @Result> <Function_Data_Type, ,int>

	declare @result decimal(10,4)

	select @result = (power((@currentValue/@investValue), (365.00/DATEDIFF(day, @investDate,GETDATE())))-1)*100
	RETURN @result

END