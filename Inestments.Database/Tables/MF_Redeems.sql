CREATE TABLE [dbo].[MF_Redeems](
	[TransactionId] [int] IDENTITY(1,1) NOT NULL,
	[PortfolioId] [int] NOT NULL,
	[FolioId] [int] NOT NULL,
	[FundId] [int] NOT NULL,
	[PurchaseDate] [date] NOT NULL,
	[Amount] [decimal](10, 4) NOT NULL,
	[PurchaseNAV] [decimal](10, 4) NOT NULL,
	[DividendPerNAV] [decimal](10, 4) NULL,
	[Dividend] [decimal](10, 4) NOT NULL,
	[ActualNAV] [decimal](10, 4) NULL,
	[SellDate] [date] NULL,
	[SellUnits] [decimal](10, 4) NULL,
	[SellNAV] [decimal](10, 4) NULL,
	[SellSTT] [decimal](10, 4) NULL,
	[Broker] [nvarchar](50) NULL,
	[CreateDate] [datetime] NOT NULL default(getdate()),
	[LastUpdateDateTime] [datetime] NULL default(getdate()),
	[IsSipInvestment] [nvarchar](1) NULL,
	[SIPID] [int] NULL,
 CONSTRAINT [PK_MF_Redeems] PRIMARY KEY CLUSTERED 
(
	[TransactionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[MF_Redeems]  WITH CHECK ADD  CONSTRAINT [FK_MF_Redeems_MF_Folios] FOREIGN KEY([FolioId])
REFERENCES [dbo].[MF_Folios] ([FolioId])
GO

ALTER TABLE [dbo].[MF_Redeems] CHECK CONSTRAINT [FK_MF_Redeems_MF_Folios]
GO

ALTER TABLE [dbo].[MF_Redeems]  WITH CHECK ADD  CONSTRAINT [FK_MF_Redeems_MF_Funds] FOREIGN KEY([FundId])
REFERENCES [dbo].[MF_Funds] ([FundId])
GO

ALTER TABLE [dbo].[MF_Redeems] CHECK CONSTRAINT [FK_MF_Redeems_MF_Funds]
GO

ALTER TABLE [dbo].[MF_Redeems]  WITH CHECK ADD  CONSTRAINT [FK_MF_Redeems_MF_Portfolios] FOREIGN KEY([PortfolioId])
REFERENCES [dbo].[MF_Portfolios] ([PortfolioId])
GO

ALTER TABLE [dbo].[MF_Redeems] CHECK CONSTRAINT [FK_MF_Redeems_MF_Portfolios]
GO

