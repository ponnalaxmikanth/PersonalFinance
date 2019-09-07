CREATE PROCEDURE [dbo].[GetLastProcessedDetails]
AS
	select MAX(Date) NAVDate, MAX(LastUpdateDateTime) LastUpdateDateTime from FundsNav
RETURN 0
