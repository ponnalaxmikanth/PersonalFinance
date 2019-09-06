CREATE TABLE [dbo].[Stock_Redeems]
(
	[Id] INT NOT NULL PRIMARY KEY,
	[StockID] [varchar](30) NOT NULL,
	[PurchaseDate] [Date] NOT NULL,
	[Quantity] [int] NOT NULL,
	[Price] MONEY NOT NULL, 
	[Dividend] MONEY NULL DEFAULT(0), 
	[SellDate] [Date] NOT NULL,
	[SellPrice] MONEY NOT NULL,
    [CreateDateTime] DATETIME NOT NULL DEFAULT getdate(), 
    [LastUpdateDateTime] DATETIME NOT NULL DEFAULT getdate()
)
