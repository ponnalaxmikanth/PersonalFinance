CREATE TABLE [dbo].[MF_FundDetails](
	[SchemaCode] [int] NOT NULL,
	[ExpenseRatio] [decimal](10, 4) NULL,
	[Category] [nvarchar](50) NULL,
	[Status] [nvarchar](50) NULL,
	[TurnOver] [decimal](10, 4) NULL,
	[MinimumInvestment] [decimal](10, 4) NULL,
	[BenchMark] [nvarchar](100) NULL,
	[MSCategory] [nvarchar](100) NULL,
	[CreatedDatetime] [datetime]  CONSTRAINT [DF_MF_FundDetails_CreatedDate] DEFAULT (getdate()) NOT NULL,
	[LastUpdateDateTime] [datetime]CONSTRAINT [DF_MF_FundDetails_LastModifiedDate] DEFAULT (getdate()) NOT NULL,
 CONSTRAINT [PK_MF_FundDetails] PRIMARY KEY CLUSTERED 
(
	[SchemaCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

