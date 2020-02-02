CREATE PROCEDURE [dbo].[BackupFundNavData]
AS
BEGIN
	SET NOCOUNT ON;
	
	DELETE h
		FROM FundsNav_History h
	INNER JOIN FundsNav n 
		ON n.SchemaCode = h.SchemaCode and n.[Date] = h.[Date]


	insert into FundsNav_History(SchemaCode, NAV, RepurchasePrice, SalePrice, [Date])
	select SchemaCode, NAV, RepurchasePrice, SalePrice, [Date] from FundsNav
END