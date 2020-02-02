CREATE TABLE [dbo].[FundsNav_History](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[SchemaCode] [int] NOT NULL,
	[ISINGrowth] [nvarchar](100) NULL,
	[ISIN_DIV_Reinvestment] [nvarchar](100) NULL,
	[SchemaName] [nvarchar](max) NULL,
	[NAV] [decimal](10, 4) NULL,
	[RepurchasePrice] [decimal](10, 4) NULL,
	[SalePrice] [decimal](10, 4) NULL,
	[Date] [datetime] NOT NULL,
	[CreateDateTime] [datetime]  CONSTRAINT [DF_FundsNav_History_CreatedDate] DEFAULT (getdate()) NOT NULL,
	[LastUpdateDateTime] [datetime]  CONSTRAINT [DF_FundsNav_History_LastModifiedDate] DEFAULT (getdate()) NOT NULL,
	[FundType] [int] NULL,
	[FundOption] [int] NULL,
	[Fund_Type] [int] NULL,
 CONSTRAINT [PK_FundsNav_History] PRIMARY KEY CLUSTERED 
(
	[SchemaCode] ASC,
	[Date] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO


