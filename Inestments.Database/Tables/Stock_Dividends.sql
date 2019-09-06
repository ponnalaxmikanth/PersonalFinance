CREATE TABLE [dbo].[Stock_Dividends]
(
	[Id] INT NOT NULL PRIMARY KEY,
	[StockID] [varchar](30) NOT NULL,
	[DividendDate] [Date] NULL,
	[Dividend] MONEY NULL DEFAULT(0), 
	[CreateDateTime] DATETIME NOT NULL DEFAULT getdate(), 
    [LastUpdateDateTime] DATETIME NOT NULL DEFAULT getdate()
)
