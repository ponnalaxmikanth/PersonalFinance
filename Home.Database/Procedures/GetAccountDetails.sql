-- =============================================
-- Author:		Kanth
-- Create date: 12/1/2018
-- Description:	Get Account Detials
-- =============================================
CREATE PROCEDURE [dbo].[GetAccountDetails]
--exec GetAccountDetails
	-- Add the parameters for the stored procedure here
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	select a.AccountId, a.Name, a.DisplayName, a.DisplayOrder, a.OpenDate, a.Limit, a.LimitIncreaseDate, a.LimitIncreaseStatus, t.AccountTypeId, t.AccountType
	from Accounts a
	inner join AccountTypes t on a.AccountTypeId = t.AccountTypeId
	where a.Status = 'Y'
	order by a.DisplayOrder

END
