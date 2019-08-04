CREATE TABLE [dbo].[FundSwitches](
	[transactionId] [int] IDENTITY(1,1) NOT NULL,
	[switchDate] [date] NOT NULL,
	[AMCId] [int] NOT NULL,
	[SchemaCode] [int] NOT NULL,
	[Units] [decimal](10, 4) NOT NULL,
	[NAV] [decimal](10, 4) NOT NULL,
	[ToAMCId] [int] NOT NULL,
	[ToSchemaCode] [int] NOT NULL,
	[ToUnits] [decimal](10, 4) NOT NULL,
	[PurchaseNAV] [decimal](10, 4) NOT NULL,
	[CreateDate] [datetime] NOT NULL default(getdate()),
 CONSTRAINT [PK_FundSwitches] PRIMARY KEY CLUSTERED 
(
	[transactionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[FundSwitches]  WITH CHECK ADD  CONSTRAINT [FK_FundSwitches_FundSwitches] FOREIGN KEY([transactionId])
REFERENCES [dbo].[FundSwitches] ([transactionId])
GO

ALTER TABLE [dbo].[FundSwitches] CHECK CONSTRAINT [FK_FundSwitches_FundSwitches]
GO

ALTER TABLE [dbo].[FundSwitches]  WITH CHECK ADD  CONSTRAINT [FK_FundSwitches_MF_From_FundHouses] FOREIGN KEY([AMCId])
REFERENCES [dbo].[MF_FundHouses] ([FundHouseId])
GO

ALTER TABLE [dbo].[FundSwitches] CHECK CONSTRAINT [FK_FundSwitches_MF_From_FundHouses]
GO

ALTER TABLE [dbo].[FundSwitches]  WITH CHECK ADD  CONSTRAINT [FK_FundSwitches_MF_To_FundHouses] FOREIGN KEY([ToAMCId])
REFERENCES [dbo].[MF_FundHouses] ([FundHouseId])
GO

ALTER TABLE [dbo].[FundSwitches] CHECK CONSTRAINT [FK_FundSwitches_MF_To_FundHouses]
GO

