-- =============================================
-- Author:		<Author,,Name>
-- Create date: <3/28/2017 10:28 PM>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Insetrt_MF_DataDumpDates]
	
	@Date varchar(50),
	@FundType int,
	@Count int
	--exec [dbo].[Insetrt_MF_DataDumpDates] '3/29/2017', 3
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	--if((select count(*) from FundsNav where SchemaCode = @schemaCode) > 0)
	--if((select count(*) from MF_DataDumpDates where [Date] = @Date and FundType = @FundType) = 0)
	--begin
		insert into MF_DataDumpDates (Date, FundType, [Count], CreateDate)
		select @Date, @FundType, @Count, GETDATE()
	--end
END
