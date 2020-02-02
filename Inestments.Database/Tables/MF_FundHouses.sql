CREATE TABLE [dbo].[MF_FundHouses](
	[FundHouseId] [int] IDENTITY(1,1) NOT NULL,
	[FundHouseName] [nvarchar](100) NOT NULL,
	[DsiplayName] [nvarchar](100) NULL,
	[Site] [nvarchar](100) NULL,
	[Description] [nvarchar](100) NULL,
	[CreatedDate] [datetime] CONSTRAINT [DF_MF_FundHouses_CreatedDate] DEFAULT (getdate()) NOT NULL,
	[LastModifiedDate] DATETIME CONSTRAINT [DF_MF_FundHouses_LastModifiedDate] DEFAULT (getdate()) NOT NULL,
 CONSTRAINT [PK_FundHouseId] PRIMARY KEY CLUSTERED 
(
	[FundHouseId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

