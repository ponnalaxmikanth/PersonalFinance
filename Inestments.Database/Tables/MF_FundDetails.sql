CREATE TABLE [dbo].[MF_FundDetails](
	[SchemaCode] [int] NOT NULL,
	[ExpenseRatio] [decimal](10, 4) NULL,
	[Category] [nvarchar](50) NULL,
	[Status] [nvarchar](50) NULL,
	[TurnOver] [decimal](10, 4) NULL,
	[MinimumInvestment] [decimal](10, 4) NULL,
	[BenchMark] [nvarchar](100) NULL,
	[MSCategory] [nvarchar](100) NULL,
	[CreatedDatetime] [datetime] NOT NULL default(getdate()),
	[LastUpdateDateTime] [datetime] NOT NULL default(getdate()),
 CONSTRAINT [PK_MF_FundDetails] PRIMARY KEY CLUSTERED 
(
	[SchemaCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

