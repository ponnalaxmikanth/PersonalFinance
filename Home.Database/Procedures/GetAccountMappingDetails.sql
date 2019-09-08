-- =============================================
-- Author:		Kanth
-- Create date: 05/20/2019
-- Description:	Get Account Details and mapping between google excel sheet mappings
-- =============================================
CREATE PROCEDURE [dbo].[GetAccountMappingDetails]
--exec GetAccountMappingDetails
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	select a.AccountId, a.Name, a.DisplayName, a.ExcelMapping
	, at.AccountTypeId, at.AccountType
		from Accounts a
		inner join AccountTypes at on a.AccountTypeId = at.AccountTypeId 
	order by a.DisplayOrder
END
