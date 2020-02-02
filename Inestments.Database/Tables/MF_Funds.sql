CREATE TABLE [dbo].[MF_Funds](
	[FundId] [int] IDENTITY(1,1) NOT NULL,
	[FundHouseId] [int] NOT NULL,
	[FundTypeId] [int] NOT NULL,
	[FundClassId] [int] NOT NULL,
	[FundOptionId] [int] NOT NULL,
	[SchemaCode] [nvarchar](100) NULL,
	[GrowthSchemaCode] [nvarchar](100) NULL,
	[FundName] [nvarchar](500) NOT NULL,
	[CreatedDate] [datetime] CONSTRAINT [DF_MF_Funds_CreatedDate] DEFAULT (getdate()) NOT NULL,
	[LastModifiedDate] DATETIME CONSTRAINT [DF_MF_Funds_LastModifiedDate] DEFAULT (getdate()) NOT NULL,
 CONSTRAINT [PK_MF_Funds] PRIMARY KEY CLUSTERED 
(
	[FundId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[MF_Funds]  WITH CHECK ADD  CONSTRAINT [FK_MF_Funds_MF_FundCategory1] FOREIGN KEY([FundClassId])
REFERENCES [dbo].[MF_FundCategory] ([FundClassId])
GO

ALTER TABLE [dbo].[MF_Funds] CHECK CONSTRAINT [FK_MF_Funds_MF_FundCategory1]
GO

ALTER TABLE [dbo].[MF_Funds]  WITH CHECK ADD  CONSTRAINT [FK_MF_Funds_MF_FundHouses1] FOREIGN KEY([FundHouseId])
REFERENCES [dbo].[MF_FundHouses] ([FundHouseId])
GO

ALTER TABLE [dbo].[MF_Funds] CHECK CONSTRAINT [FK_MF_Funds_MF_FundHouses1]
GO

ALTER TABLE [dbo].[MF_Funds]  WITH CHECK ADD  CONSTRAINT [FK_MF_Funds_MF_FundOptions] FOREIGN KEY([FundOptionId])
REFERENCES [dbo].[MF_FundOptions] ([OptionId])
GO

ALTER TABLE [dbo].[MF_Funds] CHECK CONSTRAINT [FK_MF_Funds_MF_FundOptions]
GO

ALTER TABLE [dbo].[MF_Funds]  WITH CHECK ADD  CONSTRAINT [FK_MF_Funds_MF_FundTypes1] FOREIGN KEY([FundTypeId])
REFERENCES [dbo].[MF_FundTypes] ([FundTypeId])
GO

ALTER TABLE [dbo].[MF_Funds] CHECK CONSTRAINT [FK_MF_Funds_MF_FundTypes1]
GO

