CREATE TABLE [dbo].[MF_Purchases](
	[TransactionId] [int] IDENTITY(1,1) NOT NULL,
	[PortfolioId] [int] NOT NULL,
	[FolioId] [int] NOT NULL,
	[FundId] [int] NOT NULL,
	[PurchaseDate] [date] NOT NULL,
	[Amount] [decimal](10, 4) NOT NULL,
	[PurchaseNAV] [decimal](10, 4) NOT NULL,
	[Units] [decimal](10, 4) NOT NULL,
	[DividendPerNAV] [decimal](10, 4) NULL,
	[Dividend] [decimal](10, 4) NULL,
	[ActualNAV] [decimal](10, 4) NULL,
	[Broker] [nvarchar](50) NULL,
	[CreateDate] [datetime] NOT NULL CONSTRAINT [DF_MF_Purchases_CreatedDate] default(getdate()),
	[LastUpdateDateTime] [datetime] NULL CONSTRAINT [DF_MF_Purchases_LastUpdateDateTime] default(getdate()),
	[IsSipInvestment] [nvarchar](1) NULL,
	[SIPID] [int] NULL,
 CONSTRAINT [PK_MF_Purchases] PRIMARY KEY CLUSTERED 
(
	[TransactionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[MF_Purchases]  WITH CHECK ADD  CONSTRAINT [FK_MF_Purchases_MF_Folios] FOREIGN KEY([FolioId])
REFERENCES [dbo].[MF_Folios] ([FolioId])
GO

ALTER TABLE [dbo].[MF_Purchases] CHECK CONSTRAINT [FK_MF_Purchases_MF_Folios]
GO

ALTER TABLE [dbo].[MF_Purchases]  WITH CHECK ADD  CONSTRAINT [FK_MF_Purchases_MF_Funds] FOREIGN KEY([FundId])
REFERENCES [dbo].[MF_Funds] ([FundId])
GO

ALTER TABLE [dbo].[MF_Purchases] CHECK CONSTRAINT [FK_MF_Purchases_MF_Funds]
GO

ALTER TABLE [dbo].[MF_Purchases]  WITH CHECK ADD  CONSTRAINT [FK_MF_Purchases_MF_Portfolios] FOREIGN KEY([PortfolioId])
REFERENCES [dbo].[MF_Portfolios] ([PortfolioId])
GO

ALTER TABLE [dbo].[MF_Purchases] CHECK CONSTRAINT [FK_MF_Purchases_MF_Portfolios]
GO

