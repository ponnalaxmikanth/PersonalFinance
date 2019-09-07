-- =============================================
-- Author:		Kanth
-- Create date: 12/2/2018
-- Description:	Get purchase sub groups
-- =============================================
CREATE PROCEDURE [dbo].[GetExpenseSubGroups]
	-- Add the parameters for the stored procedure here
--exec [GetExpenseSubGroups]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	select g.GroupId, g.Id, g.SubGroupName from ExpensesSubGroup g
END
