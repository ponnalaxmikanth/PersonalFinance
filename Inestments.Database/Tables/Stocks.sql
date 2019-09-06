CREATE TABLE [dbo].[Stocks] (
	[Id] INT NOT NULL,
	[StockID] [varchar](30) NOT NULL,
	[PurchaseDate] [Date] NOT NULL,
	[Quantity] [int] NOT NULL,
	[Price] MONEY NOT NULL, 
	[MarketPrice] MONEY NOT NULL,
	 [Dividend]           MONEY        CONSTRAINT [DF_Stocks_Dividend] DEFAULT ((0)) NULL,
    [CreateDateTime]     DATETIME     CONSTRAINT [DF_Stocks_CreateDateTime] DEFAULT (getdate()) NOT NULL,
    [LastUpdateDateTime] DATETIME     CONSTRAINT [DF_Stocks_LastUpdateDateTime] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_Stocks_ID] PRIMARY KEY CLUSTERED ([Id] ASC)
);
GO

