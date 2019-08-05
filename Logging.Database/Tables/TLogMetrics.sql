CREATE TABLE [dbo].[TLogMetrics](
	[LogMetricsId] [bigint] IDENTITY(1,1) NOT NULL,
	[Message] [nvarchar](100) NOT NULL,
	[Metric] [decimal](18, 4) NOT NULL,
	[Component] [nvarchar](100) NOT NULL,
	[Application] [nvarchar](100) NOT NULL,
	[CreatedDateTime] [datetime] NOT NULL,
 CONSTRAINT [PK_TLogMetrics] PRIMARY KEY CLUSTERED 
(
	[LogMetricsId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

