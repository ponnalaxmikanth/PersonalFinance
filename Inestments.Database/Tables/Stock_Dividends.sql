CREATE TABLE [dbo].[Stock_Dividends]
(
	[Id] INT NOT NULL PRIMARY KEY,
	[StockID] [varchar](30) NOT NULL,
	[DividendDate] [Date] NULL,
	[Dividend] MONEY NULL DEFAULT(0), 
	[CreateDateTime] DATETIME CONSTRAINT [DF_Stock_Dividends_CreatedDate] DEFAULT (getdate()) NOT NULL,
    [LastUpdateDateTime] DATETIME CONSTRAINT [DF_Stock_Dividends_LastModifiedDate] DEFAULT (getdate()) NOT NULL,
)
