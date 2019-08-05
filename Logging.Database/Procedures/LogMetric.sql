-- =============================================
-- Author:		Kanth
-- Create date: 06/30/2019
-- Description:	Log Metric
-- =============================================
CREATE PROCEDURE LogMetric
	-- Add the parameters for the stored procedure here
	@message nvarchar(100), @metric decimal(18,4), @component nvarchar(100), @application nvarchar(100)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	insert into TLogMetrics([Message], Metric, Component, [Application], CreatedDateTime) 
	values (@message, @metric, @component, @application, GETDATE())

END
GO
