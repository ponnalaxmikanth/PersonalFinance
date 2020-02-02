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
    [CreateDateTime] DATETIME CONSTRAINT [DF_Stock_Redeems_CreatedDate] DEFAULT (getdate()) NOT NULL,
    [LastUpdateDateTime] DATETIME CONSTRAINT [DF_Stock_Redeems_LastModifiedDate] DEFAULT (getdate()) NOT NULL,
)
