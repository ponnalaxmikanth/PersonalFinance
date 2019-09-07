
-- =============================================
-- Author:		Kanth
-- Create date: 12/1/2018
-- Description:	Get purchase groups
-- =============================================
CREATE PROCEDURE [dbo].[GetExpenseGroups]
	-- Add the parameters for the stored procedure here
--exec GetExpenseGroups
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	select g.GroupId, g.GroupName from ExpenseGroups g
END
