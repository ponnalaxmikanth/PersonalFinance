CREATE TABLE [dbo].[Stocks] (
    [Id]                 INT             IDENTITY (1, 1) NOT NULL,
    [StockID]            VARCHAR (30)    NOT NULL,
    [PurchaseDate]       DATE            NOT NULL,
    [Quantity]           INT             NOT NULL,
    [Price]              DECIMAL (18, 4) NOT NULL,
    [MarketPrice]        DECIMAL (18, 4) NOT NULL,
    [Dividend]           DECIMAL (18, 4) CONSTRAINT [DF_Stocks_Dividend] DEFAULT ((0)) NULL,
    [DividendPerStock]   DECIMAL (18, 4) NULL,
    [CreateDateTime]     DATETIME        CONSTRAINT [DF_Stocks_CreateDateTime] DEFAULT (getdate()) NOT NULL,
    [LastUpdateDateTime] DATETIME        CONSTRAINT [DF_Stocks_LastUpdateDateTime] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_Stocks_ID] PRIMARY KEY CLUSTERED ([Id] ASC)
);




GO

