-- =============================================
-- Author:		Kanth
-- Create date: 06/30/2019
-- Description:	Log Message
-- =============================================
CREATE PROCEDURE LogMessage
	-- Add the parameters for the stored procedure here
	@shortMessage nvarchar(100), @longMessage nvarchar(max), @component nvarchar(100), @application nvarchar(100)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	insert into TLogMessage(ShortMessage, LongMessage, Component, [Application], CreatedDateTime)
	values(@shortMessage, @longMessage, @component, @application, GETDATE())

END
GO
