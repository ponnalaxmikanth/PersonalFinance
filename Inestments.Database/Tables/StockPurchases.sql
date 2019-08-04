CREATE TABLE [dbo].[StockPurchases](
	[StockID] [varchar](30) NULL,
	[Purchasedatetime] [datetime] NULL,
	[No_of_Stocks] [int] NULL,
	[Stockprice] [float] NULL, 
    [CreateDateTime] DATETIME NOT NULL DEFAULT getdate(), 
    [LastModifiedDateTime] DATETIME NOT NULL DEFAULT getdate()
) ON [PRIMARY]

GO
