-- =============================================
-- Author:		Kanth
-- Create date: 12/1/2018
-- Description:	Get Account Types
-- =============================================
CREATE PROCEDURE [dbo].[GetAccountTypes]
--exec GetAccountTypes
	-- Add the parameters for the stored procedure here
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	select t.AccountTypeId, t.AccountType from AccountTypes t 
END
