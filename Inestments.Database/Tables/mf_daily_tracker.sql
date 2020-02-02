CREATE TABLE [dbo].[mf_daily_tracker](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[portfolioId] [int] NOT NULL,
	[date] [date] NOT NULL,
	[period] [int] NOT NULL CONSTRAINT [DF_mf_daily_tracker_period]  DEFAULT ((-1)),
	[investment] [decimal](18, 4) NOT NULL,
	[currentvalue] [decimal](18, 4) NOT NULL,
	[profit] [decimal](10, 4) NOT NULL,
	[lastupdatedate] [datetime] NOT NULL CONSTRAINT [DF_mf_daily_tracker_lastupdatedate] default(getdate()),
	[createdate] [datetime] NOT NULL CONSTRAINT [DF_mf_daily_tracker_createdate] default(getdate()),
 CONSTRAINT [PK_mf_daily_tracker] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

