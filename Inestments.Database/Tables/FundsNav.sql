CREATE TABLE [dbo].[FundsNav](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[SchemaCode] [int] NOT NULL,
	[Date] [datetime] NOT NULL,
	[ISINGrowth] [nvarchar](100) NULL,
	[ISIN_DIV_Reinvestment] [nvarchar](100) NULL,
	[SchemaName] [nvarchar](max) NULL,
	[FundType] [int] NULL,
	[FundOption] [int] NULL,
	[Fund_Type] [int] NULL,
	[NAV] [decimal](10, 4) NULL,
	[RepurchasePrice] [decimal](10, 4) NULL,
	[SalePrice] [decimal](10, 4) NULL,
	[CreateDateTime] [datetime] NOT NULL,
	[LastUpdateDateTime] [datetime] NOT NULL,
	[SchemeType] [varchar](20) NULL,
	[Scheme_Category] [varchar](20) NULL,
	[Scheme_Min_Amount] [varchar](20) NULL,
	[Closure_Date] [date] NULL,
	[Launch_Date] [date] NULL,
	[FundHouseId] [int] NULL,
 CONSTRAINT [PK_FundsNav] PRIMARY KEY CLUSTERED 
(
	[SchemaCode] ASC,
	[Date] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
