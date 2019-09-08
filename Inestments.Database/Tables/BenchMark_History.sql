CREATE TABLE [dbo].[BenchMark_History](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[BenchMarkId] [int] NOT NULL,
	[Date] [datetime] NOT NULL,
	[OpenValue] [decimal](18, 4) NULL,
	[HighValue] [decimal](18, 4) NULL,
	[LowValue] [decimal](18, 4) NULL,
	[CloseValue] [decimal](18, 4) NULL,
	[SharesTraded] [bigint] NULL,
	[TurnOver] [decimal](18, 4) NULL,
	[LastUpdateDate] [datetime] NOT NULL,
	[CreateDate] [datetime] NOT NULL,
 CONSTRAINT [PK_BenchMark_History] PRIMARY KEY CLUSTERED 
(
	[BenchMarkId] ASC,
	[Date] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


